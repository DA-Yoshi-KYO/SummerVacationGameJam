# Stack PR Builder

1つの大きなブランチ(PR)のコミットを、GUI操作だけでネイティブの GitHub Stacked PR に分割するためのツールです。
`gh stack modify` の対話TUIは使わず、`gh stack init` が複数の既存ブランチを非対話で一括登録できる仕様を利用しています。

## フォルダ構成

```
Tools/StackPrBuilder/
  develop/   ソースコード。改造・ビルドはここで行う
  exe/       配布用の単一exe(ビルド済み)。使うだけの人はここだけでOK
```

## 使うだけの人向け (ビルド不要)

1. [`../exe/StackPrBuilder.exe`](../exe/StackPrBuilder.exe) をダブルクリックして起動
2. 事前に **.NET 8 ランタイム(x64)** が必要です。無い場合は以下からインストールしてください
   (Visual StudioやUnityでC#開発をしているPCなら既に入っていることが多いです)
   - https://dotnet.microsoft.com/download/dotnet/8.0 → 「.NET Desktop Runtime 8.0.x (x64)」
3. [GitHub CLI (`gh`)](https://cli.github.com/) をインストールし `gh auth login`
4. `gh extension install github/gh-stack`
5. あとは下記の「使い方」を参照

## 開発者向け (改造・ビルド)

### 前提条件

- Visual Studio 2022 に **「.NET デスクトップ開発」ワークロード**が入っていること
  (Visual Studio Installer → 個別のコンポーネント で確認・追加できます)
- GitHub CLI (`gh`) + `gh extension install github/gh-stack`

### ビルド

`develop/StackPrBuilder.csproj` を VS2022 で開いてビルド、または:

```bash
cd Tools/StackPrBuilder/develop
dotnet build
dotnet run
```

### 配布用exeの再発行

ソースを変更したら、`exe/` フォルダの中身も更新してください(軽量・フレームワーク依存のシングルファイル):

```bash
cd Tools/StackPrBuilder/develop
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o ../exe
```

> `--self-contained true` にすると.NETランタイム同梱で他のPCでも単体で動きますが、
> 生成物が150MB超になりgitと相性が悪いため、このリポジトリでは**フレームワーク依存(軽量)**を採用しています。

## 使い方

1. アプリを起動 (`../exe/StackPrBuilder.exe` またはVS2022から実行)
2. 起動時に「リポジトリパス」は自動検出を試みます(見つからなければ手入力)
3. **Base** に分岐元のブランチ(例: `develop`)、**分割対象ブランチ** に元々の大きなPRのブランチ名を指定
4. 「Fetch」または「① コミット読み込み」(こちらは自動fetch付き) → コミット一覧が古い→新しい順に表示される
5. **(任意) 並び替え**: 行を選択して「↑/↓」で表示順を入れ替え、「並び替えを確定」でその順序に
   cherry-pickして実際にコミットを再構築する(SHAは新しくなります)。コンフリクトが起きた場合は自動処理せず中断します。
   - 作業ツリーを一時的にbaseへ切り替えて処理するため、**未コミットの変更が無い状態**で実行してください。
   - Unity Editorを開いたままだと再インポートが走る可能性があるため、閉じてからの実行を推奨します。
6. 各コミットの「レイヤー番号」列を編集して、どのPRに属するかをグループ分けする
   - 例: `PR1(コミットA) / PR2(コミットB) / PR3(コミットC,D,E)` なら、A=1, B=2, C=3, D=3, E=3
   - 番号は表示順(並び替え後の順)で**増加方向のみ**(グループ化であり並び替えではない)
7. 「② レイヤー計算」→ グループごとのブランチ候補が下に表示される。
   ブランチ名は `stacked/大要素/要素` の形式で、`stacked/` は固定(このリポジトリでは
   `stacked/**` 以外のブランチ作成がRulesetで禁止されているため編集不可)。
   - **大要素**: このスタック全体の分類名(例: `チップ`)。②の左上の入力欄に入れて
     「全レイヤーへ一括反映」を押すと、全レイヤーへまとめて設定できる
   - **要素**: 各レイヤー固有の内容(例: `BaseCreate`)。デフォルトは `layer-N` だが自由に編集可
8. 「③ Stack作成」→ 内部で以下を自動実行:
   - 各レイヤーの境目にローカルブランチを作成
   - 各ブランチを `git push`
   - `gh stack init --base <base> <branch1> <branch2> ...` (非対話・一括登録)
   - `gh stack submit --auto` (PRを作成してスタックとして連結)
   - チェックボックスがONなら、分割元PRにコメントを付けて自動クローズ
9. 以降、下位レイヤーに修正が入ったら **「Sync」**ボタン(`gh stack sync --prune`)で自動リベース連鎖
10. レビューが揃ったら **「Merge All」**ボタン(`gh stack merge -y`)で一括マージ

## 構成 (develop/)

```
StackPrBuilder.csproj      WPFアプリ本体 (net8.0-windows)
App.xaml / App.xaml.cs
MainWindow.xaml(.cs)       画面とイベントハンドラ
Models/
  CommitInfo.cs            コミット1件 + レイヤー番号
  StackLayerPlan.cs        レイヤー(=作成するブランチ)1件分
  PullRequestSummary.cs    `gh pr list` の1件分
  ReorderResult.cs         並び替え(cherry-pick再構築)の結果
Services/
  CliRunner.cs             git/gh を子プロセス実行する共通ラッパー
  GitService.cs            LibGit2Sharpでのコミット読み取り・ブランチ作成・並び替え
  GitHubService.cs         `gh pr list --json ...` のラップ
  StackOrchestrator.cs     fetch → push → gh stack init → gh stack submit / sync / merge / PRクローズ
```

## 既知の制約

- push・`gh` 認証は、実行しているPCの既存の資格情報(Git Credential Manager / `gh auth login`)を
  そのまま利用します。ツール内でパスワードやトークンを入力する画面はありません。
- 並び替え確定(cherry-pick再構築)は、コンフリクトが起きた場合は自動解決しません。その並び順は
  諦めて手動で調整するか、`git rebase -i` 等で事前に整理してから読み込んでください。
