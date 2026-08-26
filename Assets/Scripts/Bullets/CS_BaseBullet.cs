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

    protected bool isActive = false;//弾がアクティブかどうか

    protected CS_UpgradeChipManager _upgradeChipManager;

    private void Awake()
    {
        _upgradeChipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();
    }

    private void Start()
    {
        Collider collider = GetComponent<Collider>();

        if (collider is CapsuleCollider capsule)
        {
            capsule.radius *= _upgradeChipManager.upgradeStatus.getupgradeStatus.bulletSizeIncreaseRate;
            capsule.height *= _upgradeChipManager.upgradeStatus.getupgradeStatus.bulletSizeIncreaseRate;
        }
        else if (collider is SphereCollider sphere)
        {
            sphere.radius *= _upgradeChipManager.upgradeStatus.getupgradeStatus.bulletSizeIncreaseRate;
        }
        else if (collider is BoxCollider box)
        {
            box.size *= _upgradeChipManager.upgradeStatus.getupgradeStatus.bulletSizeIncreaseRate;
        }
    }

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

        int currentDamage = (int)(damage * _upgradeChipManager.upgradeStatus.getupgradeStatus.damageIncreaseRate);

        foreach (var effect in _upgradeChipManager.damageBoostEffects)
        {
            currentDamage += effect.DamageUp((int)damage, other.gameObject);
        }

        //ダメージを与える処理
        Debug.Log("Hit " + other.gameObject.name + " Damage: " + currentDamage);

        // ライフスティール効果がある場合発動
        CS_LifeStealEffect lifeStealEffect = GameObject.FindAnyObjectByType<CS_LifeStealEffect>();
        if (lifeStealEffect != null)
            lifeStealEffect.ApplyEffect(currentDamage);

        // 継続ダメージ効果がある場合発動
        CS_DotBulletEffect dotBulletEffect = GameObject.FindAnyObjectByType<CS_DotBulletEffect>();
        if (dotBulletEffect != null)
            dotBulletEffect.ApplyDotEffect(other.gameObject, currentDamage);

        Deactivate();
    }

    //弾の移動処理
    protected virtual void BulletMovement()
    {
        transform.position += transform.forward * (speed * _upgradeChipManager.upgradeStatus.getupgradeStatus.bulletSpeedIncreaseRate) * Time.deltaTime;
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
