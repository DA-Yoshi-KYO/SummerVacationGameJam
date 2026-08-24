using UnityEngine;

public class CS_WeaponStatusUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("武器の弾数増加率")]
    private float _bulletCountIncreaseRate = 0.5f;

    [SerializeField]
    [Header("武器の連射速度増加率")]
    private float _fireRateIncreaseRate = 0.3f;

    [SerializeField]
    [Header("武器のリロード時間減少率")]
    private float _reloadTimeReductionRate = 0.3f;


    protected override void ApplyEffectLevel1()
    {
        // 武器の弾数増加率を50%加算
        _chipManager.upgradeStatus.upgradeStatus.bulletCountIncreaseRate += _bulletCountIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        // 武器の連射速度増加率を30%加算
        _chipManager.upgradeStatus.upgradeStatus.fireRateIncreaseRate += _fireRateIncreaseRate;
    }

    protected override void ApplyEffectLevel3()
    {
        // 武器のリロード時間減少率を30%加算
        _chipManager.upgradeStatus.upgradeStatus.reloadSpeedIncreaseRate += _reloadTimeReductionRate;
    }

    protected override void ApplyEffectLevel4()
    {
        // HPが少ない敵を優先的に狙うようになるエフェクトを追加させる
        // 対象: プレイヤーの武器
    }

    protected override void ApplyEffectLevel5()
    {
        // 発射される弾の数が+1されるエフェクトを追加させる
        // 対象: プレイヤーの武器
        // (ショットガンの場合は5ペレットの場合、10ペレットになる)
    }
}