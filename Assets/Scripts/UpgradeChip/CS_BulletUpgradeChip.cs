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


    protected override void ApplyEffectLevel1()
    {
        // 弾のサイズ増加率を加算
        _chipManager.upgradeStatus.bulletSizeIncreaseRate += _bulletSizeIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        // 弾の射程増加率を加算
        _chipManager.upgradeStatus.bulletRangeIncreaseRate += _bulletRangeIncreaseRate;
    }

    protected override void ApplyEffectLevel3()
    {
        // 弾の貫通力増加量を加算
        _chipManager.upgradeStatus.bulletPenetrationIncreaseAmount += _bulletPenetrationIncreaseAmount;
    }

    protected override void ApplyEffectLevel4()
    {
        // 弾の速度増加率を加算
        _chipManager.upgradeStatus.bulletSpeedIncreaseRate += _bulletSpeedIncreaseRate;
    }

    protected override void ApplyEffectLevel5()
    {
        // 弾が着弾した際、一定範囲の敵にダメージを与えるエフェクトを追加させる
        // 対象：弾
    }
}