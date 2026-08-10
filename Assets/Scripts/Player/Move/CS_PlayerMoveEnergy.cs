using UnityEngine;

public class CS_PlayerMoveEnergy : MonoBehaviour
{
    [SerializeField]
    private SO_PlayerMoveStatus _stats;

    public float CurrentEnergy { get; private set; }

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
    /// ENを消費する
    /// </summary>
    /// <returns>消費できた場合true</returns>
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
    /// ENを強制的に消費する
    /// </summary>
    public void Consume(float amount)
    {
        CurrentEnergy = Mathf.Max(
            CurrentEnergy - amount,
            0f
        );
    }

    /// <summary>
    /// ENを回復する
    /// </summary>
    [ContextMenu("エネルギー回復")]
    private void RecoverEnergy()
    {
        CurrentEnergy = _stats.maxEnergy;
    }

    /// <summary>
    /// ブースト可能か
    /// </summary>
    public bool CanBoost()
    {
        return CurrentEnergy > 0f;
    }
}
