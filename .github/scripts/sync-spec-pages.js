// Confluenceの「仕様書」フォルダ配下のページ一覧を取得し、
// .github/spec-pages.json (source: "confluence" の項目) と
// .github/ISSUE_TEMPLATE/feature_report.yml の options (AUTO-GENERATED spec-pages の範囲) を更新する。
//
// 必要な環境変数:
//   CONFLUENCE_BASE_URL   例: https://summervacationgamejam.atlassian.net
//   CONFLUENCE_EMAIL      APIトークンを発行したAtlassianアカウントのメールアドレス
//   CONFLUENCE_API_TOKEN  https://id.atlassian.com/manage-profile/security/api-tokens で発行
//   CONFLUENCE_SPACE_KEY  例: MFS
//   CONFLUENCE_FOLDER_ID  仕様書フォルダのID 例: 65812
//
// 注意: 「フォルダー」型コンテンツの子ページ取得APIは比較的新しいConfluence機能のため、
// 実際にAtlassian環境で一度実行してエンドポイントが想定通り動くか確認すること。
// 失敗した場合はエラーメッセージに従ってエンドポイントを見直す。

const fs = require("fs");
const path = require("path");

const REPO_ROOT = path.resolve(__dirname, "..", "..");
const SPEC_PAGES_JSON = path.join(REPO_ROOT, ".github", "spec-pages.json");
const FEATURE_REPORT_YML = path.join(REPO_ROOT, ".github", "ISSUE_TEMPLATE", "feature_report.yml");

const BEGIN_MARKER = "# BEGIN AUTO-GENERATED spec-pages";
const END_MARKER = "# END AUTO-GENERATED spec-pages";

function requireEnv(name) {
  const value = process.env[name];
  if (!value) {
    throw new Error(`環境変数 ${name} が設定されていません`);
  }
  return value;
}

async function fetchConfluenceFolderChildren({ baseUrl, email, apiToken, folderId }) {
  const auth = Buffer.from(`${email}:${apiToken}`).toString("base64");
  const headers = {
    Authorization: `Basic ${auth}`,
    Accept: "application/json",
  };

  // 候補1: v2 API の pages 一覧を parent-id で絞り込む
  const candidateUrls = [
    `${baseUrl}/wiki/api/v2/pages?parent-id=${folderId}&limit=100`,
    `${baseUrl}/wiki/api/v2/folders/${folderId}/direct-children?limit=100`,
  ];

  let lastError = null;
  for (const url of candidateUrls) {
    try {
      const res = await fetch(url, { headers });
      if (!res.ok) {
        lastError = new Error(`${url} -> HTTP ${res.status}: ${await res.text()}`);
        continue;
      }
      const json = await res.json();
      const results = json.results || [];
      if (results.length === 0 && candidateUrls.indexOf(url) === 0) {
        // 空でも成功扱い(フォルダ配下が本当に空の可能性があるため)だが、
        // 念のため次の候補も試さずここで確定させる
      }
      return results.map((item) => ({
        id: item.id,
        title: item.title,
      }));
    } catch (err) {
      lastError = err;
    }
  }

  throw new Error(
    `Confluence APIから子ページ一覧を取得できませんでした。エンドポイントの見直しが必要な可能性があります。最後のエラー: ${lastError}`
  );
}

function loadSpecPages() {
  return JSON.parse(fs.readFileSync(SPEC_PAGES_JSON, "utf8"));
}

function saveSpecPages(data) {
  fs.writeFileSync(SPEC_PAGES_JSON, JSON.stringify(data, null, 2) + "\n", "utf8");
}

function regenerateFeatureReportOptions(titles) {
  const content = fs.readFileSync(FEATURE_REPORT_YML, "utf8");
  const lines = content.split("\n");

  const beginIdx = lines.findIndex((l) => l.includes(BEGIN_MARKER));
  const endIdx = lines.findIndex((l) => l.includes(END_MARKER));

  if (beginIdx === -1 || endIdx === -1 || endIdx <= beginIdx) {
    throw new Error(
      `feature_report.yml に ${BEGIN_MARKER} / ${END_MARKER} のマーカーが見つかりませんでした`
    );
  }

  const indent = lines[beginIdx].match(/^(\s*)/)[1];
  const newOptionLines = titles.map((t) => `${indent}- ${t}`);

  const newLines = [
    ...lines.slice(0, beginIdx + 1),
    ...newOptionLines,
    ...lines.slice(endIdx),
  ];

  fs.writeFileSync(FEATURE_REPORT_YML, newLines.join("\n"), "utf8");
}

async function main() {
  const baseUrl = requireEnv("CONFLUENCE_BASE_URL");
  const email = requireEnv("CONFLUENCE_EMAIL");
  const apiToken = requireEnv("CONFLUENCE_API_TOKEN");
  const spaceKey = requireEnv("CONFLUENCE_SPACE_KEY");
  const folderId = requireEnv("CONFLUENCE_FOLDER_ID");

  console.log(`Confluenceフォルダ ${folderId} の子ページを取得します...`);
  const children = await fetchConfluenceFolderChildren({ baseUrl, email, apiToken, folderId });
  console.log(`取得件数: ${children.length}`);
  children.forEach((c) => console.log(`  - ${c.title} (id=${c.id})`));

  const data = loadSpecPages();
  const manualPages = data.pages.filter((p) => p.source !== "confluence");
  const confluencePages = children.map((c) => ({
    title: c.title,
    url: `${baseUrl}/wiki/spaces/${spaceKey}/pages/${c.id}`,
    source: "confluence",
  }));

  const beforeTitles = new Set(data.pages.filter((p) => p.source === "confluence").map((p) => p.title));
  const afterTitles = new Set(confluencePages.map((p) => p.title));
  const added = [...afterTitles].filter((t) => !beforeTitles.has(t));
  const removed = [...beforeTitles].filter((t) => !afterTitles.has(t));

  data.pages = [...manualPages, ...confluencePages];
  saveSpecPages(data);

  regenerateFeatureReportOptions(data.pages.map((p) => p.title));

  console.log(`追加された項目: ${added.length ? added.join(", ") : "なし"}`);
  console.log(`削除された項目: ${removed.length ? removed.join(", ") : "なし"}`);
}

main().catch((err) => {
  console.error(err);
  process.exit(1);
});
