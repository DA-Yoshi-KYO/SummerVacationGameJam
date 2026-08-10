// Confluenceページが更新された際、ページ内の「変更履歴」見出し直下にある表を
// 更新前後のバージョンで比較し、新しく追加された行だけをDiscordに通知する。
//
// 必要な環境変数:
//   CONFLUENCE_BASE_URL   例: https://summervacationgamejam.atlassian.net
//   CONFLUENCE_EMAIL      APIトークンを発行したAtlassianアカウントのメールアドレス
//   CONFLUENCE_API_TOKEN  https://id.atlassian.com/manage-profile/security/api-tokens で発行
//   CONFLUENCE_PAGE_ID    通知対象のページID(Confluence Automationから渡される)
//   DISCORD_WEBHOOK_URL   通知先のDiscord Webhook URL
//   CHANGELOG_HEADING     変更履歴表の直前にある見出しの文字列(既定値: "仕様書の変更ログ")
//
// 実装メモ:
// - Confluence REST API v1 の `content` エンドポイントに `version` パラメータを付けると、
//   指定バージョン時点の本文を取得できる(要現地確認・現状ドキュメント上の挙動に基づく実装)。
// - 表の「新規行」判定は、行数が増えていたら末尾に増えた分を新規行とみなす単純な方式。
//   途中への挿入・行の並び替え・削除には対応しない。

const REQUIRED = [
  "CONFLUENCE_BASE_URL",
  "CONFLUENCE_EMAIL",
  "CONFLUENCE_API_TOKEN",
  "CONFLUENCE_PAGE_ID",
  "DISCORD_WEBHOOK_URL",
];

function requireEnv(name) {
  const value = process.env[name];
  if (!value) {
    throw new Error(`環境変数 ${name} が設定されていません`);
  }
  return value;
}

function stripTags(html) {
  return html
    .replace(/<[^>]*>/g, "")
    .replace(/&nbsp;/g, " ")
    .replace(/&amp;/g, "&")
    .replace(/&lt;/g, "<")
    .replace(/&gt;/g, ">")
    .replace(/&quot;/g, '"')
    .replace(/&#39;/g, "'")
    .replace(/\s+/g, " ")
    .trim();
}

// 見出しテキストの直後にある最初の <table>...</table> を取り出す
function extractSectionTable(html, headingText) {
  const headingRe = /<h[1-6][^>]*>(.*?)<\/h[1-6]>/gis;
  let match;
  let sectionStart = -1;
  while ((match = headingRe.exec(html)) !== null) {
    if (stripTags(match[1]) === headingText) {
      sectionStart = match.index + match[0].length;
      break;
    }
  }
  if (sectionStart === -1) {
    return null;
  }

  const tableStart = html.indexOf("<table", sectionStart);
  if (tableStart === -1) {
    return null;
  }
  const tableEndTagIdx = html.indexOf("</table>", tableStart);
  if (tableEndTagIdx === -1) {
    return null;
  }
  return html.slice(tableStart, tableEndTagIdx + "</table>".length);
}

// <tr> ごとに { isHeader, cells } を返す。<th> を含む行はヘッダー行とみなす
function parseTableRows(tableHtml) {
  const rows = [];
  const rowRe = /<tr[^>]*>(.*?)<\/tr>/gis;
  let rowMatch;
  while ((rowMatch = rowRe.exec(tableHtml)) !== null) {
    const rowHtml = rowMatch[1];
    const cellRe = /<t[hd][^>]*>(.*?)<\/t[hd]>/gis;
    const cells = [];
    let isHeader = false;
    let cellMatch;
    while ((cellMatch = cellRe.exec(rowHtml)) !== null) {
      cells.push(stripTags(cellMatch[1]));
    }
    if (/<th[\s>]/i.test(rowHtml)) {
      isHeader = true;
    }
    if (cells.length > 0) {
      rows.push({ isHeader, cells });
    }
  }
  return rows;
}

async function fetchPageVersion({ baseUrl, headers, pageId, version }) {
  const versionParam = version ? `&version=${version}&status=any` : "";
  const url = `${baseUrl}/wiki/rest/api/content/${pageId}?expand=body.storage,version,space${versionParam}`;
  const res = await fetch(url, { headers });
  if (!res.ok) {
    throw new Error(`${url} -> HTTP ${res.status}: ${await res.text()}`);
  }
  return res.json();
}

function rowKey(cells) {
  return cells.join("");
}

async function postToDiscord({ webhookUrl, pageTitle, pageUrl, headerRow, newRows }) {
  const labels = headerRow ? headerRow.cells : null;

  const blocks = newRows.map((row) => {
    if (labels && labels.length === row.cells.length) {
      return row.cells.map((v, i) => `**${labels[i]}**: ${v || "(空欄)"}`).join("\n");
    }
    return row.cells.join(" / ");
  });

  const content =
    `📝 **${pageTitle}** の変更履歴に新しい行が追加されました\n${pageUrl}\n\n` +
    blocks.map((b) => `---\n${b}`).join("\n");

  const res = await fetch(webhookUrl, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ content: content.slice(0, 1900) }),
  });
  if (!res.ok) {
    throw new Error(`Discordへの送信に失敗しました: HTTP ${res.status} ${await res.text()}`);
  }
}

