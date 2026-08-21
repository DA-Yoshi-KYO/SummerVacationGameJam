# Stack PR Builder

1つの大きなブランチ(PR)のコミットを、GUI操作だけでネイティブの GitHub Stacked PR に分割するためのツールです。
`gh stack modify` の対話TUIは使わず、`gh stack init` が複数の既存ブランチを非対話で一括登録できる仕様を利用しています。

## 前提条件

- Visual Studio 2022 に **「.NET デスクト開発」ワークロード**が入っていること
  (Visual Studio Installer → 個別のコンポーネント で確認・追加できます)
- [GitHub CLI (`gh`)](https://cli.github.com/) がインストール済み、`gh auth login` 済みであること
- `gh extension install github/gh-stack` 済みであること
- 対象リポジトリの clone がローカルにあり、分割したいコミット群が1本のブランチに乗っていること

> **注意**: このツールはこの開発環境(サンドボックス)には .NET SDK ワークロードが入っておらず、
> ビルド検証ができていません。お手元の VS2022 で `StackPrBuilder.csproj` を開いてビルドしてください。
> `Microsoft.NET.Sdk を解決できませんでした` というエラーが出る場合は、上記ワークロードの追加が必要です。

## 使い方

1. `StackPrBuilder.csproj` を Visual Studio 2022 で開く(または `dotnet run` )
2. 起動時に「リポジトリパス」は自動検出を試みます(見つからなければ手入力)
3. **Base** に分岐元のブランチ(例: `develop`)、**分割対象ブランチ** に元々の大きなPRのブランチ名を指定
4. 「① コミット読み込み」→ コミット一覧が古い→新しい順に表示される
5. 各コミットの「レイヤー番号」列を編集して、どのPRに属するかをグループ分けする
   - 例: `PR1(コミットA) / PR2(コミットB) / PR3(コミットC,D,E)` なら、A=1, B=2, C=3, D=3, E=3
   - 番号は履歴順で**増加方向のみ**(並び替えではなく区切りのみ)
6. 「② レイヤー計算」→ グループごとのブランチ候補が下に表示される。ブランチ名は自由に編集可
7. 「③ Stack作成」→ 内部で以下を自動実行:
   - 各レイヤーの境目にローカルブランチを作成
   - 各ブランチを `git push`
   - `gh stack init --base <base> <branch1> <branch2> ...` (非対話・一括登録)
   - `gh stack submit --auto` (PRを作成してスタックとして連結)
8. 以降、下位レイヤーに修正が入ったら **「Sync」**ボタン(`gh stack sync --prune`)で自動リベース連鎖
9. レビューが揃ったら **「Merge All」**ボタン(`gh stack merge -y`)で一括マージ

## 構成

```
StackPrBuilder.csproj      WPFアプリ本体 (net8.0-windows)
App.xaml / App.xaml.cs
MainWindow.xaml(.cs)       画面とイベントハンドラ
Models/
  CommitInfo.cs            コミット1件 + レイヤー番号
  StackLayerPlan.cs        レイヤー(=作成するブランチ)1件分
  PullRequestSummary.cs    `gh pr list` の1件分
Services/
  CliRunner.cs             git/gh を子プロセス実行する共通ラッパー
  GitService.cs            LibGit2Sharpでのコミット読み取り・ブランチ作成
  GitHubService.cs         `gh pr list --json ...` のラップ
  StackOrchestrator.cs     push → gh stack init → gh stack submit / sync / merge
```

## 既知の制約

- コミットの**並び替え**には未対応です(区切り=グループ化のみ)。並び替えが必要な場合は事前に
  `git rebase -i` などで整理してから読み込んでください。
- push・`gh` 認証は、実行しているPCの既存の資格情報(Git Credential Manager / `gh auth login`)を
  そのまま利用します。ツール内でパスワードやトークンを入力する画面はありません。
