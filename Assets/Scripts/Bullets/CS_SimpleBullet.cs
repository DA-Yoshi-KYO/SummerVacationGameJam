using UnityEngine;

public class CS_SimpleBullet : CS_BaseBullet
{
    [Tooltip("弾が発射された場所")]
    private Vector3 _firePoint;

    [Tooltip("射程距離(弾が消える距離)")]
    private float _range;

    private void Update()
    {
        if (!isActive)
            return;

        BulletMovement();

        // 射程距離を超えたら非アクティブ化
        if (Vector3.Distance(_firePoint, transform.position) > _range)
        {
            Deactivate();
        }
    }

    public override void Activate(Vector3 pos, Quaternion rot)
    {
        _firePoint = pos;

        base.Activate(pos, rot);
    }

    public void SetRange(float range)
    {
        _range = range;
    }
}
