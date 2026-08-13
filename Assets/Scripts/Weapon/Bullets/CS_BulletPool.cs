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

    [Header("弾を生成する場所")][SerializeField] private Transform spawnPoint;

    private List<CS_BaseBullet> pool = new List<CS_BaseBullet>();

    public void Initialize(CS_BaseBullet bulletPrefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            CS_BaseBullet bullet = Instantiate(
                bulletPrefab,
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
        foreach (var bullet in pool)
        {
            if (!bullet.gameObject.activeSelf)
                return bullet;
        }
        return null;
    }
}
