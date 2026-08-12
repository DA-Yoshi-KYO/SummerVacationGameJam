/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　弾丸の基底クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;

public class CS_BaseBullet : MonoBehaviour
{
    [Header("照準の重み")][SerializeField] protected float angleWeight;
    protected float damage;
    protected float speed;
    protected GameObject owner;//弾を撃ったオブジェクト

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
    public GameObject FindTargetWithAim(Transform shooter, CS_AimUI aimUI)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject lockonGameObject = null;
        float lockonScore = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(shooter.position, enemy.transform.position);

            Vector3 dirToEnemy = (enemy.transform.position - shooter.position).normalized;
            float angle = Vector3.Angle(shooter.forward, dirToEnemy);

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
            Vector3 dirToBest = (lockonGameObject.transform.position - shooter.position).normalized;
            float angle = Vector3.Angle(shooter.forward, dirToBest);

            aimUI.SetLocked(angle < 10f);
        }
        else
        {
            aimUI.SetLocked(false);
        }

        return lockonGameObject;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner)
            return;

        //ダメージを与える処理
        Debug.Log("Hit " + collision.gameObject.name + " Damage: " + damage);

        Destroy(gameObject);
    }

    //Setter関数

    //ダメージの設定
    public void SetDamage(float Damage)
    {
        damage = Damage;
    }

    //スピードの設定
    public void SetSpeed(float Speed)
    {
        speed = Speed;
    }

    //弾を撃ったオブジェクトの設定
    public void SetOwner(GameObject Owner)
    {
        owner = Owner;
    }

}
