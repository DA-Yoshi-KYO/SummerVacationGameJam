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

    private void Start()
    {
        CS_ValueObserver.Instance.Register(gameObject, this, "武器の登録数：", ()=>_weaponObjectData.weaponObjectDatas.Count);
        CS_ValueObserver.Instance.Register(gameObject, this, "武器の名前登録数：", () => _weaponObjectData.weaponNames.Count);
    }

    public void RegisterWeapon(GameObject weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("武器がnullです。");
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

        int weaponIndex = _equipmentWeaponScriptList.Count - 1;

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
}