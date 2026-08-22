using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CS_DropSelectUIData : MonoBehaviour
{
    [SerializeField] private Image icon;
    //[SerializeField] private TextMeshProUGUI nameText;

    public void SetUI(CSO_WeaponLevelData.WeaponLevelData weapon)
    {
        icon.sprite = weapon.weaponIcon;
        //nameText.text = weapon.weaponName;
    }
}
