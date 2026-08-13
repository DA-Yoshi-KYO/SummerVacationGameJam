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
    [Header("照準UI")][SerializeField] protected CS_AimUI aimUI;
    [Header("照準の重み")][SerializeField] protected float angleWeight;
    [Header("発射する位置")][SerializeField] private Transform firePoint;


    public override void Start()
    {
        base.Start();

        bulletPool.Initialize(weaponData.bulletPrefab.GetComponent<CS_BaseBullet>());
    }

    //確認用でUIButtonから呼び出す用の関数のためのちに削除
    public void FireByUI()
    {
        TryShoot();
    }

    protected override void Shoot()
    {
        CS_BaseBullet bullet = bulletPool.GetBullet();

        //位置と向きをセットして有効化
        bullet.Activate(firePoint.position, firePoint.rotation);

        bullet.SetDamage(weaponData.damage);
        bullet.SetSpeed(weaponData.bulletSpeed);
        bullet.SetOwner(gameObject);

        //ミサイル弾ならターゲットを設定
        if (bullet is CS_MissileBullet m)
        {
            GameObject target = FindTargetWithAim();
            m.SetTarget(target ? target.transform : null);
        }
    }


    //一番近い敵を探す処理
    public GameObject FindNearestEnemy()
    {
        //"Enemy"タグが付いたオブジェクトを全部取得
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        //全敵の中から最も距離が近いものを探す
        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }

        return nearest;
    }

    //照準方向の一番近い敵をロックオンする処理
    public GameObject FindTargetWithAim()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject lockonGameObject = null;
        float lockonScore = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToEnemy);

            float score = dist + angle * angleWeight;

            if (score < lockonScore)
            {
                lockonScore = score;
                lockonGameObject = enemy;
            }
        }

        //ロックオン判定
        if (lockonGameObject != null)
        {
            Vector3 dirToBest = (lockonGameObject.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToBest);

            aimUI.SetLocked(angle < 10f);
        }
        else
        {
            aimUI.SetLocked(false);
        }

        return lockonGameObject;
    }
}
