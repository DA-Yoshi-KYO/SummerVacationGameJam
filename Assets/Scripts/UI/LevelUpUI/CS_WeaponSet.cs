using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CS_WeaponSet : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    // 今このスロットにセットされている武器
    public CS_SelectUISet currentWeapon;

    // レベルアップUI
    public CS_WeaponLevelUpUI weaponLevelUpUI;

    // UI の表示を更新する（武器アイコンなど）
    public void SetUI(CS_SelectUISet weapon)
    {
        currentWeapon = weapon;
        iconImage.sprite = weapon.GetData().weapon.weaponIcon;
        // ここでアイコンや名前を更新する処理を書く
        // 例：
        // iconImage.sprite = weapon.icon;
        // nameText.text = weapon.weaponName;
    }
}
