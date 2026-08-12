/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　ミサイルの武器クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;

public class CS_MissileWeapon : CS_BaseWeapon
{
    [Header("照準UI")][SerializeField] private CS_AimUI aimUI;
    [Header("発射する位置")][SerializeField] private Transform firePoint;

    public override void Start()
    {
        base.Start();

        weaponData = weaponDataBase.weapons[weaponIndex];

        currentBullets = weaponData.bulletCount;
    }

    protected override void Shoot()
    {
        GameObject missile = Instantiate(weaponData.bulletPrefab, firePoint.position, firePoint.rotation);
        CS_MissileBullet m = missile.GetComponent<CS_MissileBullet>();

        //データの設定
        m.SetDamage(weaponData.damage);
        m.SetSpeed(weaponData.bulletSpeed);
        m.SetOwner(gameObject);

        //一番近い敵を探す
        //GameObject target = m.FindNearestEnemy();
        GameObject target = m.FindTargetWithAim(transform, aimUI);

        if (target != null)
        {
            m.SetTarget(target.transform);
        }
        else
        {
            //敵がいない場合は、ダミーのターゲットを設定する
            Transform dummy = new GameObject().transform;
            dummy.position = transform.position + transform.forward * weaponData.distanceForward;
            m.SetTarget(dummy);
            m.SetDummyTarget();
        }
    }

}
