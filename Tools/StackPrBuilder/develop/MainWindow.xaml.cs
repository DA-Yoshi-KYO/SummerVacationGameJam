using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using StackPrBuilder.Models;
using StackPrBuilder.Services;

namespace StackPrBuilder;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<CommitInfo> _commits = new();
    private readonly ObservableCollection<StackLayerPlan> _layers = new();

    /// <summary>①コミット読み込み時点の分割対象ブランチ名。Stack作成成功後のPRクローズに使う。</summary>
    private string? _loadedSourceBranch;

    private CliRunner? _cli;
    private GitService? _git;
    private GitHubService? _github;
    private StackOrchestrator? _orchestrator;

    public MainWindow()
    {
        InitializeComponent();
        CommitsGrid.ItemsSource = _commits;
        LayersList.ItemsSource = _layers;

        var repoRoot = FindGitRepoRoot(AppContext.BaseDirectory);
        if (repoRoot is not null)
            RepoPathBox.Text = repoRoot;

        Loaded += (_, _) => TryRefreshBranchList();
    }

    private static string? FindGitRepoRoot(string startDir)
    {
        var dir = new DirectoryInfo(startDir);
        for (var i = 0; i < 12 && dir is not null; i++, dir = dir.Parent)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
                return dir.FullName;
        }
        return null;
    }

    /// <summary>RepoPathBoxの値を元に各サービスを(必要なら)再生成して返す。</summary>
    private bool TryGetServices(out GitService git, out GitHubService github, out StackOrchestrator orchestrator)
    {
        var repoPath = RepoPathBox.Text.Trim();
        if (string.IsNullOrEmpty(repoPath) || !Directory.Exists(repoPath))
        {
            AppendLog("エラー: リポジトリパスが不正です。");
            git = null!;
            github = null!;
            orchestrator = null!;
            return false;
        }

        if (_cli is null)
        {
            _cli = new CliRunner();
            _cli.OutputReceived += line => Dispatcher.Invoke(() => AppendLog(line));
        }

        _git = new GitService(repoPath);
        _github = new GitHubService(_cli, repoPath);
        _orchestrator = new StackOrchestrator(_git, _cli, repoPath);

        git = _git;
        github = _github;
        orchestrator = _orchestrator;
        return true;
    }

    private void TryRefreshBranchList()
    {
        if (!TryGetServices(out var git, out _, out _)) return;
        try
        {
            var branches = git.GetAllBranchNames();
            SourceBranchCombo.ItemsSource = branches;
        }
        catch (Exception ex)
        {
            AppendLog($"ブランチ一覧の取得に失敗しました: {ex.Message}");
        }
    }

    private async void FetchButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out _, out _, out var orchestrator)) return;

        SetButtonsEnabled(false);
        try
        {
            var result = await orchestrator.FetchAsync();
            if (!result.Success)
            {
                AppendLog("=== Fetch 失敗 (ログ参照) ===");
                return;
            }
            AppendLog("=== Fetch 完了 ===");
            TryRefreshBranchList();
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }

    private void AppendLog(string line)
    {
        LogBox.AppendText(line + Environment.NewLine);
        LogBox.ScrollToEnd();
    }

    private async void LoadCommitsButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out var git, out _, out var orchestrator)) return;

        var baseBranch = BaseBranchBox.Text.Trim();
        var sourceBranch = (SourceBranchCombo.Text ?? "").Trim();

        if (string.IsNullOrEmpty(baseBranch) || string.IsNullOrEmpty(sourceBranch))
        {
            AppendLog("エラー: baseブランチと分割対象ブランチを入力してください。");
            return;
        }

        SetButtonsEnabled(false);
        try
        {
            // リモートの新しいブランチがローカルにまだ無いと「branchが見つかりません」に
            // なるため、読み込み前に必ず fetch しておく。
            var fetchResult = await orchestrator.FetchAsync();
            if (!fetchResult.Success)
                AppendLog("警告: fetchに失敗しました。ブランチが古いままの可能性があります。");
            else
                TryRefreshBranchList();

            var commits = git.GetCommitsAheadOfBase(baseBranch, sourceBranch);
            _commits.Clear();
            foreach (var c in commits)
                _commits.Add(c);
            _layers.Clear();

            AppendLog($"{commits.Count} 件のコミットを読み込みました ({baseBranch}..{sourceBranch})。");
            if (commits.Count == 0)
                AppendLog("コミットが0件です。base/対象ブランチの指定を確認してください。");

            _loadedSourceBranch = commits.Count > 0 ? sourceBranch : null;
        }
        catch (Exception ex)
        {
            AppendLog($"コミット読み込みに失敗しました: {ex.Message}");
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }

    private async void LoadPrListButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out _, out var github, out _)) return;

        try
        {
            var prs = await github.ListOpenPullRequestsAsync();
            if (prs.Count == 0)
            {
                AppendLog("オープン中のPRはありません。");
                return;
            }

            AppendLog("--- オープン中のPR一覧 ---");
            foreach (var pr in prs)
                AppendLog(pr.ToString());

            SourceBranchCombo.ItemsSource = prs.Select(p => p.HeadRefName).ToList();
        }
        catch (Exception ex)
        {
            AppendLog($"PR一覧の取得に失敗しました: {ex.Message}");
        }
    }

    private void MoveUpButton_Click(object sender, RoutedEventArgs e) => MoveSelected(-1);

    private void MoveDownButton_Click(object sender, RoutedEventArgs e) => MoveSelected(+1);

    private void MoveSelected(int delta)
    {
        if (CommitsGrid.SelectedItem is not CommitInfo selected) return;

        var index = _commits.IndexOf(selected);
        var newIndex = index + delta;
        if (index < 0 || newIndex < 0 || newIndex >= _commits.Count) return;

        _commits.Move(index, newIndex);
        CommitsGrid.SelectedItem = selected;
        CommitsGrid.ScrollIntoView(selected);
    }

    private async void ApplyReorderButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out var git, out _, out _)) return;

        if (_commits.Count == 0)
        {
            AppendLog("エラー: 先にコミットを読み込んでください。");
            return;
        }

        var baseBranch = BaseBranchBox.Text.Trim();
        if (string.IsNullOrEmpty(baseBranch))
        {
            AppendLog("エラー: baseブランチを入力してください。");
            return;
        }

        var confirm = MessageBox.Show(
            "作業ツリーを一時的にbaseへ切り替えて、表示中の順序でコミットを再構築(cherry-pick)します。\n" +
            "未コミットの変更が無いことを確認してください。よろしいですか?",
            "確認", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (confirm != MessageBoxResult.Yes) return;

        var currentOrder = _commits.ToList();
        SetButtonsEnabled(false);
        try
        {
            AppendLog("並び替えを確定します(cherry-pick再構築)...");
            var result = await Task.Run(() => git.ReorderAndRewrite(baseBranch, currentOrder));

            if (!result.Success)
            {
                AppendLog($"並び替えに失敗しました: {result.ErrorMessage}");
                return;
            }

            _commits.Clear();
            foreach (var c in result.NewCommits)
                _commits.Add(c);
            _layers.Clear();

            AppendLog($"並び替えを確定しました({result.NewCommits.Count}件、コミットSHAは再作成されています)。" +
                      "レイヤー番号を再確認してから②レイヤー計算を実行してください。");
        }
        catch (Exception ex)
        {
            AppendLog($"並び替えに失敗しました: {ex.Message}");
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }

    private void ComputeLayersButton_Click(object sender, RoutedEventArgs e)
    {
        _layers.Clear();

        if (_commits.Count == 0)
        {
            AppendLog("エラー: 先にコミットを読み込んでください。");
            return;
        }

        StackLayerPlan? current = null;
        int? lastGroup = null;

        foreach (var commit in _commits)
        {
            if (commit.GroupNumber < 1)
            {
                AppendLog($"エラー: コミット {commit.ShortSha} のレイヤー番号が不正です(1以上を指定)。");
                _layers.Clear();
                return;
            }

            if (lastGroup is not null && commit.GroupNumber < lastGroup)
            {
                AppendLog($"エラー: レイヤー番号は履歴順で増加方向のみ許可されます " +
                          $"(コミット {commit.ShortSha} で {lastGroup} → {commit.GroupNumber})。並び替えではなく区切りのみ指定してください。");
                _layers.Clear();
                return;
            }

            if (current is null || commit.GroupNumber != lastGroup)
            {
                current = new StackLayerPlan
                {
                    GroupNumber = commit.GroupNumber,
                    // このリポジトリのRuleset(DevelopBranchRule)は "stacked/**" のみ作成を許可しているため、
                    // それに合わせたプレフィックスにする。
                    BranchName = $"stacked/layer-{commit.GroupNumber}",
                };
                _layers.Add(current);
                lastGroup = commit.GroupNumber;
            }

            current.Commits.Add(commit);
        }

        AppendLog($"{_layers.Count} レイヤーに組み立てました。下のブランチ名を確認・編集してください。");
    }

    private async void BuildStackButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out _, out _, out var orchestrator)) return;

        if (_layers.Count == 0)
        {
            AppendLog("エラー: 先に②レイヤー計算を実行してください。");
            return;
        }

        var baseBranch = BaseBranchBox.Text.Trim();
        SetButtonsEnabled(false);
        try
        {
            var ok = await orchestrator.BuildStackAsync(baseBranch, _layers.ToList());
            AppendLog(ok ? "=== Stack作成 成功 ===" : "=== Stack作成 失敗 (ログ参照) ===");

            if (ok && CloseSourcePrCheckBox.IsChecked == true && !string.IsNullOrEmpty(_loadedSourceBranch))
            {
                var layerBranches = string.Join(", ", _layers.Select(l => l.BranchName));
                var comment = $"Stacked PR ({layerBranches}) に分割したためクローズします。";
                AppendLog($"分割元PR ({_loadedSourceBranch}) をクローズします...");
                var closeResult = await orchestrator.CloseSourcePullRequestAsync(_loadedSourceBranch, comment);
                AppendLog(closeResult.Success
                    ? "=== 分割元PR クローズ 成功 ==="
                    : "=== 分割元PR クローズ 失敗 (対応するPRが無い等。ログ参照) ===");
            }
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }

    private async void SyncButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out _, out _, out var orchestrator)) return;

        SetButtonsEnabled(false);
        try
        {
            var result = await orchestrator.SyncAsync();
            AppendLog(result.Success ? "=== Sync 成功 ===" : "=== Sync 失敗 (ログ参照) ===");
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }

    private async void MergeAllButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetServices(out _, out _, out var orchestrator)) return;

        var confirm = MessageBox.Show(
            "スタック内の全PRをGitHub上でマージします。よろしいですか?",
            "確認", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (confirm != MessageBoxResult.Yes) return;

        SetButtonsEnabled(false);
        try
        {
            var result = await orchestrator.MergeAllAsync();
            AppendLog(result.Success ? "=== Merge All 成功 ===" : "=== Merge All 失敗 (ログ参照) ===");
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }

    private void SetButtonsEnabled(bool enabled)
    {
        FetchButton.IsEnabled = enabled;
        LoadCommitsButton.IsEnabled = enabled;
        LoadPrListButton.IsEnabled = enabled;
        MoveUpButton.IsEnabled = enabled;
        MoveDownButton.IsEnabled = enabled;
        ApplyReorderButton.IsEnabled = enabled;
        ComputeLayersButton.IsEnabled = enabled;
        BuildStackButton.IsEnabled = enabled;
        SyncButton.IsEnabled = enabled;
        MergeAllButton.IsEnabled = enabled;
    }
}
