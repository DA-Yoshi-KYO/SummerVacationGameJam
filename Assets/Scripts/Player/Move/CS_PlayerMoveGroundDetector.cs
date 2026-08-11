using UnityEngine;

public class CS_PlayerMoveGroundDetector : MonoBehaviour
{
    [Tooltip("地面判定を行うレイヤー")]
    [SerializeField]
    private LayerMask _groundLayer;

    [Tooltip("地面判定の距離")]
    [SerializeField]
    private float _checkDistance = 0.2f;

    [Tooltip("地面判定の半径")]
    [SerializeField]
    private float _checkRadius = 0.3f;

    /// <summary>
    /// 地面に接地しているかどうか
    /// </summary>
    public bool IsGrounded { get; private set; }

    /// <summary>
    /// 地面の法線ベクトル
    /// </summary>
    public Vector3 GroundNormal { get; private set; }


    private void FixedUpdate()
    {
        CheckGround();
    }

    /// <summary>
    /// 地面判定
    /// </summary>
    private void CheckGround()
    {
        // 少し上にオフセットして判定することで、地面の凹凸による誤判定を防ぐ
        Vector3 origin = transform.position + Vector3.up * 0.05f;

        // 地面判定を行う
        if (Physics.SphereCast(
            origin,
            _checkRadius,
            Vector3.down,
            out RaycastHit hit,
            _checkDistance,
            _groundLayer,
            QueryTriggerInteraction.Ignore))
        {
            IsGrounded = true;
            GroundNormal = hit.normal;
        }
        else
        {
            IsGrounded = false;
            GroundNormal = Vector3.up;
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// Gizmosで地面判定の範囲を表示する
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position + Vector3.up * 0.05f;

        Gizmos.DrawWireSphere(
            origin + Vector3.down * _checkDistance,
            _checkRadius
        );
    }

#endif
}
