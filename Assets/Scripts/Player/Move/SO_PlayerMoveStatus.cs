using UnityEngine;

[CreateAssetMenu(
    fileName = "DB_PlayerMoveStatus",
    menuName = "ScriptableObjects/Player/MoveStatus"
)]
public class CSO_PlayerMoveStatus : ScriptableObject
{
    [Header("===== 基本設定 =====")]

    [Tooltip("重力")]
    public float gravity = 9.81f;

    [Header("===== 移動 =====")]

    [Tooltip("通常移動速度")]
    public float moveSpeed = 8f;

    [Tooltip("通常移動時の加速度")]
    public float acceleration = 20f;

    [Tooltip("通常移動時の減速度")]
    public float deceleration = 15f;

    [Header("===== 上昇 =====")]

    [Tooltip("上昇時の推進力")]
    public float ascendForce = 20f;

    [Range(0f, 1f)]
    public float airControl = 0.6f;

    [Header("===== ブースト =====")]

    [Tooltip("ブーストを押した瞬間の最大推進力")]
    public float boostInitialForce = 100f;

    [Tooltip("上昇方向のブースト軽減係数")]
    public float boostAscendReduction = 0.5f;

    [Tooltip("初動推進力が減衰する時間")]
    public float boostInitialDuration = 0.15f;

    [Tooltip("初動推進力の減衰カーブ")]
    public AnimationCurve boostInitialCurve =
        AnimationCurve.EaseInOut(0f, 1f, 1f, 0.2f);

    [Tooltip("初動が終わった後の継続推進力")]
    public float boostContinuousForce = 15f;

    [Tooltip("ブースト中のEN消費量 / 秒")]
    public float boostEnergyConsumption = 10f;

    [Tooltip("ブーストを離した後、再度使用できるまでのクールタイム")]
    public float boostCooldown = 0.5f;


    [Header("===== エネルギー =====")]

    [Tooltip("最大エネルギー値")]
    public float maxEnergy = 100f;
}
