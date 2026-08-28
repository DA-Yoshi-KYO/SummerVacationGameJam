/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　武器のレベルデータ
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-20 | 初回作成
 */
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CSO_WeaponLevelData", menuName = "Weapon/CSO_WeaponLevelData")]
public class CSO_WeaponLevelData : ScriptableObject
{
    [System.Serializable]
    public class WeaponLevelData
    {
        [Header("武器の名前")] public string weaponName;
        [Header("武器のアイコン")] public Sprite weaponIcon;
        [Header("武器の説明")] public string text;
        [Header("最小レベル")]　public int minLevel;
        [Header("最大レベル")] public int maxLevel;
        [Header("現在のレベル")] public int currentLevel = 1;
        [Header("ダメージ")] public AnimationCurve damage;
        [Header("連射速度")] public AnimationCurve fireRate;
        [Header("射程距離")] public AnimationCurve range;
        [Header("弾数")] public AnimationCurve bulletCount;
        [Header("リロード時間")] public AnimationCurve reloadTime;
    }

    [SerializeField] private List<WeaponLevelData> weaponList;
    private Dictionary<string, WeaponLevelData> weaponDictionary;

    //読み取り専用変数
    public IReadOnlyDictionary<string, WeaponLevelData> weaponDatas
    {
        get
        {
            if (weaponDictionary == null)
            {
                weaponDictionary = new Dictionary<string, WeaponLevelData>();
                foreach (var weapon in weaponList)
                    weaponDictionary[weapon.weaponName] = weapon;
            }
            return weaponDictionary;
        }
    }
}