using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]
public class CS_PlayerMoveSystem : MonoBehaviour
{
    [SerializeField]
    private SO_PlayerMoveStatus _stats;

    [SerializeField]
    private CS_PlayerMoveInput _input;

    [SerializeField]
    private CS_PlayerMoveGroundDetector _groundDetector;

    [SerializeField]
    private CS_PlayerMoveBooster _booster;

    [SerializeField]
    private Rigidbody _rb;

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

        CS_ValueObserver.Instance.Register(
            gameObject,
            this,
            "プレイヤー：現在の速度",
            () => _rb.linearVelocity
        );
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
        Vector3 moveDirection =
            CalculateMoveDirection(
                _input.MoveInput
            );

        Vector3 currentVelocity =
            _rb.linearVelocity;

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
            float currentAcceleration =
                _stats.acceleration;

            if (!_groundDetector.IsGrounded)
            {
                currentAcceleration *=
                    _stats.airControl;
            }

            Vector3 targetVelocity =
                moveDirection *
                _stats.moveSpeed;

            horizontalVelocity =
                Vector3.MoveTowards(
                    horizontalVelocity,
                    targetVelocity,
                    currentAcceleration *
                    Time.fixedDeltaTime
                );

            _rb.linearVelocity =
                new Vector3(
                    horizontalVelocity.x,
                    currentVelocity.y,
                    horizontalVelocity.z
                );
        }
        else
        {
            horizontalVelocity =
                Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    _stats.deceleration *
                    Time.fixedDeltaTime
                );

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
