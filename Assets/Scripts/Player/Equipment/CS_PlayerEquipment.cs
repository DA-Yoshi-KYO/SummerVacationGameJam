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
    [Tooltip("装備した武器の位置リスト")]
    private List<Transform> _equipmentWeaponPositionList;

    [Header("===== デバッグ用 =====")]
    [SerializeField]
    [Tooltip("装備させる武器名")]
    private string _debugWeaponName;


    private void Start()
    {
        // 初期装備の武器を登録
        foreach (Transform equipmentPosition in _equipmentWeaponPositionList)
        {
            if (equipmentPosition == null)continue;

            if (equipmentPosition.childCount <= 0) continue;

            GameObject weaponObject = equipmentPosition.GetChild(0).gameObject;
            CS_BaseWeapon weaponScript = weaponObject.GetComponent<CS_BaseWeapon>();

            if (weaponScript == null) continue;

            if (_equipmentWeaponScriptList == null)
            {
                _equipmentWeaponScriptList = new List<CS_BaseWeapon>();
            }

            _equipmentWeaponScriptList.Add(weaponScript);
        }
    }

    public void RegisterWeapon(GameObject weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("武器がnullです。");
            return;
        }

        // 空いている装備位置を探す
        int weaponIndex = -1;

        for (int i = 0; i < _equipmentWeaponPositionList.Count; i++)
        {
            if (_equipmentWeaponPositionList[i].childCount == 0)
            {
                weaponIndex = i;
                break;
            }
        }
        if (weaponIndex == -1)
        {
            Debug.LogWarning("装備位置が空いていません。");
            return;
        }

        if (_equipmentWeaponScriptList == null)
        {
            _equipmentWeaponScriptList = new List<CS_BaseWeapon>();
        }

        if (_equipmentWeaponScriptList.Count >= _equipmentWeaponPositionList.Count)
        {
            return;
        }

        // 武器を生成
        GameObject weaponObj = Instantiate(weapon);

        // ======================
        // 武器のスクリプトを取得
        // ======================

        CS_BaseWeapon weaponScript = weaponObj.GetComponent<CS_BaseWeapon>();
        if (weaponScript == null)
        {
            Debug.LogWarning("武器にCS_BaseWeaponスクリプトがアタッチされていません。");
            return;
        }

        // 武器のスクリプト情報をリストに追加
        _equipmentWeaponScriptList.Add(weaponScript);

        // ======================
        // 武器の位置を設定
        // ======================

        // 武器の位置を設定
        weaponObj.transform.SetParent(_equipmentWeaponPositionList[weaponIndex]);

        // 武器の位置と回転をリセット
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;
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

    [ContextMenu("ランダム武器装備")]
    private void RandomWeponEquipment()
    {
        // ランダムに武器を選択
        int randomIndex = Random.Range(0, _weaponObjectData.weaponObjectDatas.Count);
        string selectedWeaponName = _weaponObjectData.weaponNames[randomIndex];

        // 選択された武器のPrefabを取得
        GameObject selectedWeapon = _weaponObjectData.weaponObjectDatas[selectedWeaponName].weaponPrefab;

        // 武器を装備
        RegisterWeapon(selectedWeapon);
    }

    [ContextMenu("デバッグ用：指定武器装備")]
    private void DebugWeaponEquipment()
    {
        if (string.IsNullOrEmpty(_debugWeaponName))
        {
            Debug.LogWarning("デバッグ用の武器名が設定されていません。");
            return;
        }
        // デバッグ用の武器名からPrefabを取得
        if (!_weaponObjectData.weaponObjectDatas.ContainsKey(_debugWeaponName))
        {
            Debug.LogWarning($"デバッグ用の武器名 '{_debugWeaponName}' は武器データに存在しません。");
            return;
        }
        GameObject debugWeaponPrefab = _weaponObjectData.weaponObjectDatas[_debugWeaponName].weaponPrefab;
        // 武器を装備
        RegisterWeapon(debugWeaponPrefab);
    }
}