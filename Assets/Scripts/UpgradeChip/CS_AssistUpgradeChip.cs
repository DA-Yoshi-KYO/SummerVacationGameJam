using UnityEngine;

public class CS_AssistUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("取得経験値の増加率")]
    private float _experienceGainIncreaseRate = 0.2f;

    [SerializeField]
    [Header("プレイヤーの移動速度増加率")]
    private float _playerMovementSpeedIncreaseRate = 0.5f;

    [SerializeField]
    [Header("チップの効果量増加率")]
    private float _allChipEffectIncreaseRate = 1.2f;

    protected override void ApplyEffectLevel1()
    {
        // 経験値獲得量増加率を20%加算
        _chipManager.upgradeStatus.upgradeStatus.experienceGainIncreaseRate += _experienceGainIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        // プレイヤーの移動速度増加率を50%加算
        _chipManager.upgradeStatus.upgradeStatus.playerMovementSpeedIncreaseRate += _playerMovementSpeedIncreaseRate;
    }

    protected override void ApplyEffectLevel3()
    {
        // バリアを生成するエフェクトを追加させる
        // 対象: プレイヤー
    }

    protected override void ApplyEffectLevel4()
    {
        // HPが10%以下になった時に、様々なバフを付与するエフェクトを追加させる
        // 対象: プレイヤー
    }

    protected override void ApplyEffectLevel5()
    {
        // すべてのチップの効果量を20%増加させる(1.2倍)
        _chipManager.upgradeStatus._allChipEffectIncreaseRate = _allChipEffectIncreaseRate;
    }
}