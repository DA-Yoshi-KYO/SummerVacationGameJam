using UnityEngine;

public class CS_WeaponStatusUpgradeChip : CS_UpgradeChipBase
{
    [SerializeField]
    [Header("武器の弾数増加率")]
    private float _bulletCountIncreaseRate = 0.5f;

    [SerializeField]
    [Header("武器の連射速度増加率")]
    private float _fireRateIncreaseRate = 0.3f;

    [SerializeField]
    [Header("武器のリロード時間減少率")]
    private float _reloadTimeReductionRate = 0.3f;

    private void Start()
    {
        _chipName = "WeaponStatusUpgradeChip";
        _level = 1;
    }

    protected override void ApplyEffectLevel1()
    {
        if(UpgradeStatus_NameCheck("EffectLevel1")) return;

        // 武器の弾数増加率を50%加算
        _chipManager.upgradeStatus.upgradeStatus.bulletCountIncreaseRate += _bulletCountIncreaseRate;
    }

    protected override void ApplyEffectLevel2()
    {
        if(UpgradeStatus_NameCheck("EffectLevel2")) return;

        // 武器の連射速度増加率を30%加算
        _chipManager.upgradeStatus.upgradeStatus.fireRateIncreaseRate += _fireRateIncreaseRate;
    }

    protected override void ApplyEffectLevel3()
    {
        if(UpgradeStatus_NameCheck("EffectLevel3")) return;

        // 武器のリロード時間減少率を30%加算
        _chipManager.upgradeStatus.upgradeStatus.reloadSpeedIncreaseRate += _reloadTimeReductionRate;
    }

    protected override void ApplyEffectLevel4()
    {
        if (UpgradeStatus_NameCheck("EffectLevel4")) return;

        // HPが少ない敵を優先的に狙うようにする
        foreach(var weapon in _playerEquipment.equipmentWeaponScriptList)
        {
            weapon.ChangeTargetSystem(new CS_LowHpWeaponTarget());
        }
    }

    protected override void ApplyEffectLevel5()
    {
        if (UpgradeStatus_NameCheck("EffectLevel5")) return;

        // 発射される弾の数が+1する
        foreach(var weapon in _playerEquipment.equipmentWeaponScriptList)
        {
            weapon.multipleShotCount = 2;
        }
    }
}