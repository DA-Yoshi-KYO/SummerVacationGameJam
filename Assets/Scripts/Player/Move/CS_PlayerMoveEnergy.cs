using UnityEngine;

public class CS_PlayerMoveBoostEnergy : MonoBehaviour
{
    [Header("===== 参照 =====")]

    [Tooltip("プレイヤーのステータスSO")]
    [SerializeField]
    private CSO_PlayerMoveStatus _stats;

    [Tooltip("アップグレードチップマネージャーの参照")]
    [SerializeField]
    private CS_UpgradeChipManager _upgradeChipManager;

    /// <summary>
    /// 現在のエネルギー値
    /// </summary>
    private float _currentEnergy;
    public float currentEnergy => _currentEnergy;

    /// <summary>
    /// 最大エネルギー値
    /// </summary>
    public float maxEnergy => _stats.maxEnergy + _upgradeChipManager.upgradeStatus.getupgradeStatus.boostEnergyIncreaseAmount;


    private void Awake()
    {
        _currentEnergy = _stats.maxEnergy;

        _upgradeChipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();

        CS_ValueObserver.Instance.Register(
            gameObject,
            this,
            "プレイヤー：現在のエネルギー値",
            () => currentEnergy
        );
    }

    /// <summary>
    /// エネルギーを消費する
    /// </summary>
    /// <returns>
    /// true: 消費成功 false : 消費失敗
    /// </returns>
    public bool TryConsume(float amount)
    {
        amount *= _upgradeChipManager.upgradeStatus.getupgradeStatus.boostConsumptionReductionRate;

        if (_currentEnergy < amount)
        {
            return false;
        }

        _currentEnergy -= amount;

        return true;
    }

    /// <summary>
    /// エネルギーを強制的に消費する
    /// </summary>
    public void Consume(float amount)
    {
        _currentEnergy = Mathf.Max(
            _currentEnergy - amount,
            0f
        );
    }

    /// <summary>
    /// エネルギーを回復する
    /// </summary>
    /// <param name="amount">回復量</param>
    public void Regenerate(float amount)
    {
        _currentEnergy = Mathf.Min(
            _currentEnergy + amount,
            maxEnergy
        );
    }

    /// <summary>
    /// ブースト可能か
    /// </summary>
    /// <returns>
    /// true: ブースト可能 false : ブースト不可
    /// </returns>
    public bool CanBoost()
    {
        return _currentEnergy > 0f;
    }
}
