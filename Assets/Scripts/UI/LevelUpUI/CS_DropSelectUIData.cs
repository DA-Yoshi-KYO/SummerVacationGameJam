/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   レベルアップ選択UIからアイコン画像だけを受け取って表示する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;
using UnityEngine.UI;
using static CSO_WeaponDataBase;
using static CSO_WeaponLevelData;

public class CS_DropSelectUIData : MonoBehaviour
{
    [Header("プレイヤー武器スロットUI")]
    [SerializeField] private CS_PlayerWeaponSlotUI playerSlotUI;

    [Header("武器データベース（WeaponDataBase）")]
    [SerializeField] private CSO_WeaponDataBase weaponDataBase;

    //武器データを受け取り、アイコン画像をUIに反映する
    public void SetUI(CSO_WeaponLevelData.WeaponLevelData weaponLevelData)
    {
        //WeaponLevelDataからWeaponDataBaseに変換
        var weaponData = ConvertToWeaponDataBase(weaponLevelData, weaponDataBase);

        //PlayerWeaponSlotのUIを更新
        playerSlotUI.SetupWeapon(weaponData);
    }

    public static CSO_WeaponDataBase.WeaponDataBase ConvertToWeaponDataBase(
      CSO_WeaponLevelData.WeaponLevelData levelData,
      CSO_WeaponDataBase dataBase)
    {
        var baseData = dataBase.weaponDatas[levelData.weaponName].CloneData();

        baseData.weaponName = levelData.weaponName;
        baseData.weaponIcon = levelData.weaponIcon;

        //レベルアップ後の現在レベルで Evaluateする
        int level = levelData.currentLevel;

        baseData.currentLevel = level;
        baseData.damage = levelData.damage.Evaluate(level);
        baseData.fireRate = levelData.fireRate.Evaluate(level);
        baseData.range = levelData.range.Evaluate(level);
        baseData.bulletCount = Mathf.RoundToInt(levelData.bulletCount.Evaluate(level));
        baseData.reloadTime = levelData.reloadTime.Evaluate(level);

        return baseData;
    }

}