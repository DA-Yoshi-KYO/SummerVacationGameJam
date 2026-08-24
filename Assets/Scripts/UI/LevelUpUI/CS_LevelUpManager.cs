/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   レベルアップで選択された武器（またはチップ）をプレイヤーの装備に反映する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;

public class CS_LevelUpManager : MonoBehaviour
{
    public static CS_LevelUpManager Instance;

    //プレイヤーが装備している武器データ
    public CSO_WeaponLevelData.WeaponLevelData[] playerWeapons = new CSO_WeaponLevelData.WeaponLevelData[6];

    //武器スロットの UI
    public CS_DropSelectUIData[] weaponSlots = new CS_DropSelectUIData[6];

    [Header("")][SerializeField] private CS_UpGradeCountUI upGradeCountUI;

    private void Awake()
    {
        Instance = this;
    }

    //レベルアップ選択UIから受け取った武器を指定スロットにセットする
    public void SetWeapon(int slotIndex, CS_SelectUISet selectUI)
    {
        var upgradeData = selectUI.GetData();

        if (!upgradeData.isWeapon)
        {
            //チップに関することをかく
            Debug.Log("武器じゃないデータがドロップされたよ");
            return;
        }
        else
        {
            //正しい型で受け取る
            var weapon = upgradeData.weapon;

            //プレイヤーの武器リストにセット
            playerWeapons[slotIndex] = weapon;

            //UIを更新
            weaponSlots[slotIndex].SetUI(weapon);

            //アップグレード回数を減らす
            upGradeCountUI.LevelDown();
        }
    }
}