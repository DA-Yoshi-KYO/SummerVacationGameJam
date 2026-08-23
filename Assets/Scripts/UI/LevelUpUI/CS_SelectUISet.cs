/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   レベルアップ選択画面に表示されるUIを管理する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CS_SelectUISet : MonoBehaviour
{
    [Header("UI表示用のアイコン")][SerializeField] private Image icon;
    [Header("UI表示用のテキスト")][SerializeField] private TextMeshProUGUI nameText;

    private CS_LevelUpSelectUI.UpgradeWrapper data;

    //レベルアップ候補のデータをUIに反映
    public void SetData(CS_LevelUpSelectUI.UpgradeWrapper d)
    {
        data = d;

        if (d.isWeapon)
        {
            //武器の場合、武器名と武器アイコンを表示
            nameText.text = d.weapon.weaponName;
            icon.sprite = d.weapon.weaponIcon;
        }
        else
        {
            //本来ならチップのデータをいれる
            nameText.text = "Chipp";
            icon.sprite = d.weapon.weaponIcon;
        }
    }

    //他のクラスがこのUIのデータを取得するための関数
    public CS_LevelUpSelectUI.UpgradeWrapper GetData()
    {
        return data;
    }
}
