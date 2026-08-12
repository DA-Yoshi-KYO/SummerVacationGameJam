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

public class CS_BaseWeapon : MonoBehaviour
{
    [Header("武器データベース")][SerializeField] protected CSO_WeaponDataBase weaponDataBase;
    [Header("武器のインデックス")][SerializeField] protected int weaponIndex;

    protected WeaponList weaponData;//武器のデータ

    protected int currentBullets;//現在の弾数
    protected bool reloading;//リロード中かどうか
    protected float nextFireTime;//次に発射できる時間

    public virtual void Start()
    {
        weaponData = weaponDataBase.weapons[weaponIndex];

        currentBullets = weaponData.bulletCount;
    }

    protected virtual void Update()
    {
        //リロード中は発射しない
        if (reloading)
            return;

        //弾を発射
        //InputActionにするなら処理変更が必要
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryShoot();
        }
    }

    //弾を発射可能か判定して、可能なら発射する
    public virtual void TryShoot()
    {
        //次に発射出来るまで待つ
        if (Time.time < nextFireTime) return;

        //弾がない場合はリロード
        if (currentBullets <= 0.0f)
        {
            StartCoroutine(Reload());
            return;
        }

        //弾を発射
        StartCoroutine(FireBurst());

        //次に発射出来る時間を更新
        nextFireTime = Time.time + (1.0f / weaponData.fireRate);
    }

    //連射処理
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

    //弾を発射する処理
    protected virtual void Shoot()
    {
    }

    //リロード処理
    protected IEnumerator Reload()
    {
        reloading = true;

        //リロード時間待つ
        yield return new WaitForSeconds(weaponData.reloadTime);

        currentBullets = weaponData.bulletCount;
        reloading = false;
    }

}
