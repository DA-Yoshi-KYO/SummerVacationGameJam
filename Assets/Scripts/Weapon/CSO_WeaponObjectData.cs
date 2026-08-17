using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB_WeaponObjectData", menuName = "ScriptableObjects/Weapon/CSO_WeaponObjectData")]
public class CSO_WeaponObjectData : ScriptableObject
{
    [System.Serializable]
    public class WeaponObjectDataBase
    {
        [Header("武器の名前")] public string weaponName;
        [Header("武器のPrefab")] public GameObject weaponPrefab;
    }

    [SerializeField] private WeaponObjectDataBase[] _weaponObjectList; // Inspectorで設定するための変数
    private Dictionary<string, WeaponObjectDataBase> _weaponObjectDictionary; // 実行時にDictionaryに変換するための変数
    private List<string> _weaponNameList; // 武器名のリスト

    // 読み取り専用変数
    public IReadOnlyDictionary<string, WeaponObjectDataBase> weaponObjectDatas
    {
        get
        {
            if (_weaponObjectDictionary == null)
            {
                _weaponObjectDictionary = new Dictionary<string, WeaponObjectDataBase>();
                foreach (var weapon in _weaponObjectList)
                    _weaponObjectDictionary[weapon.weaponName] = weapon;
            }
            return _weaponObjectDictionary;
        }
    }

    public IReadOnlyList<string> weaponNames
    {
        get
        {
            if (_weaponNameList == null)
            {
                _weaponNameList = new List<string>();
                foreach (var weapon in _weaponObjectList)
                    _weaponNameList.Add(weapon.weaponName);
            }
            return _weaponNameList;
        }
    }
}
