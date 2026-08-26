using UnityEngine;

public class CS_SniperRifleWeapon : CS_BaseWeapon
{
    public override void Start()
    {
        weaponName = "SniperRifle";
        base.Start();
    }

    protected override void Shot()
    {
        //プールから弾を取得して発射
        CS_BaseBullet bullet = base.ActivateBullet();

        //標的を設定
        GameObject targetEnemy = _weaponTarget.FindTarget();

        if (targetEnemy != null)
        {
            // 敵の方向ヘのベクトルを計算
            Vector3 directionToEnemy = (targetEnemy.transform.position - transform.position).normalized;

            bullet.Activate(firePoint.position, Quaternion.LookRotation(directionToEnemy));
        }
        else
        {
            // 敵が見つからない場合は、通常の発射方向で弾を発射
            bullet.Activate(firePoint.position, firePoint.rotation);
        }

        // 弾の種類に応じた設定
        if (bullet is CS_SimpleBullet)
        {
            CS_SimpleBullet simpleBullet = bullet as CS_SimpleBullet;
            simpleBullet.SetRange(weaponData.range);
        }
    }
}
