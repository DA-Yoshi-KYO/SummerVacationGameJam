using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    [Tooltip("初期装備の武器のリスト")]
    private List<GameObject> _firstSelectWeaponList;

    [SerializeField]
    [Tooltip("武器のプレハブデータ")]
    private CSO_WeaponObjectData _weaponObjectData;

    [Tooltip("装備している武器のスクリプトのリスト")]
    private List<CS_BaseWeapon> _equipmentWeaponScriptList;

    [SerializeField]
    [Tooltip("持てる武器の最大数")]
    private int _maxWeaponCount = 6;


    public void RegisterWeapon(GameObject weapon)
    {
        if (weapon == null){
            Debug.LogWarning("武器がnullです。");
            return;
        }

        if (_equipmentWeaponScriptList == null){
            _equipmentWeaponScriptList = new List<CS_BaseWeapon>();
        }

        if (_equipmentWeaponScriptList.Count >= _maxWeaponCount){
            return;
        }

        // 武器のスクリプトを取得
        CS_BaseWeapon weaponScript = weapon.GetComponent<CS_BaseWeapon>();
        if (weaponScript == null){
            Debug.LogWarning("武器にCS_BaseWeaponスクリプトがアタッチされていません。");
            return;
        }

        _equipmentWeaponScriptList.Add(weaponScript);
    }

    private void Update()
    {
        if (_equipmentWeaponScriptList == null || _equipmentWeaponScriptList.Count == 0)
        {
            return;
        }

        // プレイヤーの入力を取得
        bool isShootingKeyInput = CS_InputManager.readInstance.customInputSystem.Player.Shot.IsPressed();

        foreach (var weapon in _equipmentWeaponScriptList)
        {
            // 武器の発射処理を呼び出す
            weapon.SetShooting(isShootingKeyInput);
        }
    }
}