async function main() {
  for (const name of REQUIRED) requireEnv(name);

  const baseUrl = process.env.CONFLUENCE_BASE_URL;
  const email = process.env.CONFLUENCE_EMAIL;
  const apiToken = process.env.CONFLUENCE_API_TOKEN;
  const pageId = process.env.CONFLUENCE_PAGE_ID;
  const webhookUrl = process.env.DISCORD_WEBHOOK_URL;
  const heading = process.env.CHANGELOG_HEADING || "仕様書の変更ログ";

  const auth = Buffer.from(`${email}:${apiToken}`).toString("base64");
  const headers = { Authorization: `Basic ${auth}`, Accept: "application/json" };

  console.log(`ページ ${pageId} の現在バージョンを取得します...`);
  const current = await fetchPageVersion({ baseUrl, headers, pageId });
  const currentVersion = current.version.number;
  console.log(`現在バージョン: ${currentVersion}`);

  if (currentVersion <= 1) {
    console.log("初版のため比較対象の旧バージョンがありません。終了します。");
    return;
  }

  const previous = await fetchPageVersion({
    baseUrl,
    headers,
    pageId,
    version: currentVersion - 1,
  });

  const currentHtml = current.body.storage.value;
  const previousHtml = previous.body.storage.value;

  const currentTable = extractSectionTable(currentHtml, heading);
  const previousTable = extractSectionTable(previousHtml, heading);

  if (!currentTable) {
    console.log(`見出し「${heading}」の表が現在のページに見つかりませんでした。終了します。`);
    return;
  }

  const currentRows = parseTableRows(currentTable);
  const previousRows = previousTable ? parseTableRows(previousTable) : [];

  const currentHeader = currentRows.find((r) => r.isHeader) || null;
  const currentData = currentRows.filter((r) => !r.isHeader);
  const previousData = previousRows.filter((r) => !r.isHeader);

  console.log(`現在の行数: ${currentData.length} / 直前バージョンの行数: ${previousData.length}`);

  if (currentData.length <= previousData.length) {
    console.log("新規に追加された行はありませんでした(行数が増えていません)。");
    return;
  }

  // 直前バージョンに存在した行(内容一致)を除いた、末尾側の増分を新規行とみなす
  const previousKeys = new Set(previousData.map((r) => rowKey(r.cells)));
  const tailCandidates = currentData.slice(previousData.length);
  const newRows = tailCandidates.filter((r) => !previousKeys.has(rowKey(r.cells)));

  if (newRows.length === 0) {
    console.log("末尾の行が直前バージョンと同一内容のため、新規行なしと判断しました。");
    return;
  }

  const pageUrl = `${current._links.base}${current._links.webui}`;

  console.log(`新規行 ${newRows.length} 件をDiscordに通知します。`);
  await postToDiscord({
    webhookUrl,
    pageTitle: current.title,
    pageUrl,
    headerRow: currentHeader,
    newRows,
  });
  console.log("通知しました。");
}

main().catch((err) => {
  console.error(err);
  process.exit(1);
});
