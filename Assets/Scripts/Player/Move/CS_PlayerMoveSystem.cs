using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]
public class CS_PlayerMoveSystem : MonoBehaviour
{
    [Header("===== 参照 =====")]

    [Tooltip("プレイヤーのステータスSO")]
    [SerializeField]
    private CSO_PlayerMoveStatus _stats;

    [Tooltip("プレイヤーの入力情報")]
    [SerializeField]
    private CS_PlayerMoveInput _input;

    [Tooltip("プレイヤーの地面判定")]
    [SerializeField]
    private CS_PlayerMoveGroundDetector _groundDetector;

    [Tooltip("プレイヤーのブースト推進")]
    [SerializeField]
    private CS_PlayerMoveBooster _booster;

    [Tooltip("アップグレードチップマネージャーの参照")]
    private CS_UpgradeChipManager _upgradeChipManager;

    [Tooltip("プレイヤーのRigidbody")]
    [SerializeField]
    private Rigidbody _rb;

    [Tooltip("カメラ")]
    [SerializeField]
    private Camera targetCamera;


    private void Reset()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
        _rb.useGravity = false;

        _upgradeChipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();
    }

    private void FixedUpdate()
    {
        Move();
        ApplyAscend();
        Gravity();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // 入力値から移動方向を計算
        Vector3 moveDirection =
            CalculateMoveDirection(
                _input.MoveInput
            );

        // 現在の速度を取得
        Vector3 currentVelocity =
            _rb.linearVelocity;

        // 水平速度を取得
        Vector3 horizontalVelocity =
            new Vector3(
                currentVelocity.x,
                0f,
                currentVelocity.z
            );

        // ========================================
        // 通常移動
        // ========================================

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            // 現在の加速度を取得
            float currentAcceleration =
                _stats.acceleration;

            // 地面に接地していない場合は、空中制御を適用する
            if (!_groundDetector.IsGrounded)
            {
                currentAcceleration *=
                    _stats.airControl;
            }

            // 目標速度を計算
            Vector3 targetVelocity =
                moveDirection *
                _stats.moveSpeed * _upgradeChipManager.upgradeStatus.getupgradeStatus.playerMovementSpeedIncreaseRate;

            // 目標速度に向かって加速する
            horizontalVelocity =
                Vector3.MoveTowards(
                    horizontalVelocity,
                    targetVelocity,
                    currentAcceleration *
                    Time.fixedDeltaTime
                );

            // Rigidbodyの速度を更新
            _rb.linearVelocity =
                new Vector3(
                    horizontalVelocity.x,
                    currentVelocity.y,
                    horizontalVelocity.z
                );
        }
        // 入力がない場合は減速する
        else
        {
            // 減速する
            horizontalVelocity =
                Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    _stats.deceleration *
                    Time.fixedDeltaTime
                );

            // Rigidbodyの速度を更新
            _rb.linearVelocity =
                new Vector3(
                    horizontalVelocity.x,
                    currentVelocity.y,
                    horizontalVelocity.z
                );
        }

        // ========================================
        // ブースト推進
        // ========================================

        if (_booster.IsBoosting)
        {
            // ブースト方向に移動方向を加算する
            Vector3 boostDirection =
                moveDirection;

            boostDirection.Normalize();

            _rb.AddForce(
                boostDirection *
                _booster.CurrentBoostForce,
                ForceMode.Acceleration
            );
        }

        // -------------------------------
        // 上昇ブースト
        // -------------------------------

        if (_input.AscendInput)
        {
            _rb.AddForce(
                Vector3.up *
                _booster.CurrentBoostForce *
                _stats.boostAscendReduction,
                ForceMode.Acceleration
            );
        }
    }

    /// <summary>
    /// カメラ基準の移動方向を取得
    /// </summary>
    private Vector3 CalculateMoveDirection(Vector2 inputValue)
    {
        Vector3 forward =
            targetCamera.transform.forward;

        Vector3 right =
            targetCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction =
            forward * inputValue.y +
            right * inputValue.x;

        ///
        return Vector3.ClampMagnitude(
            direction,
            1f
        );
    }

    /// <summary>
    /// 上昇処理
    /// </summary>
    private void ApplyAscend()
    {
        if (!_input.AscendInput)
        {
            return;
        }

        _rb.AddForce(
            Vector3.up *
            _stats.ascendForce,
            ForceMode.Force
        );
    }

    /// <summary>
    /// 重力処理
    /// </summary>
    private void Gravity()
    {
        if (_groundDetector.IsGrounded)
        {
            return;
        }

        _rb.AddForce(
            Vector3.down *
            _stats.gravity,
            ForceMode.Acceleration
        );
    }
}
