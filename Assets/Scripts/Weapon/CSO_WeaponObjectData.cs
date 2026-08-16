using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB_WeaponObjectData", menuName = "ScriptableObjects/Weapon/CSO_WeaponObjectData")]
public class CSO_WeaponObjectData : ScriptableObject
{
    [System.Serializable]
    public class WeaponObjectDataBase
    {
        [Header("•Ší‚Ì–¼‘O")] public string weaponName;
        [Header("•Ší‚ÌPrefab")] public GameObject weaponPrefab;
    }

    [SerializeField] private WeaponObjectDataBase[] _weaponObjectList; // Inspector‚Åİ’è‚·‚é‚½‚ß‚Ì•Ï”
    private Dictionary<string, WeaponObjectDataBase> _weaponObjectDictionary; // Às‚ÉDictionary‚É•ÏŠ·‚·‚é‚½‚ß‚Ì•Ï”

    // “Ç‚İæ‚èê—p•Ï”
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

}
