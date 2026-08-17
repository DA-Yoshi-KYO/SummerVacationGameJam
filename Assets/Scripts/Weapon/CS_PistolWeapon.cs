using UnityEngine;

public class CS_PistolWeapon : CS_BaseWeapon
{
    [SerializeField]
    [Header("照準UI")]
    private CS_AimUI _aimUI;

    [SerializeField]
    [Header("照準の重み")]
    private float _angleWeight;

    protected override void Shot()
    {
        //プールから弾を取得して発射
        CS_BaseBullet bullet = base.ActivateBullet();

        //ターゲットを設定
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

    //照準方向の一番近い敵を狙う処理
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

        return lockonGameObject;
    }

}
