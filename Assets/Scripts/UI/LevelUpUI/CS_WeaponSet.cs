/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   武器のデータを設定する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_WeaponSet : MonoBehaviour
{
    [Header("アイコンの画像")][SerializeField] private Image iconImage;
    public CS_SelectUISet currentWeapon;//現在の武器

    public CS_WeaponLevelUpUI weaponLevelUpUI;

    private void Start()
    {
        if(currentWeapon == null)
        {
            iconImage.enabled = false;
            weaponLevelUpUI.enabled = false;
        }
    }

    //UIの表示を設定する
    public void SetUI(CS_SelectUISet weapon)
    {
        iconImage.enabled = true;
        weaponLevelUpUI.enabled = true;
        currentWeapon = weapon;
        iconImage.sprite = weapon.GetData().weapon.weaponIcon;
    }
}
