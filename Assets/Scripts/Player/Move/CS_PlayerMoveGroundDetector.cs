using UnityEngine;

public class CS_PlayerMoveGroundDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField]
    private float _checkDistance = 0.2f;

    [SerializeField]
    private float _checkRadius = 0.3f;

    public bool IsGrounded { get; private set; }

    public Vector3 GroundNormal { get; private set; }

    private void FixedUpdate()
    {
        CheckGround();
    }

    /// <summary>
    /// ’n–Ê”»’è
    /// </summary>
    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;

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
