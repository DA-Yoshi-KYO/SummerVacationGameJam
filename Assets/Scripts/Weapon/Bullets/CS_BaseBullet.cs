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
    protected float damage;
    protected float speed;
    protected GameObject owner;//弾を撃ったオブジェクト

    protected bool isActive = false;

    protected virtual void Update()
    {
        if (!isActive)
            return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.gameObject == owner)
            return;

        //ダメージを与える処理
        Debug.Log("Hit " + other.gameObject.name + " Damage: " + damage);

        Deactivate();
    }

    //プール用のリセット処理
    public virtual void Activate(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;

        isActive = true;
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        isActive = false;

        speed = 0f;
        owner = null;

        gameObject.SetActive(false);
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
