using Unity.VisualScripting;
using UnityEngine;

public class CS_PlayerLowHpBuff : MonoBehaviour
{
    [SerializeField]
    [Tooltip("効果を発動するHPの割合")]
    private float _lowHpThreshold = 0.1f;

    [SerializeField]
    [Tooltip("再度効果を発動できるまでのクールダウン時間")]
    private float _cooldownTime = 60f;

    [SerializeField]
    [Tooltip("前回発動してからの経過時間")]
    private float _elapsedTime = 0f;

    [SerializeField]
    [Tooltip("バフの持続時間")]
    private float _buffDuration = 10f;

    [SerializeField]
    [Tooltip("バフの残り時間")]
    private float _buffRemainingTime = 0f;


    [Tooltip("プレイヤーのステータス参照")]
    private CS_PlayerStatus _playerStatus;

    [Tooltip("プレイヤーの参照")]
    private Transform _player;

    [Tooltip("強化チップマネージャーの参照")]
    private CS_UpgradeChipManager _chipManager;

    // ====================
    // バフの効果量
    // ====================

    [Tooltip("体力の回復割合")]
    private float _hpRegenerationRate = 0.1f;

    [Tooltip("ブーストエネルギーの回復割合")]
    private float _boostEnergyRegenerationRate = 0.1f;

    [Tooltip("通常移動速度の上昇割合")]
    private float _movementSpeedIncreaseRate = 0.5f;

    [Tooltip("ブースト移動速度の上昇割合")]
    private float _boostSpeedIncreaseRate = 0.5f;


    // 付与したバフ

    private CS_HpRegeneration _hpRegenerationBuff;
    private CS_BoostEnergyRegeneration _boostEnergyRegenerationBuff;

    private void Update()
    {
        if (_buffRemainingTime > 0f)
        {
            _buffRemainingTime -= Time.deltaTime;
            // バフの持続時間が終了した場合、バフを削除
            if (_buffRemainingTime > 0.0f) return;

            RemoveBuffs();
        }
        else
        {

            // 経過時間がクールダウン時間未満の場合は、経過時間を更新して終了
            if (_elapsedTime < _cooldownTime)
            {
                _elapsedTime += Time.deltaTime;
                return;
            }

            // プレイヤーの体力を確認
            if (_playerStatus == null) return;

            // 体力の割合を計算
            float currentHpRatio = _playerStatus.currentHealth / _playerStatus.maxHealth;

            // 体力の割合が閾値よりも高い場合は、効果を発動せずに終了
            if (currentHpRatio > _lowHpThreshold) return;

            ApplyBuffs();
        }
    }


    // ====================
    // 設定用のメソッド
    // ====================

    public void SetChipManager(CS_UpgradeChipManager chipManager)
    {
        _chipManager = chipManager;
    }

    public void SetCoolTime(float coolTime)
    {
        _cooldownTime = coolTime;
        _elapsedTime = _cooldownTime;
    }

    public void SetLowHpThreshold(float lowHpThreshold)
    {
        _lowHpThreshold = lowHpThreshold;
    }

    public void SetPlayer(Transform player)
    {
        _player = player;

        _playerStatus = _player.GetComponent<CS_PlayerStatus>();
    }

    public void SetBuffDuration(float buffDuration)
    {
        _buffDuration = buffDuration;
    }

    // --- それぞれのバフ効果量設定用のメソッド --- //

    public void SetHpRegenerationRate(float hpRegenerationRate)
    {
        _hpRegenerationRate = hpRegenerationRate;
    }

    public void SetBoostEnergyRegenerationRate(float boostEnergyRegenerationRate)
    {
        _boostEnergyRegenerationRate = boostEnergyRegenerationRate;
    }

    public void SetMovementSpeedIncreaseRate(float movementSpeedIncreaseRate)
    {
        _movementSpeedIncreaseRate = movementSpeedIncreaseRate;
    }

    public void SetBoostSpeedIncreaseRate(float boostSpeedIncreaseRate)
    {
        _boostSpeedIncreaseRate = boostSpeedIncreaseRate;
    }


    // ===================
    // プライベートメソッド
    // ===================

    private void ApplyBuffs()
    {
        // ====================
        // 体力回復バフ
        // ====================

        _hpRegenerationBuff = _player.AddComponent<CS_HpRegeneration>();

        // 体力の10%を回復するように設定
        float regenerationAmount = _playerStatus.maxHealth * _hpRegenerationRate;
        _hpRegenerationBuff.SetRegenerationAmount(regenerationAmount); // 回復量を設定
        _hpRegenerationBuff.SetRegenerationInterval(1f); // 回復間隔を設定
        _hpRegenerationBuff.SetTarget(_playerStatus); // 回復対象を設定

        // ====================
        // ブーストエネルギー回復バフ
        // ====================

        _boostEnergyRegenerationBuff = _player.AddComponent<CS_BoostEnergyRegeneration>();
        CS_PlayerMoveBoostEnergy playerBoostEnergy = _player.GetComponent<CS_PlayerMoveBoostEnergy>();

        // エネルギーの10%を回復するように設定
        float boostEnergyRegenerationAmount = playerBoostEnergy.maxEnergy * _boostEnergyRegenerationRate;
        _boostEnergyRegenerationBuff.SetRegenerationAmount(boostEnergyRegenerationAmount); // 回復量を設定
        _boostEnergyRegenerationBuff.SetRegenerationInterval(1f); // 回復間隔を設定
        _boostEnergyRegenerationBuff.SetTarget(playerBoostEnergy); // 回復対象を設定

        // ====================
        // シールドバフ
        // ====================

        CS_PlayerShield playerShield = _player.GetComponent<CS_PlayerShield>();
        if (playerShield == null) playerShield = _player.AddComponent<CS_PlayerShield>();

        // 一時的シールドを付与する
        playerShield.SetTempShieldDurability(30.0f);

        // ====================
        // 移動速度バフ
        // ====================

        _chipManager.upgradeStatus.upgradeStatus.playerMovementSpeedIncreaseRate += _movementSpeedIncreaseRate; // 移動速度を50%増加
        _chipManager.upgradeStatus.upgradeStatus.boostSpeedIncreaseRate += _boostSpeedIncreaseRate; // ブースト速度を50%増加

        // バフの持続時間を設定
        _buffRemainingTime = _buffDuration;

    }

    private void RemoveBuffs()
    {
        // 体力回復バフを削除
        if (_hpRegenerationBuff != null)
        {
            Destroy(_hpRegenerationBuff);
            _hpRegenerationBuff = null;
        }

        // ブーストエネルギー回復バフを削除
        if (_boostEnergyRegenerationBuff != null)
        {
            Destroy(_boostEnergyRegenerationBuff);
            _boostEnergyRegenerationBuff = null;
        }

        // シールドバフを削除
        CS_PlayerShield playerShield = _player.GetComponent<CS_PlayerShield>();
        if (playerShield != null)
        {
            playerShield.SetTempShieldDurability(0.0f); // 一時的シールドを削除
        }

        // 移動速度バフを元に戻す
        _chipManager.upgradeStatus.upgradeStatus.playerMovementSpeedIncreaseRate -= _movementSpeedIncreaseRate; // 移動速度を元に戻す
        _chipManager.upgradeStatus.upgradeStatus.boostSpeedIncreaseRate -= _boostSpeedIncreaseRate; // ブースト速度を元に戻す
    }
}
