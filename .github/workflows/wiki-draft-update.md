---
on:
  push:
    branches: [develop]
    paths:
      - 'Assets/Scripts/**/*.cs'
  workflow_dispatch:

permissions:
  contents: read
  pull-requests: read
  copilot-requests: write

engine: copilot

safe-outputs:
  create-pull-request:
    base-branch: "develop"
    preserve-branch-name: true
    allowed-branches:
      - "秋野翔太/*"

tools:
  github:
    toolsets: [pull_requests]

---

# Wikiドラフト自動生成

このリポジトリの `Assets/Scripts/**/*.cs` が変更されたら、変更内容を要約したGitHub Wikiページの下書きを作成してください。

## 手順

1. 今回のpush(またはworkflow_dispatchの場合は直近のコミット)で変更された `.cs` ファイルを確認する。
2. **ファイル単位でページを作らないこと。** まずファイル名から共通の対象(主語)を推測してグループ化する。
   - 例: `CS_PlayerMove.cs`, `CS_PlayerAction.cs`, `CS_PlayerLook.cs` は、ファイル名に共通して含まれる「Player」から「プレイヤー」という1つの対象に属すると判断し、まとめて1つのWikiページにする。
   - 判断基準はクラス名/ファイル名に含まれる共通の名詞(Player, Enemy, Weapon, UI など)。命名規則から対象が推測できない場合のみ、ファイル名をそのまま対象名にする。
   - **重要**: ページ本文は今回の差分ファイルだけでなく、同じ対象に属する既存の全ファイルを`Assets/Scripts/`配下から探して読み、その内容も含めて作成すること(例: 今回`CS_PlayerAction.cs`だけが変更された場合でも、既存の`CS_PlayerMove.cs`や`CS_PlayerLook.cs`も読み、プレイヤーに関する完全なページを作る)。Wikiページは反映時に丸ごと上書きされるため、差分ファイルの内容だけで作ると過去に記載されていた他ファイルの情報が失われる。
   - 対象ごとに、クラス名・主要メソッドの役割・シリアライズされたパラメータ(`[SerializeField]`等)をもとに、日本語でWikiページ本文を作成する。見出し構成は「概要 / 実装物 / 機能説明」のような形にする。
3. 生成した本文の先頭に、以下の形式のフロントマターを付与する。`target_page` には実際のWikiページ名を、カテゴリ付き(`:`区切り)でそのまま指定すること(例: `実装物:プレイヤー`)。
   ```
   ---
   target_page: "実装物:<対象>"
   ---
   ```
4. ファイルを `wiki-drafts/<対象を表すファイル名>.md` として保存する。**ファイル名自体に`:`を使わないこと**(対象名だけをファイル名にする。例: `wiki-drafts/プレイヤー.md`)。1つの変更セットに複数の対象が含まれる場合は、対象ごとに別ファイルにする。
5. `wiki-drafts/` 配下に既に同じ対象のファイルがある場合は、新しい内容で上書き更新する。
6. これらのファイルを追加/更新するプルリクエストを作成する。ブランチ名は `秋野翔太/` で始め、その後にスラッシュを含まない1つのセグメントだけを続けること(例: `秋野翔太/八咫烏Wiki更新`)。**`秋野翔太/八咫烏/更新` のようにスラッシュを2つ以上含む名前は、このリポジトリのブランチ作成ルールで拒否されるため絶対に使わないこと。**

## 注意

- Wikiリポジトリ(このリポジトリとは別のgitリポジトリ)には一切書き込まないこと。あくまでこのリポジトリ内の `wiki-drafts/` フォルダにファイルを追加/更新するだけでよい。実際のWikiへの反映は別の仕組み(社内ツール)が行う。
- `wiki-drafts/` 以外のファイルは変更しないこと。
- PRのベースブランチは必ず `develop` にすること(`main`ではない)。
