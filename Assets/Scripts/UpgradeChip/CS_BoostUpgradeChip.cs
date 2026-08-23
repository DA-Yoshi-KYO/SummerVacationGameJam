using UnityEngine;

public class CS_BoostUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("ブーストエネルギー増加量")]
    private int _boostEnergyIncreaseAmount = 100;

    [SerializeField]
    [Header("ブースト消費軽減率")]
    private float _boostConsumptionReductionRate = 0.1f;

    [SerializeField]
    [Tooltip("ブースト時の移動速度増加率")]
    private float _boostSpeedIncreaseRate = 0.5f;


    protected override void ApplyEffectLevel1()
    {
        // ブーストエネルギー増加量を100増加
        _chipManager.upgradeStatus.boostEnergyIncreaseAmount += _boostEnergyIncreaseAmount;
    }

    protected override void ApplyEffectLevel2()
    {
        // ブーストエネルギー自然回復エフェクトコンポーネントを追加させる
        // 対象：プレイヤー
    }

    protected override void ApplyEffectLevel3()
    {
        // ブースト消費軽減率を10%加算
        _chipManager.upgradeStatus.boostConsumptionReductionRate += _boostConsumptionReductionRate;
    }

    protected override void ApplyEffectLevel4()
    {
        // ブースト時の移動速度増加率を50%加算
        _chipManager.upgradeStatus.boostSpeedIncreaseRate += _boostSpeedIncreaseRate;
    }

    protected override void ApplyEffectLevel5()
    {
        // 敵を倒したときにブーストエネルギーを5回復するエフェクトを追加させる
        // 対象：プレイヤー
    }
}