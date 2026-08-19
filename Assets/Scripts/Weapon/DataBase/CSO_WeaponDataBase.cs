/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　武器のデータベース
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CSO_WeaponDataBase", menuName = "Weapon/CSO_WeaponDataBase")]
public class CSO_WeaponDataBase : ScriptableObject
{
    [System.Serializable]
    public class WeaponDataBase
    {
        [Header("武器の名前")] public string weaponName;
        [Header("ダメージ")] public float damage;
        [Header("連射速度")] public float fireRate;
        [Header("弾速")] public float bulletSpeed;
        [Header("射程距離")] public float range;
        [Header("弾数")] public int bulletCount;
        [Header("リロード時間")] public float reloadTime;
        [Header("弾のPrefab")] public GameObject bulletPrefab;
    }

    [SerializeField] private List<WeaponDataBase> weaponList;//Inspectorで設定するための変数
    private Dictionary<string, WeaponDataBase> weaponDictionary;//実行時にDictionaryに変換するための変数

    //読み取り専用変数
    public IReadOnlyDictionary<string, WeaponDataBase> weaponDatas
    {
        get
        {
            if (weaponDictionary == null)
            {
                weaponDictionary = new Dictionary<string, WeaponDataBase>();
                foreach (var weapon in weaponList)
                    weaponDictionary[weapon.weaponName] = weapon;
            }
            return weaponDictionary;
        }
    }
}

