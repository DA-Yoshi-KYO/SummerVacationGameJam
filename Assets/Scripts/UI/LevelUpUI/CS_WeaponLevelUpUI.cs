/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   武器のレベルを管理する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using TMPro;
using UnityEngine;

public class CS_WeaponLevelUpUI : MonoBehaviour
{
    [Header("レベルのテキスト")]public TextMeshProUGUI weaponLevelText;

    [Header("初期レベル")] public int initWeaponLevel;
    [Header("最大レベル")] public int maxWeaponLevel;

    [HideInInspector] public int currentWeaponLevel;//現在のレベル

    private CS_WeaponSet mySlot;

    void Start()
    {
        mySlot = GetComponentInParent<CS_WeaponSet>();

        weaponLevelText.text = initWeaponLevel.ToString();

        currentWeaponLevel = initWeaponLevel;
    }

    void Update()
    {
    }

    //レベルアップ
    public void LevelUp()
    {
        var data = mySlot.currentWeapon.GetData().weapon;

        if (data.currentLevel < data.maxLevel)
        {
            data.currentLevel++;
        }

        currentWeaponLevel++;
        currentWeaponLevel = Mathf.Clamp(currentWeaponLevel, initWeaponLevel, maxWeaponLevel);
        weaponLevelText.text = currentWeaponLevel.ToString();
    }

    //レベルダウン
    public void LevelDown()
    {
        currentWeaponLevel--;
        currentWeaponLevel = Mathf.Clamp(currentWeaponLevel, initWeaponLevel, maxWeaponLevel);
        weaponLevelText.text = currentWeaponLevel.ToString();
    }

    //レベル設定
    public void SetLevel(int level)
    {
        currentWeaponLevel = level;
        currentWeaponLevel = Mathf.Clamp(currentWeaponLevel, initWeaponLevel, maxWeaponLevel);
        weaponLevelText.text = currentWeaponLevel.ToString();
    }
}
