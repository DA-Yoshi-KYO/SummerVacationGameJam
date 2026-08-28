/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    UIの武器スロットの管理クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-17 | 初回作成
 */
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerWeaponSlotUIManager : MonoBehaviour
{
    [Header("UI武器データベース")][SerializeField] private CSO_WeaponDataBase weaponDataBase;

    [Header("武器スロット配列")][SerializeField] private CS_PlayerWeaponSlotUI[] slotArray;
    private Dictionary<string, List<CS_PlayerWeaponSlotUI>> slots = new Dictionary<string, List<CS_PlayerWeaponSlotUI>>();

    private int nextSlotIndex = 0;//次に武器をセットするスロットのインデックス

    void Start()
    {
        if (weaponDataBase == null)
        {
            Debug.LogError("UIWeaponDataBase が設定されていません");
            return;
        }

        //全部空にする
        for (int i = 0; i < slotArray.Length; i++)
        {
            slotArray[i].ClearSlot();
        }
    }

    void Update()
    {
    }

    //武器を追加
    public void AddWeapon(string weaponName)
    {
        if (nextSlotIndex >= slotArray.Length) return;

        var data = weaponDataBase.weaponDatas[weaponName];

        var slot = slotArray[nextSlotIndex];
        slot.SetupWeapon(data);

        if (!slots.ContainsKey(weaponName))
        {
            slots[weaponName] = new List<CS_PlayerWeaponSlotUI>();
        }

        slots[weaponName].Add(slot);

        nextSlotIndex++;
    }

    //同じ武器を全部発射
    public void UseBulletsAll(string weaponName, int amount)
    {
        if (!slots.ContainsKey(weaponName)) return;

        foreach (var slot in slots[weaponName])
        {
            slot.UseBullets(amount);
        }
    }

    //同じ武器を全部補充
    public void AddBulletsAll(string weaponName, int amount)
    {
        if (!slots.ContainsKey(weaponName)) return;

        foreach (var slot in slots[weaponName])
        {
            slot.AddBullets(amount);
        }
    }
}
