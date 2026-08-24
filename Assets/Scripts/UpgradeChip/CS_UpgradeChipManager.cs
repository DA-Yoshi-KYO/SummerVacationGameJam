using System.Collections.Generic;
using UnityEngine;


public class CS_UpgradeChipManager : MonoBehaviour
{
    [Tooltip("アップグレードステータス")]
    private CSO_UpgradeStatus _upgradeStatus;
    public CSO_UpgradeStatus upgradeStatus => _upgradeStatus;

    private List<CS_UpgradeChipBase> upgradeChips = new List<CS_UpgradeChipBase>();

    private void Start()
    {
        _upgradeStatus = ScriptableObject.CreateInstance<CSO_UpgradeStatus>();

        // すでにアタッチされているアップグレードチップを取得
        CS_UpgradeChipBase[] existingChips = GetComponents<CS_UpgradeChipBase>();
        foreach (var chip in existingChips)
        {
            if (!upgradeChips.Contains(chip))
            {
                upgradeChips.Add(chip);
            }
        }
    }

    // ============ デバッグ用メソッド ============ //

    [SerializeField]
    [Header("デバッグ用：追加するアップグレードチップの名前")]
    private string debugAddChipName;
    [ContextMenu("アップグレードチップをリストに追加")]
    public void AddUpgradeChipsToList()
    {
        switch (debugAddChipName)
        {
            case "Bullet":
                CS_BulletUpgradeChip bulletUpgradeChip = transform.GetComponent<CS_BulletUpgradeChip>();
                if (bulletUpgradeChip == null)
                {
                    bulletUpgradeChip = transform.gameObject.AddComponent<CS_BulletUpgradeChip>();
                    upgradeChips.Add(bulletUpgradeChip);
                }
                break;
            case "WeaponStatus":
                CS_WeaponStatusUpgradeChip weaponStatusUpgradeChip = transform.GetComponent<CS_WeaponStatusUpgradeChip>();
                if (weaponStatusUpgradeChip == null)
                {
                    weaponStatusUpgradeChip = transform.gameObject.AddComponent<CS_WeaponStatusUpgradeChip>();
                    upgradeChips.Add(weaponStatusUpgradeChip);
                }
                break;
            case "Hp":
                CS_HpUpgradeChip hpUpgradeChip = transform.GetComponent<CS_HpUpgradeChip>();
                if (hpUpgradeChip == null)
                {
                    hpUpgradeChip = transform.gameObject.AddComponent<CS_HpUpgradeChip>();
                    upgradeChips.Add(hpUpgradeChip);
                }
                break;
            case "Boost":
                CS_BoostUpgradeChip boostUpgradeChip = transform.GetComponent<CS_BoostUpgradeChip>();
                if (boostUpgradeChip == null)
                {
                    boostUpgradeChip = transform.gameObject.AddComponent<CS_BoostUpgradeChip>();
                    upgradeChips.Add(boostUpgradeChip);
                }
                break;
            case "Assist":
                CS_AssistUpgradeChip assistUpgradeChip = transform.GetComponent<CS_AssistUpgradeChip>();
                if (assistUpgradeChip == null)
                {
                    assistUpgradeChip = transform.gameObject.AddComponent<CS_AssistUpgradeChip>();
                    upgradeChips.Add(assistUpgradeChip);
                }
                break;
            case "FirePower":
                CS_FirePowerUpgradeChip firePowerUpgradeChip = transform.GetComponent<CS_FirePowerUpgradeChip>();
                if (firePowerUpgradeChip == null)
                {
                    firePowerUpgradeChip = transform.gameObject.AddComponent<CS_FirePowerUpgradeChip>();
                    upgradeChips.Add(firePowerUpgradeChip);
                }
                break;
        }
    }

    [ContextMenu("すべてアップグレードチップの指定レベルを上げる")]
    public void LevelUpAllUpgradeChip()
    {
        foreach (var chip in upgradeChips)
        {
            chip.LevelUp();
        }
    }

    [ContextMenu("チップの効果を適用")]
    public void ApplyAllUpgradeChipEffects()
    {
        foreach (var chip in upgradeChips)
        {
            chip.ApplyEffect();
        }
    }
}