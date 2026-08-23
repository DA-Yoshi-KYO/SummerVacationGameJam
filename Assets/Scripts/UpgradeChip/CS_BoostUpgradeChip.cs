using Unity.VisualScripting;
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

    [Header("===== ブーストエネルギー自然回復 =====")]

    [SerializeField]
    [Tooltip("ブーストエネルギー自然回復量")]
    private int _boostEnergyRecoveryAmount = 5;

    [SerializeField]
    [Tooltip("ブーストエネルギー自然回復間隔")]
    private float _boostEnergyRecoveryInterval = 1.0f;


    protected override void ApplyEffectLevel1()
    {
        // ブーストエネルギー増加量を100増加
        _chipManager.upgradeStatus.upgradeStatus.boostEnergyIncreaseAmount += _boostEnergyIncreaseAmount;
    }

    protected override void ApplyEffectLevel2()
    {
        // ブーストエネルギー自然回復エフェクトコンポーネントを追加させる
        CS_BoostEnergyRegeneration boostEnergyRecovery = _player.AddComponent<CS_BoostEnergyRegeneration>();

        // 回復量と回復間隔を設定
        boostEnergyRecovery.SetRegenerationAmount(_boostEnergyRecoveryAmount);
        boostEnergyRecovery.SetRegenerationInterval(_boostEnergyRecoveryInterval);

        // プレイヤーのブーストエネルギーを取得して、回復対象として設定
        CS_PlayerMoveBoostEnergy playerMoveBoostEnergy = _player.GetComponent<CS_PlayerMoveBoostEnergy>();

        if (playerMoveBoostEnergy != null)
            boostEnergyRecovery.SetTarget(playerMoveBoostEnergy);
    }

    protected override void ApplyEffectLevel3()
    {
        // ブースト消費軽減率を10%加算
        _chipManager.upgradeStatus.upgradeStatus.boostConsumptionReductionRate += _boostConsumptionReductionRate;
    }

    protected override void ApplyEffectLevel4()
    {
        // ブースト時の移動速度増加率を50%加算
        _chipManager.upgradeStatus.upgradeStatus.boostSpeedIncreaseRate += _boostSpeedIncreaseRate;
    }

    protected override void ApplyEffectLevel5()
    {
        // 敵を倒したときにブーストエネルギーを5回復するエフェクトを追加させる
        // 対象：プレイヤー
    }
}