using Unity.VisualScripting;
using UnityEngine;

public class CS_HpUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("最大HP増加量")]
    private int _healthIncreaseAmount = 50; // 最大HP増加量

    [SerializeField]
    [Header("ダメージ軽減率")]
    [Range(0.0f, 1.0f)]
    private float _damageReductionRate = 0.2f; // ダメージ軽減率

    [Header("===== HP自然回復 =====")]

    [SerializeField]
    [Tooltip("HP自然回復量")]
    private int _hpRegenerationAmount = 5; // HP自然回復量

    [SerializeField]
    [Tooltip("HP自然回復間隔")]
    private float _hpRegenerationInterval = 1.0f; // HP自然回復間隔

    [Header("===== ライフスティール =====")]

    [SerializeField]
    [Tooltip("ライフスティール回復率")]
    [Range(0.0f, 1.0f)]
    private float _lifeStealRate = 0.1f; // ライフスティール回復率


    [Header("===== 敵撃破時HP回復 =====")]

    [SerializeField]
    [Tooltip("敵撃破時HP回復量")]
    private int _killHpRecoveryAmount = 5; // 敵撃破時HP回復量


    private CS_PlayerStatus _playerStatus;

    private void Start()
    {
        _chipName = "HpUpgradeChip";
        _level = 1;

        _playerStatus = _player.GetComponent<CS_PlayerStatus>();
    }

    protected override void ApplyEffectLevel1()
    {
        if(UpgradeStatus_NameCheck("EffectLevel1")) return;

        // 最大HPを50増加
        _chipManager.upgradeStatus.upgradeStatus.healthIncreaseAmount += _healthIncreaseAmount;
    }

    protected override void ApplyEffectLevel2()
    {
        if (UpgradeStatus_NameCheck("EffectLevel2")) return;

        // HP自然回復エフェクトコンポーネントを追加させる
        CS_HpRegeneration hpRegeneration = _player.AddComponent<CS_HpRegeneration>();

        // 回復量と回復間隔を設定
        hpRegeneration.SetRegenerationAmount(_hpRegenerationAmount);
        hpRegeneration.SetRegenerationInterval(_hpRegenerationInterval);

        // プレイヤーのステータスを設定
        hpRegeneration.SetTarget(_playerStatus);
    }

    protected override void ApplyEffectLevel3()
    {
        if(UpgradeStatus_NameCheck("EffectLevel3")) return;

        // ダメージ軽減率を20%加算
        _chipManager.upgradeStatus.upgradeStatus.damageReductionRate += _damageReductionRate;
    }

    protected override void ApplyEffectLevel4()
    {
        if (UpgradeStatus_NameCheck("EffectLevel4")) return;

        CS_LifeStealEffect lifeStealEffect = _player.AddComponent<CS_LifeStealEffect>();

        // 回復量を設定
        lifeStealEffect.SetHealRate(_lifeStealRate);
        // プレイヤーのステータスを設定
        lifeStealEffect.SetPlayerStatus(_playerStatus);
    }

    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // 敵を倒したときにHPを回復するエフェクトを追加させる
        CS_KillHpRecoveryEffect killHpRecoveryEffect = _player.AddComponent<CS_KillHpRecoveryEffect>();

        // 回復量を設定
        killHpRecoveryEffect.SetRecoveryHpAmount(_killHpRecoveryAmount);

        // プレイヤーのステータスを設定
        killHpRecoveryEffect.SetPlayerStatus(_playerStatus);
    }
}