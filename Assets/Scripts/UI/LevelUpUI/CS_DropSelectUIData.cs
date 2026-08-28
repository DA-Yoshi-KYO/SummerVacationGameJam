/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   レベルアップ選択UIからアイコン画像だけを受け取って表示する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_DropSelectUIData : MonoBehaviour
{
    [Header("表示するアイコン")][SerializeField] private Image icon;

    //武器データを受け取り、アイコン画像をUIに反映する
    public void SetUI(CSO_WeaponLevelData.WeaponLevelData weapon)
    {
        icon.sprite = weapon.weaponIcon;
    }
}
