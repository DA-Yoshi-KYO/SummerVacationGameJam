using System.Collections.Generic;
using UnityEngine;


public class CS_UpgradeChipManager : MonoBehaviour
{
    [SerializeField] 
    [Header("アップグレードステータス")]
    private CSO_UpgradeStatus _upgradeStatus;
    public CSO_UpgradeStatus upgradeStatus => _upgradeStatus;

    private List<CS_UpgradeChipBase> upgradeChips = new List<CS_UpgradeChipBase>();
}