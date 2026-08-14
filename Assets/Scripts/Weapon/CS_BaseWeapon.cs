/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　武器の基底クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;

public abstract class CS_BaseWeapon : MonoBehaviour
{
    [Header("発射する位置")][SerializeField] private Transform firePoint;

    [Header("武器データベース")][SerializeField] protected CSO_WeaponDataBase weaponDataBase;
    [Header("武器名（Dictionaryで検索）")][SerializeField] protected string weaponName;
    [Header("弾プール")][SerializeField] protected CS_BulletPool bulletPool;

    protected CSO_WeaponDataBase.WeaponDataBase weaponData;

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

        //武器名から武器データを取得
        weaponData = weaponDataBase.weaponDatas[weaponName];

        if (weaponData == null)
        {
            Debug.LogError(weaponName + " がデータベースに存在しません");
            return;
        }
        
        //現在の弾数を設定
        currentBullets = weaponData.bulletCount;

        //弾プールを初期化
        bulletPool.Initialize(weaponData.bulletPrefab.GetComponent<CS_BaseBullet>());
    }

    private void Update()
    {
        //弾を発射
        if (isReloading)
            Reloading();//リロード中
        else
            TryShot();  //射撃処理
    }

    //弾を発射可能か判定して、可能なら発射する
    protected virtual void TryShot()
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
        Shot();
        currentBullets--;

        //次に発射出来る時間を更新
        nextFireTime = Time.time + weaponData.fireRate;
    }

    //弾を発射する処理
    protected abstract void Shot();

    //弾をプールから取得して有効化する処理
    //射撃時はこのメソッドを呼び出す
    protected CS_BaseBullet ActivateBullet()
    {
        CS_BaseBullet bullet = bulletPool.GetBullet();

        //位置と向きをセットして有効化
        bullet.Activate(firePoint.position, firePoint.rotation);

        bullet.SetDamage(weaponData.damage);
        bullet.SetSpeed(weaponData.bulletSpeed);
        bullet.SetOwner(gameObject);

        return bullet;
    }

    //リロード処理
    private void Reloading()
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
