using UnityEngine;

public class CS_PulseFieldWeapon : CS_BaseWeapon
{
    [Tooltip("生成した弾")]
    private CS_BaseBullet _bulletObject;

    [Tooltip("武器を使用しているキャラクターのゲームオブジェクト")]
    private GameObject _UseObject;

    public override void Start()
    {
        weaponName = "PulseField";

        base.Start();

        // ※仮※
        // 競合の可能性があるため
        // プレイヤー装備想定で記載
        // CS_PlayerEquipmentに本来は装備するときに設定させる
        _UseObject = GameObject.FindFirstObjectByType<CS_PlayerEquipment>().gameObject;
    }

    private void Update()
    {
        // 既に弾が存在する場合は生成しない
        if (_bulletObject != null)
        {
            if (_bulletObject.gameObject.activeSelf) return;
        }

        // 弾(ダメージフィールド)を生成
        _bulletObject = base.ActivateBullet();

        if (_bulletObject is CS_DamageFieldBullet damageFieldBullet)
        {
            // ダメージフィールドの範囲を設定
            damageFieldBullet.SetDamageFieldRange(base.weaponData.range);

            // 自分自身をターゲットに設定
            damageFieldBullet.SetTarget(_UseObject);
        }
        else
        {
            Debug.LogError("ダメージフィールド弾が使用されていません");
        }
    }

    protected override void Shot()
    {
    }

    public void SetUseObject(GameObject useObject)
    {
        _UseObject = useObject;
    }
}
