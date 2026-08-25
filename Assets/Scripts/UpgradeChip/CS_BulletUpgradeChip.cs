using UnityEngine;

public class CS_BulletUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("弾のサイズ増加率")]
    private float _bulletSizeIncreaseRate = 0.2f;

    [SerializeField]
    [Header("弾の射程増加率")]
    private float _bulletRangeIncreaseRate = 0.3f;

    [SerializeField]
    [Header("弾の貫通力増加量")]
    private int _bulletPenetrationIncreaseAmount = 1;

    [SerializeField]
    [Header("弾の速度増加率")]
    private float _bulletSpeedIncreaseRate = 0.3f;

    private void Start()
    {
        _chipName = "BulletUpgradeChip";
    }

    protected override void ApplyEffectLevel1()
    {
        if(UpgradeStatus_NameCheck("EffectLevel1")) return;

        // 弾のサイズ増加率を加算
        _chipManager.upgradeStatus.upgradeStatus.bulletSizeIncreaseRate += _bulletSizeIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        if(UpgradeStatus_NameCheck("EffectLevel2")) return;

        // 弾の射程増加率を加算
        _chipManager.upgradeStatus.upgradeStatus.bulletRangeIncreaseRate += _bulletRangeIncreaseRate;
    }

    protected override void ApplyEffectLevel3()
    {
        if(UpgradeStatus_NameCheck("EffectLevel3")) return;

        // 弾の貫通力増加量を加算
        _chipManager.upgradeStatus.upgradeStatus.bulletPenetrationIncreaseAmount += _bulletPenetrationIncreaseAmount;
    }

    protected override void ApplyEffectLevel4()
    {
        if(UpgradeStatus_NameCheck("EffectLevel4")) return;

        // 弾の速度増加率を加算
        _chipManager.upgradeStatus.upgradeStatus.bulletSpeedIncreaseRate += _bulletSpeedIncreaseRate;
    }

    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // 弾が着弾した際、一定範囲の敵にダメージを与えるエフェクトを追加
    }
}