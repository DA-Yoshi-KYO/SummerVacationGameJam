/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　武器の基底クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;
using static CSO_WeaponDataBase;
using System.Collections;

public abstract class CS_BaseWeapon : MonoBehaviour
{
    [Header("武器データベース")][SerializeField] protected CSO_WeaponDataBase weaponDataBase;

    [Header("武器名（Dictionaryで検索）")][SerializeField] protected string weaponName;

    [Header("弾プール")][SerializeField] protected CS_BulletPool bulletPool;

    protected WeaponDataBase weaponData;

    protected int currentBullets;//現在の弾数
    protected bool isReloading;//リロード中かどうか
    protected float nextFireTime;//次に発射できる時間

    protected float reloadTime = 0.0f;//リロード中のタイマー

    public virtual void Start()
    {
        if (weaponDataBase == null)
        {
            Debug.LogError("WeaponDataBase が設定されていません");
            return;
        }

        if (!weaponDataBase.weaponDatas.TryGetValue(weaponName, out weaponData))
        {
            Debug.LogError(weaponName + " がデータベースに存在しません");
            return;
        }

        currentBullets = weaponData.bulletCount;
    }

    protected virtual void Update()
    {
        //弾を発射
        if (isReloading)
            Reloading();
        else
            TryShoot();
    }

    //弾を発射可能か判定して、可能なら発射する
    protected virtual void TryShoot()
    {
        //次に発射出来るまで待つ
        if (Time.time < nextFireTime)
            return;

        //弾がない場合はリロード
        if (currentBullets <= 0)
        {
            isReloading = true;
            return;
        }

        //弾を発射
        Shoot();
        currentBullets--;

        //次に発射出来る時間を更新
        nextFireTime = Time.time + (1.0f / weaponData.fireRate);
    }

    //弾を発射する処理
    protected abstract void Shoot();

    //連射処理
    //一回の射撃で複数の弾を発射する場合に使用
    protected virtual IEnumerator FireBurst()
    {
        //現在の撃てる弾数と武器の最大連射数を比較
        int shots = Mathf.Min(currentBullets, weaponData.bulletCount);

        for (int i = 0; i < shots; i++)
        {
            Shoot();

            currentBullets--;

            //連射間隔
            yield return new WaitForSeconds(1.0f / weaponData.fireRate);
        }
    }

    //リロード処理
    protected virtual void Reloading()
    {
        reloadTime += Time.deltaTime;

        if (reloadTime >= weaponData.reloadTime)
        {
            currentBullets = weaponData.bulletCount;
            isReloading = false;
            reloadTime = 0.0f;
        }
    }
}
