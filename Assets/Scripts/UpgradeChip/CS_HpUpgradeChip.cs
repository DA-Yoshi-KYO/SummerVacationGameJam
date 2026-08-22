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

    protected override void ApplyEffectLevel1()
    {
        // 最大HPを50増加
        _chipManager.upgradeStatus.healthIncreaseAmount += _healthIncreaseAmount;
    }

    protected override void ApplyEffectLevel2()
    {
        // HP自然回復エフェクトコンポーネントを追加させる
        // 対象：プレイヤー
    }

    protected override void ApplyEffectLevel3()
    {
        // ダメージ軽減率を20%加算
        _chipManager.upgradeStatus.damageReductionRate += _damageReductionRate;
    }

    protected override void ApplyEffectLevel4()
    {
        // 与えたダメージの10%をHPとして回復するエフェクトを追加させる
        // 対象：プレイヤーの武器
    }

    protected override void ApplyEffectLevel5()
    {
        // 敵を倒したときにHPを5回復するエフェクトを追加させる
        // 対象：敵 ? 弾 ? プレイヤー
    }
}