using System.Collections.Generic;
using UnityEngine;

public class CS_ShotgunWeapon : CS_BaseWeapon
{
    [SerializeField]
    [Header("ペレット数")]
    private int _bulletCount;

    [SerializeField]
    [Header("拡散率(小さいほど直進しやすい)")]
    private float _spreadRate;

    public override void Start()
    {
        weaponName = "Shotgun";

        base.Start();
    }

    protected override void Shot()
    {
        // プールから弾を取得して発射
        List<CS_BaseBullet> bulletList = new List<CS_BaseBullet>();

        for (int i = 0; i < _bulletCount; i++)
        {
            bulletList.Add(base.ActivateBullet());
        }


        // ターゲットを取得
        GameObject targetEnemy = _weaponTarget.FindTarget();

        Vector3 baseDirection = transform.forward;

        if (targetEnemy != null)
        {
            // ターゲットが存在する場合は、ターゲットの方向を基準にする
            baseDirection = (targetEnemy.transform.position - transform.position).normalized;
        }

        // 弾を発射
        foreach (var bullet in bulletList)
        {
            // 弾の初期位置を設定
            bullet.transform.position = baseDirection;

            float randomAngleX = Random.Range(-_spreadRate, _spreadRate);
            float randomAngleY = Random.Range(-_spreadRate, _spreadRate);

            Quaternion spreadRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0);

            // 弾の方向を拡散させる
            Vector3 spreadDirection = spreadRotation * baseDirection;
            bullet.Activate(firePoint.position, Quaternion.LookRotation(spreadDirection));

            // 弾の射程を設定
            if (bullet is CS_SimpleBullet)
            {
                CS_SimpleBullet simpleBullet = bullet as CS_SimpleBullet;
                simpleBullet.SetRange(weaponData.range);
            }
        }
    }
}
