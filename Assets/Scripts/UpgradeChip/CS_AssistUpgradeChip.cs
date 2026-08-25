using Unity.VisualScripting;
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

    [Header("===== バリア =====")]
    
    [SerializeField]
    [Tooltip("バリアの耐久値")]
    private float _barrierDurability = 50f;

    [SerializeField]
    [Tooltip("バリアが回復するまでの時間")]
    private float _barrierRecoveryTime = 5f;

    [Header("===== HPが10%以下になった時のバフ =====")]

    [SerializeField]
    [Tooltip("HPが10%以下になった時に付与されるバフの持続時間")]
    private float _buffDuration = 10f;

    [SerializeField]
    [Tooltip("バフが発動するHPの閾値")]
    [Range(0.0f, 1.0f)]
    private float _lowHpThreshold = 0.1f;

    [SerializeField]
    [Tooltip("バフのクールタイム")]
    private float _buffCoolTime = 60f;

    [Header("それぞれのバフの効果量")]

    [SerializeField]
    [Tooltip("体力の回復割合")]
    [Range(0.0f, 1.0f)]
    private float _hpRegenerationRate = 0.1f;

    [SerializeField]
    [Tooltip("ブーストエネルギーの回復割合")]
    [Range(0.0f, 1.0f)]
    private float _boostEnergyRegenerationRate = 0.1f;

    [SerializeField]
    [Tooltip("通常移動速度の上昇割合")]
    [Range(0.0f, 1.0f)]
    private float _movementSpeedIncreaseRate = 0.5f;

    [SerializeField]
    [Tooltip("ブースト移動速度の上昇割合")]
    [Range(0.0f, 1.0f)]
    private float _boostSpeedIncreaseRate = 0.5f;

    private void Start()
    {
        _chipName = "AssistUpgradeChip";
    }

    protected override void ApplyEffectLevel1()
    {
        if(UpgradeStatus_NameCheck("EffectLevel1")) return;

        // 経験値獲得量増加率を20%加算
        _chipManager.upgradeStatus.upgradeStatus.experienceGainIncreaseRate += _experienceGainIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        if(UpgradeStatus_NameCheck("EffectLevel2")) return;

        // プレイヤーの移動速度増加率を50%加算
        _chipManager.upgradeStatus.upgradeStatus.playerMovementSpeedIncreaseRate += _playerMovementSpeedIncreaseRate;
    }

    protected override void ApplyEffectLevel3()
    {
        if (UpgradeStatus_NameCheck("EffectLevel3")) return;

        // バリアを生成するエフェクトを追加させる
        CS_PlayerShield playerShield = _player.GetComponent<CS_PlayerShield>();
        if (playerShield == null) playerShield = _player.AddComponent<CS_PlayerShield>();

        // バリアの耐久値と回復時間を設定
        playerShield.SetShieldDurability(_barrierDurability);
        playerShield.SetShieldRegenTime(_barrierRecoveryTime);
    }

    protected override void ApplyEffectLevel4()
    {
        if (UpgradeStatus_NameCheck("EffectLevel4")) return;

        // HPが10%以下になった時に、様々なバフを付与するエフェクトを追加させる
        CS_PlayerLowHpBuff lowHpBuff = _player.AddComponent<CS_PlayerLowHpBuff>();

        // 参照の設定
        lowHpBuff.SetPlayer(_player);// プレイヤーのTransformを設定
        lowHpBuff.SetChipManager(_chipManager);// チップマネージャーを設定

        // 設定
        lowHpBuff.SetLowHpThreshold(_lowHpThreshold); // HPが10%以下になった時に発動
        lowHpBuff.SetBuffDuration(_buffDuration); // バフの持続時間を設定
        lowHpBuff.SetCoolTime(_buffCoolTime); // バフのクールタイムを設定

        // バフの効果量を設定
        lowHpBuff.SetHpRegenerationRate(_hpRegenerationRate); // 体力の回復割合
        lowHpBuff.SetBoostEnergyRegenerationRate(_boostEnergyRegenerationRate); // ブーストエネルギーの回復割合
        lowHpBuff.SetMovementSpeedIncreaseRate(_movementSpeedIncreaseRate); // 通常移動速度の上昇割合
        lowHpBuff.SetBoostSpeedIncreaseRate(_boostSpeedIncreaseRate); // ブースト移動速度の上昇割合
    }

    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // すべてのチップの効果量を20%増加させる(1.2倍)
        _chipManager.upgradeStatus.allChipEffectIncreaseRate = _allChipEffectIncreaseRate;
    }
}