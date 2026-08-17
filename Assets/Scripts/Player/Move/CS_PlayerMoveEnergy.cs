using UnityEngine;

public class CS_PlayerMoveBoostEnergy : MonoBehaviour
{
    [Header("===== 参照 =====")]

    [Tooltip("プレイヤーのステータスSO")]
    [SerializeField]
    private CSO_PlayerMoveStatus _stats;

    /// <summary>
    /// 現在のエネルギー値
    /// </summary>
    public float CurrentEnergy { get; private set; }

    /// <summary>
    /// 最大エネルギー値
    /// </summary>
    public float MaxEnergy => _stats.maxEnergy;


    private void Awake()
    {
        CurrentEnergy = _stats.maxEnergy;

        CS_ValueObserver.Instance.Register(
            gameObject,
            this,
            "プレイヤー：現在のエネルギー値",
            () => CurrentEnergy
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
        if (CurrentEnergy < amount)
        {
            return false;
        }

        CurrentEnergy -= amount;

        return true;
    }

    /// <summary>
    /// エネルギーを強制的に消費する
    /// </summary>
    public void Consume(float amount)
    {
        CurrentEnergy = Mathf.Max(
            CurrentEnergy - amount,
            0f
        );
    }

    /// <summary>
    /// エネルギーを回復する
    /// </summary>
    [ContextMenu("エネルギー回復")]
    private void RecoverEnergy()
    {
        CurrentEnergy = _stats.maxEnergy;
    }

    /// <summary>
    /// ブースト可能か
    /// </summary>
    /// <returns>
    /// true: ブースト可能 false : ブースト不可
    /// </returns>
    public bool CanBoost()
    {
        return CurrentEnergy > 0f;
    }
}
