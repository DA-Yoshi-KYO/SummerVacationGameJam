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
    public class WeaponList
    {
        [Header("武器の名前")] public string weaponName;
        [Header("ダメージ")] public float damage;
        [Header("連射速度")] public float fireRate;
        [Header("弾速")] public float bulletSpeed;
        [Header("弾数")] public int bulletCount;
        [Header("リロード時間")] public float reloadTime;
        [Header("弾のPrefab")] public GameObject bulletPrefab;
        [Header("敵がいない場合の直進距離")] public float distanceForward;
    }

    public List<WeaponList> weapons;//武器のリスト
}

