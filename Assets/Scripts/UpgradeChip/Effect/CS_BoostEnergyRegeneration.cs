using UnityEngine;

public class CS_BoostEnergyRegeneration : MonoBehaviour
{
    [Tooltip("エネルギー自然回復量")]
    private float _energyRegenerationAmount = 1.0f;

    [Tooltip("前回の回復からの経過時間")]
    private float _timeSinceLastRegeneration = 0.0f;

    [Tooltip("回復間隔（秒数）")]
    private float _regenerationInterval = 1.0f;

    [Tooltip("回復対象: プレイヤーのブーストエネルギー")]
    private CS_PlayerMoveBoostEnergy _PlayerBoostEnergy;

    void Update()
    {
        // 経過時間を更新
        _timeSinceLastRegeneration += Time.deltaTime;

        // 回復間隔が経過した場合、エネルギーを回復
        if (_timeSinceLastRegeneration < _regenerationInterval) return;

        // プレイヤーのエネルギー回復
        if (_PlayerBoostEnergy != null)
            _PlayerBoostEnergy.Regenerate(_energyRegenerationAmount);

        _timeSinceLastRegeneration = 0.0f; // 経過時間をリセット
    }

    /// <summary>
    /// 回復量を設定する
    /// </summary>
    /// <param name="amount">一度の回復量</param>
    public void SetRegenerationAmount(float amount)
    {
        _energyRegenerationAmount = amount;
    }

    /// <summary>
    /// 回復間隔を設定する
    /// </summary>
    /// <param name="interval">秒数</param>
    public void SetRegenerationInterval(float interval)
    {
        _regenerationInterval = interval;
    }

    /// <summary>
    /// 回復対象のプレイヤーのブーストエネルギーを設定する
    /// </summary>
    /// <param name="playerBoostEnergy">プレイヤーのブーストエネルギー</param>
    public void SetTarget(CS_PlayerMoveBoostEnergy playerBoostEnergy)
    {
        _PlayerBoostEnergy = playerBoostEnergy;
    }
}
