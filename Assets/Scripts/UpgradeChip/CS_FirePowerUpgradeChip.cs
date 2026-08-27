using UnityEngine;

public class CS_FirePowerUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("与えるダメージ増加率")]
    private float _damageIncreaseRate = 0.2f;

    [Header("与えるダメージが確率で増加するエフェクト")]

    [Tooltip("発動確率")]
    [SerializeField]
    private float _criticalBoostChance = 0.2f;
    [Tooltip("ダメージ増加率")]
    [SerializeField]
    private float _criticalBoostDamageRate = 2.0f;

    [Header("相手のHPが一定値以下の時、与えるダメージを増加するエフェクト")]

    [Tooltip("HPが一定値以下の時の閾値")]
    [SerializeField]
    private float _lowHpThreshold = 0.3f;
    [Tooltip("ダメージ増加率")]
    [SerializeField]
    private float _lowHpDamageIncreaseRate = 1.3f;

    [Header("撃破数が一定数事に威力が増加するエフェクト")]

    [Tooltip("ダメージ上昇を適用するカウント区切り数")]
    [SerializeField]
    private int _killStackThreshold = 10;
    [Tooltip("撃破数スタックごとのダメージ増加率")]
    [SerializeField]
    private float _killStackDamageIncreaseRate = 0.1f;

    private void Start()
    {
        _chipName = "FirePowerUpgradeChip";
        _level = 1;
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

        // 与えるダメージが確率で増加するエフェクトを追加
        CS_CriticalBoostEffect criticalBoostEffect = new CS_CriticalBoostEffect();

        criticalBoostEffect.SetUpDamageRate(_criticalBoostChance); // 発動確率
        criticalBoostEffect.SetCriticalRate(_criticalBoostDamageRate); // ダメージ増加率

        _chipManager.AddDamageBoostEffect(criticalBoostEffect);
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

        // 敵のHPが一定値以下の時、与えるダメージを増加するエフェクトを追加
        CS_LowHpBoostEffect lowHpBoostEffect = new CS_LowHpBoostEffect();

        lowHpBoostEffect.SetLowHpThreshold(_lowHpThreshold); // HP閾値
        lowHpBoostEffect.SetUpDamageRate(_lowHpDamageIncreaseRate); // ダメージ増加率

        _chipManager.AddDamageBoostEffect(lowHpBoostEffect);
    }


    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // 撃破数が一定数事に威力が増加するエフェクトを追加
        CS_KillStackDamageBoostEffect killStackDamageBoostEffect = new CS_KillStackDamageBoostEffect();

        killStackDamageBoostEffect.SetKillStackThreshold(_killStackThreshold); // 撃破数閾値
        killStackDamageBoostEffect.SetUpDamageRate(_killStackDamageIncreaseRate); // ダメージ増加率

        _chipManager.AddDamageBoostEffect(killStackDamageBoostEffect);
    }
}