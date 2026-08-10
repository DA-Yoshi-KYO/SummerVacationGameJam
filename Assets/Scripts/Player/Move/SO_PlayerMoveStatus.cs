using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_PlayerMoveStatus",
    menuName = "ScriptableObjects/Player/MoveStatus"
)]
public class SO_PlayerMoveStatus : ScriptableObject
{
    [Header("===== 移動 =====")]

    [Tooltip("通常移動速度")]
    public float moveSpeed = 8f;

    [Tooltip("通常移動時の加速度")]
    public float acceleration = 20f;

    [Tooltip("通常移動時の減速度")]
    public float deceleration = 15f;

    [Header("===== 旋回 =====")]

    public float groundTurnSpeed = 720f;

    public float airTurnSpeed = 360f;

    [Header("===== ジャンプ =====")]

    public float jumpPower = 10f;

    [Range(0f, 1f)]
    public float airControl = 0.6f;

    [Header("===== ブースト =====")]

    [Tooltip("ブーストを押した瞬間の最大推進力")]
    public float boostInitialForce = 100f;

    [Tooltip("初動推進力が減衰する時間")]
    public float boostInitialDuration = 0.15f;

    [Tooltip("初動推進力の減衰カーブ")]
    public AnimationCurve boostInitialCurve =
        AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    [Tooltip("初動が終わった後の継続推進力")]
    public float boostContinuousForce = 15f;

    [Tooltip("ブースト中のEN消費量 / 秒")]
    public float boostEnergyConsumption = 10f;


    [Header("===== エネルギー =====")]

    public float maxEnergy = 100f;

    public float energyRecovery = 20f;

    public float energyRecoveryDelay = 0.5f;
}
