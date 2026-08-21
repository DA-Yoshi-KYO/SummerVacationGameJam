namespace StackPrBuilder.Models;

/// <summary>
/// スタックの1レイヤー(=1本のブランチ=1つのPRになる予定のもの)。
/// ComputeLayers() でコミットのGroupNumberから機械的に組み立てられる。
/// </summary>
public class StackLayerPlan
{
    public int GroupNumber { get; set; }

    /// <summary>作成するブランチ名。デフォルト値をUI上で編集可能にする。</summary>
    public string BranchName { get; set; } = "";

    /// <summary>このレイヤーに属するコミット(履歴順)。</summary>
    public List<CommitInfo> Commits { get; } = new();

    /// <summary>このレイヤーの先頭に立てるブランチが指す先、つまり一番新しいコミットのSHA。</summary>
    public string HeadCommitSha => Commits.Count > 0 ? Commits[^1].Sha : "";

    public string Summary =>
        Commits.Count == 0
            ? "(コミットなし)"
            : $"{Commits.Count}コミット ({Commits[0].ShortSha}..{Commits[^1].ShortSha})";
}
