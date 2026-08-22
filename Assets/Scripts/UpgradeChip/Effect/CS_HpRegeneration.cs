using UnityEngine;

public class CS_HpRegeneration : MonoBehaviour
{
    [Tooltip("HP自然回復量")]
    private float _hpRegenerationAmount = 1.0f;

    [Tooltip("前回の回復からの経過時間")]
    private float _timeSinceLastRegeneration = 0.0f;

    [Tooltip("回復間隔（秒数）")]
    private float _regenerationInterval = 1.0f;

    [Tooltip("回復対象: プレイヤー")]
    private CS_PlayerStatus _PlayerStatus;

    void Update()
    {
        // 経過時間を更新
        _timeSinceLastRegeneration += Time.deltaTime;

        // 回復間隔が経過した場合、HPを回復
        if (_timeSinceLastRegeneration < _regenerationInterval) return;

        // プレイヤーの回復
        if (_PlayerStatus != null)
            _PlayerStatus.Regenerate(_hpRegenerationAmount);


        _timeSinceLastRegeneration = 0.0f; // 経過時間をリセット
    }

    /// <summary>
    /// 回復対象を設定する
    /// </summary>
    /// <param name="playerStatus">プレイヤーのステータス</param>
    public void SetTarget(CS_PlayerStatus playerStatus)
    {
        _PlayerStatus = playerStatus;
    }

    /// <summary>
    /// 回復量を設定する
    /// </summary>
    /// <param name="amount">一度の回復量</param>
    public void SetRegenerationAmount(float amount)
    {
        _hpRegenerationAmount = amount;
    }

    /// <summary>
    /// 回復間隔を設定する
    /// </summary>
    /// <param name="interval">秒数</param>
    public void SetRegenerationInterval(float interval)
    {
        _regenerationInterval = interval;
    }
}
