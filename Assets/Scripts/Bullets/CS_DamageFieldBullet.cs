using UnityEngine;

public class CS_DamageFieldBullet : CS_BaseBullet
{
    [Tooltip("追従するゲームオブジェクト")]
    private GameObject _targetObject;

    private Collider _damageFieldCollider;

    private void Awake()
    {
        _damageFieldCollider = GetComponent<Collider>();
        if (_damageFieldCollider == null)
        {
            Debug.LogError("DamageFieldBulletにはColliderが必要です。");
        }
    }

    private void Update()
    {
        if (_targetObject == null) return;

        // ターゲットの位置に追従する
        transform.position = _targetObject.transform.position;
    }

    public void SetTarget(GameObject target)
    {
        _targetObject = target;
    }

    public void SetDamageFieldRange(float range)
    {
        if (_damageFieldCollider is SphereCollider sphereCollider)
        {
            sphereCollider.radius = range;
        }
        else
        {
            Debug.LogError("DamageFieldBulletのColliderはSphereColliderである必要があります。");
        }
    }

    public void OnDrawGizmos()
    {
        if (_damageFieldCollider is SphereCollider sphereCollider)
        {
            Gizmos.color = new Color(0, 0.627f, 0.914f, 0.5f);
            Gizmos.DrawSphere(transform.position, sphereCollider.radius);
        }
    }
}
