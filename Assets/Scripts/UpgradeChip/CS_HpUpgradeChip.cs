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

    private void Start()
    {
        _chipName = "HpUpgradeChip";
        _level = 1;
    }

    protected override void ApplyEffectLevel1()
    {
        if(UpgradeStatus_NameCheck("EffectLevel1")) return;

        // 最大HPを50増加
        _chipManager.upgradeStatus.upgradeStatus.healthIncreaseAmount += _healthIncreaseAmount;
    }

    protected override void ApplyEffectLevel2()
    {
        if(UpgradeStatus_NameCheck("EffectLevel2")) return;

        // HP自然回復エフェクトコンポーネントを追加させる
        CS_HpRegeneration hpRegeneration = _player.AddComponent<CS_HpRegeneration>();

        // 回復量と回復間隔を設定
        hpRegeneration.SetRegenerationAmount(_hpRegenerationAmount);
        hpRegeneration.SetRegenerationInterval(_hpRegenerationInterval);

        // プレイヤーのステータスを取得して、回復対象として設定
        CS_PlayerStatus playerStatus = _player.GetComponent<CS_PlayerStatus>();
        if (playerStatus != null)
            hpRegeneration.SetTarget(playerStatus);
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

        // 与えたダメージの10%をHPとして回復するエフェクトを追加させる
        // 対象：弾
    }

    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // 敵を倒したときにHPを5回復するエフェクトを追加させる
        // 対象： 弾
    }
}