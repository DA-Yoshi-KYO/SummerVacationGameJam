using StackPrBuilder.Models;

namespace StackPrBuilder.Services;

/// <summary>
/// 「ブランチ作成 → push → gh stack init → gh stack submit」の一連の流れをまとめる。
/// gh stack modify の対話TUIは使わない: 分割済みの複数ブランチを
/// `gh stack init &lt;branch1&gt; &lt;branch2&gt; ...` で一括・非対話登録できることを利用する。
/// </summary>
public class StackOrchestrator
{
    private readonly GitService _git;
    private readonly CliRunner _cli;
    private readonly string _repoPath;
    private readonly string _ghPath;
    private readonly string _gitPath;

    public event Action<string>? Log;

    public StackOrchestrator(GitService git, CliRunner cli, string repoPath, string ghPath = "gh", string gitPath = "git")
    {
        _git = git;
        _cli = cli;
        _repoPath = repoPath;
        _ghPath = ghPath;
        _gitPath = gitPath;
        _cli.OutputReceived += line => Log?.Invoke(line);
    }

    public async Task<bool> BuildStackAsync(string baseBranch, IReadOnlyList<StackLayerPlan> layers, string remote = "origin")
    {
        if (layers.Count == 0)
        {
            Log?.Invoke("エラー: レイヤーが1つもありません。");
            return false;
        }

        foreach (var layer in layers)
        {
            if (string.IsNullOrWhiteSpace(layer.BranchName))
            {
                Log?.Invoke("エラー: ブランチ名が空のレイヤーがあります。");
                return false;
            }
        }

        // 1. 各レイヤーの境目にローカルブランチを作成
        foreach (var layer in layers)
        {
            Log?.Invoke($"[1/4] branch作成: {layer.BranchName} @ {layer.Commits[^1].ShortSha}");
            try
            {
                _git.CreateBranchAt(layer.BranchName, layer.HeadCommitSha);
            }
            catch (Exception ex)
            {
                Log?.Invoke($"branch作成に失敗: {ex.Message}");
                return false;
            }
        }

        // 2. 各ブランチをpush (資格情報はGit Credential Managerに委譲)
        foreach (var layer in layers)
        {
            Log?.Invoke($"[2/4] push: {layer.BranchName}");
            var pushResult = await _cli.RunAsync(_gitPath, $"push -u {remote} {layer.BranchName}", _repoPath);
            if (!pushResult.Success)
            {
                Log?.Invoke($"push失敗: {layer.BranchName}\n{pushResult.StdErr}");
                return false;
            }
        }

        // 3. gh stack init で一括・非対話登録
        var branchArgs = string.Join(' ', layers.Select(l => l.BranchName));
        Log?.Invoke("[3/4] gh stack init 実行");
        var initResult = await _cli.RunAsync(_ghPath, $"stack init --base {baseBranch} {branchArgs}", _repoPath);
        if (!initResult.Success)
        {
            Log?.Invoke($"gh stack init 失敗:\n{initResult.StdErr}");
            return false;
        }

        // 4. gh stack submit でPRを一括作成
        Log?.Invoke("[4/4] gh stack submit 実行");
        var submitResult = await _cli.RunAsync(_ghPath, "stack submit --auto", _repoPath);
        if (!submitResult.Success)
        {
            Log?.Invoke($"gh stack submit 失敗:\n{submitResult.StdErr}");
            return false;
        }

        Log?.Invoke("完了: Stacked PRを作成しました。");
        return true;
    }

    /// <summary>
    /// リモートの最新参照を取り込む。ローカルのremote追跡ブランチが古いままだと
    /// 「対象branchが見つかりません」エラーになるため、コミット読み込み前に必ず呼ぶ。
    /// </summary>
    public Task<CliResult> FetchAsync(string remote = "origin") =>
        _cli.RunAsync(_gitPath, $"fetch {remote} --prune", _repoPath);

    /// <summary>下位レイヤーの変更を全レイヤーへ自動リベース連鎖させる。</summary>
    public Task<CliResult> SyncAsync() =>
        _cli.RunAsync(_ghPath, "stack sync --prune", _repoPath);

    /// <summary>スタック全体を一括マージする。</summary>
    public Task<CliResult> MergeAllAsync() =>
        _cli.RunAsync(_ghPath, "stack merge -y", _repoPath);

    /// <summary>
    /// 分割元ブランチに紐づくPRをコメント付きでクローズする。
    /// そのブランチにオープンなPRが無い場合は gh 側がエラーを返すので、
    /// 呼び出し側でログに出すだけに留め、全体の失敗にはしない。
    /// </summary>
    public Task<CliResult> CloseSourcePullRequestAsync(string sourceBranch, string comment) =>
        _cli.RunAsync(_ghPath, $"pr close \"{sourceBranch}\" --comment \"{comment}\"", _repoPath);
}
