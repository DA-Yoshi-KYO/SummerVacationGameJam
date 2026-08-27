/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　武器の基底クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CS_BaseWeapon : MonoBehaviour
{
    [Header("発射する位置")][SerializeField] protected Transform firePoint;

    [Header("武器データベース")][SerializeField] protected CSO_WeaponDataBase weaponDataBase;
    [Header("武器名（Dictionaryで検索）")] protected string weaponName;
    [Header("弾プール")][SerializeField] protected CS_BulletPool bulletPool;

    protected CSO_WeaponDataBase.WeaponDataBase weaponData;

    // 引数に弾のゲームオブジェクトを渡して追加コンポーネントを設定するリスト
    private Dictionary<Type, Action<GameObject>> bulletComponentSetters = new Dictionary<Type, Action<GameObject>>();

    private CS_UpgradeChipManager _upgradeChipManager;

    protected int currentBullets;//現在の弾数
    protected bool isReloading;//リロード中かどうか
    protected bool isShooting;//射撃中かどうか
    protected float nextFireTime;//次に発射できる時間

    [SerializeField]
    protected int _multipleShotCount = 1;//同時発射数
    public int multipleShotCount
    {
        get { return _multipleShotCount; }
        set { _multipleShotCount = value; }
    }

    protected float reloadTime = 0.0f;//リロード中のタイマー

    public virtual void Start()
    {
        if (weaponDataBase == null)
        {
            Debug.LogError("WeaponDataBase が設定されていません");
            return;
        }

        //武器名から武器データを取得
        weaponData = weaponDataBase.weaponDatas[weaponName].CloneData();

        if (weaponData == null)
        {
            Debug.LogError(weaponName + " がデータベースに存在しません");
            return;
        }

        _upgradeChipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();

        _multipleShotCount = 1;

        //現在の弾数を設定
        currentBullets = weaponData.bulletCount;

        bulletPool = GetComponent<CS_BulletPool>();

        //弾プールを初期化
        bulletPool.Initialize(weaponData.bulletPrefab.GetComponent<CS_BaseBullet>());
    }

    private void Update()
    {
        //弾を発射
        if (isReloading)
            Reloading();//リロード中
        else if (isShooting)
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
        for (int i = 0; i < weaponData.bulletCount; i++)
            Shot();

        currentBullets--;

        //次に発射出来る時間を更新
        nextFireTime = Time.time + weaponData.fireRate * _upgradeChipManager.upgradeStatus.getupgradeStatus.fireRateIncreaseRate;
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

        // 弾のゲームオブジェクトに追加するコンポーネントを設定する
        foreach (var key in bulletComponentSetters.Keys)
        {
            // すでに追加されていない場合のみ追加する
            if (bullet.gameObject.GetComponent(key) == null)
            {
                bulletComponentSetters[key](bullet.gameObject);
            }
        }

        return bullet;
    }

    //リロード処理
    protected void Reloading()
    {
        reloadTime += Time.deltaTime;

        if (reloadTime >= weaponData.reloadTime * _upgradeChipManager.upgradeStatus.getupgradeStatus.reloadSpeedIncreaseRate)
        {
            currentBullets = (int)(weaponData.bulletCount * _upgradeChipManager.upgradeStatus.getupgradeStatus.bulletCountIncreaseRate);
            isReloading = false;
            reloadTime = 0.0f;
        }
    }

    // 射撃中かどうかを設定する
    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    // 弾のゲームオブジェクトに追加するコンポーネントを登録するメソッド
    public void RegistAddBulletComponent<T>() where T : Component
    {
        Type bulletComponentType = typeof(T);

        // すでに登録されている場合は何もしない
        if (bulletComponentSetters.ContainsKey(bulletComponentType)) return;

        // 弾のゲームオブジェクトに追加するコンポーネントを設定するActionを作成
        Action<GameObject> setter = (bullet) =>
        {
            bullet.AddComponent<T>();
        };

        // リストに追加
        bulletComponentSetters.Add(bulletComponentType, setter);
    }
}
