/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   武器のデータを設定する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CS_WeaponSet : MonoBehaviour
{
    [Header("アイコンの画像")][SerializeField] private Image iconImage;
    [Header("レベルのテキスト")][SerializeField] private TextMeshProUGUI levelText;
    [Header("現在の武器(見るためのもののため格納必要じゃない)")]public CS_SelectUISet currentWeapon;

    [Header("レベルのテキスト")]public CS_WeaponLevelUpUI weaponLevelUpUI;

    private void Start()
    {
        if(currentWeapon == null)
        {
            iconImage.enabled = false;
            levelText.enabled = false;
        }
    }

    //UIの表示を設定する
    public void SetUI(CS_SelectUISet weapon)
    {
        iconImage.enabled = true;
        levelText.enabled = true;
        currentWeapon = weapon;
        iconImage.sprite = weapon.GetData().weapon.weaponIcon;
    }
}
