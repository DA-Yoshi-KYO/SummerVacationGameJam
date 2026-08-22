using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CS_SlotDropData : MonoBehaviour, IDropHandler
{
    //[SerializeField] private int slotIndex;

    [SerializeField] private CS_WeaponSet mySlot;

    public void OnDrop(PointerEventData eventData)
    {
        var data = eventData.pointerDrag.GetComponentInParent<CS_SelectUISet>();
        if (data == null) return;

        // ★ 武器が入っていない場合（null）
        if (mySlot.currentWeapon == null)
        {
            // 新しくセットする
            mySlot.SetUI(data);
            Debug.Log(data);
            return;
        }

        // ★ 武器が入っている場合 → 比較する
        if (mySlot.currentWeapon.GetData().weapon.weaponName == data.GetData().weapon.weaponName)
        {
            // 同じ武器 → レベルアップ
            mySlot.weaponLevelUpUI.LevelUp();
            return;
        }

        // ★ 違う武器 → 入れ替え
        mySlot.SetUI(data);

        // 必要ならレベルリセット
        // mySlot.upGradeUI.ResetLevel();
    }

    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.Log($"OnDrop 呼ばれた（slot {slotIndex}）");

    //    if (eventData == null)
    //    {
    //        Debug.Log("eventData が null");
    //        return;
    //    }

    //    if (eventData.pointerDrag == null)
    //    {
    //        Debug.Log("pointerDrag が null");
    //        return;
    //    }

    //    // ★ ここを変更：親から CS_SelectUISet を探す
    //    var data = eventData.pointerDrag.GetComponentInParent<CS_SelectUISet>();
    //    if (data == null)
    //    {
    //        Debug.Log("親をたどっても CS_SelectUISet が見つからない");
    //        return;
    //    }

    //    Debug.Log($"Slot {slotIndex} にドロップされた → {data.name}");

    //    CS_LevelUpManager.Instance.SetWeapon(slotIndex, data);
    //}
}