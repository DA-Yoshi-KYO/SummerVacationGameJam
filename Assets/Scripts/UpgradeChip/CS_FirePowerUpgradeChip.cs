using UnityEngine;

public class CS_FirePowerUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("与えるダメージ増加率")]
    private float _damageIncreaseRate = 0.2f;

    private void Start()
    {
        _chipName = "FirePowerUpgradeChip";
    }

    protected override void ApplyEffectLevel1()
    {
        if(UpgradeStatus_NameCheck("EffectLevel1")) return;

        // 与えるダメージ増加率を20%加算
        _chipManager.upgradeStatus.upgradeStatus.damageIncreaseRate += _damageIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        if (UpgradeStatus_NameCheck("EffectLevel2")) return;

        // 与えるダメージが20%の確率で2倍になるエフェクトを追加させる
        // 対象：弾
    }

    protected override void ApplyEffectLevel3()
    {
        if (UpgradeStatus_NameCheck("EffectLevel3")) return;

        // 命中時に継続ダメージデバフを付与するエフェクトを追加させる
        // 対象：弾
    }

    protected override void ApplyEffectLevel4()
    {
        if (UpgradeStatus_NameCheck("EffectLevel4")) return;

        // HPが30%以下の時、与えるダメージを1.3倍するエフェクトを追加させる
        // 対象：弾
    }

    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // 撃破数が10体事に威力が0.1%上昇するエフェクトを追加させる
        // 対象：弾
    }
}