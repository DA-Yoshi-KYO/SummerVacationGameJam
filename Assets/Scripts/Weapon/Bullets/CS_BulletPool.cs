/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　弾丸をプールするクラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-14 | 初回作成
 */
using System.Collections.Generic;
using UnityEngine;

public class CS_BulletPool : MonoBehaviour
{
    [Header("初期の生成する弾の数")][SerializeField] private int poolSize = 5;

    [Header("プールの親オブジェクト")][SerializeField] private Transform spawnPoint;

    private List<CS_BaseBullet> pool = new List<CS_BaseBullet>();

    private CS_BaseBullet bulletPrefabData;

    public void Initialize(CS_BaseBullet bulletPrefab)
    {
        bulletPrefabData = bulletPrefab;
        for (int i = 0; i < poolSize; i++)
        {
            CS_BaseBullet bullet = Instantiate(
                bulletPrefabData,
                spawnPoint != null ? spawnPoint.position : Vector3.zero,
                Quaternion.identity
            );

            if (spawnPoint != null)
            {
                bullet.transform.SetParent(spawnPoint);
            }

            bullet.gameObject.SetActive(false);
            pool.Add(bullet);
        }
    }

    public CS_BaseBullet GetBullet()
    {
        foreach (var bullets in pool)
        {
            if (!bullets.gameObject.activeSelf)
                return bullets;
        }

        //プールに空きがない場合は新しい弾を生成して返す
        CS_BaseBullet bullet = Instantiate(
                bulletPrefabData,
                spawnPoint != null ? spawnPoint.position : Vector3.zero,
                Quaternion.identity
            );

        if (spawnPoint != null)
        {
            bullet.transform.SetParent(spawnPoint);
        }

        bullet.gameObject.SetActive(false);
        pool.Add(bullet);

        return bullet;
    }
}
