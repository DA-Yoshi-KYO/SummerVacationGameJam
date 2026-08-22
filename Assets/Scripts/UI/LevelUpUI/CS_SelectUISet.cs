using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CS_SelectUISet : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    private CS_LevelUpSelectUI.UpgradeWrapper data;

    public void SetData(CS_LevelUpSelectUI.UpgradeWrapper d)
    {
        data = d;

        if (d.isWeapon)
        {
            nameText.text = d.weapon.weaponName;
            icon.sprite = d.weapon.weaponIcon;
        }
        else
        {
            nameText.text = "Chipp";
            icon.sprite = d.weapon.weaponIcon;
        }
    }

    public CS_LevelUpSelectUI.UpgradeWrapper GetData()
    {
        return data;
    }

}
