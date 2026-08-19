/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　武器スロットUIのデータベース
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-16 | 初回作成
 */
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CSO_UIWeaponDataBase", menuName = "UIWeapon/CSO_UIWeaponDataBase")]
public class CSO_UIWeaponDataBase : ScriptableObject
{
    [System.Serializable]
    public class UIWeaponDataBase
    {
        [Header("武器の名前")] public string weaponName;
        [Header("武器のアイコン")] public Sprite weaponIcon;
        [Header("初期の弾数")] public int initBullets;
    }

    [SerializeField] private List<UIWeaponDataBase> weaponList;//Inspectorで設定するための変数
    private Dictionary<string, UIWeaponDataBase> weaponDictionary;//実行時にDictionaryに変換するための変数

    //読み取り専用変数
    public IReadOnlyDictionary<string, UIWeaponDataBase> weaponDatas
    {
        get
        {
            if (weaponDictionary == null)
            {
                weaponDictionary = new Dictionary<string, UIWeaponDataBase>();
                foreach (var weapon in weaponList)
                    weaponDictionary[weapon.weaponName] = weapon;
            }
            return weaponDictionary;
        }
    }
}

