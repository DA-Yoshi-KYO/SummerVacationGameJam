using UnityEngine;

public class CS_MultipleLaunchMissileWeapon : CS_BaseWeapon
{
    [Header("照準UI")]
    [SerializeField] 
    protected CS_AimUI _aimUI;

    [Header("照準の重み")]
    [SerializeField] 
    protected float _angleWeight;

    [Header("発射を開始したかどうか")]
    private bool _isFiringStarted = false;

    public override void Start()
    {
        weaponName = "MultipleLaunchMissile";

        base.Start();

        _aimUI = GameObject.FindAnyObjectByType<CS_AimUI>();
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
                GameObject target = FindTargetWithAim();
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
        Shot();
        currentBullets--;
        _isFiringStarted = true;

        //次に発射出来る時間を更新
        nextFireTime = Time.time + weaponData.fireRate;
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

            float score = dist + angle * _angleWeight;

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

            _aimUI.SetLocked(angle < 10f);
        }
        else
        {
            _aimUI.SetLocked(false);
        }

        return lockonGameObject;
    }
}
