using UnityEngine;

public class CS_MultipleLaunchMissileWeapon : CS_BaseWeapon
{
    [Header("発射を開始したかどうか")]
    private bool _isFiringStarted = false;

    public override void Start()
    {
        weaponName = "MultipleLaunchMissile";

        base.Start();
    }

    private void Update()
    {
        //弾を発射
        if (isReloading)
            base.Reloading();//リロード中
        else if (isShooting || _isFiringStarted)
            TryShot();  //射撃処理
    }

    protected override void Shot()
    {
        for (int i = 0; i < weaponData.bulletCount; i++)
        {
            //プールから弾を取得して発射
            CS_BaseBullet bullet = base.ActivateBullet();

            //ターゲットを設定
            if (bullet is CS_NormalMissileBullet m)
            {
                GameObject target = _weaponTarget.FindTarget();
                m.SetTarget(target ? target.transform : null);
            }
        }
    }

    protected override void TryShot()
    {
        //次に発射出来るまで待つ
        if (Time.time < nextFireTime)
            return;

        //弾がない場合はリロード
        if (currentBullets <= 0)
        {
            isReloading = true;
            _isFiringStarted = false;
            return;
        }

        //弾を発射
        for(int i = 0; i < weaponData.bulletCount; i++)
            Shot();

        currentBullets--;
        _isFiringStarted = true;

        //次に発射出来る時間を更新
        nextFireTime = Time.time + weaponData.fireRate;
    }
}
