/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　弾丸の基底クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;
using System.Collections.Generic;

public class CS_BaseBullet : MonoBehaviour
{
    protected float damage;
    protected float speed;
    protected GameObject owner;//弾を撃ったオブジェクト

    protected bool isActive = false;//弾がアクティブかどうか

    private CS_UpgradeChipManager _upgradeChipManager;

    private void Update()
    {
        if (!isActive)
            return;

        BulletMovement();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.gameObject.tag == owner.transform.root.tag)
            return;

        int currentDamage = Mathf.RoundToInt(damage);

        if (_upgradeChipManager == null)
            _upgradeChipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();

        foreach (var effect in _upgradeChipManager.damageBoostEffects)
        {
            currentDamage += effect.DamageUp(currentDamage, other.gameObject);
        }

        //ダメージを与える処理
        Debug.Log("Hit " + other.gameObject.name + " Damage: " + currentDamage);

        Deactivate();
    }

    //弾の移動処理
    protected virtual void BulletMovement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //プール用のリセット処理

    //弾をアクティブにする処理
    public virtual void Activate(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;

        isActive = true;
        gameObject.SetActive(true);
    }
    //弾を非アクティブにする処理
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
