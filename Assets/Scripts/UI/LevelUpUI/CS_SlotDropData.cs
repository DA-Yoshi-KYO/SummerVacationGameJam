/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   ドラッグとドロップで武器をスロットに入れる処理
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_SlotDropData : MonoBehaviour, IDropHandler
{
    [Header("スロットの情報")][SerializeField] private CS_WeaponSet mySlot;

    //ドロップされたときに呼ばれる処理
    public void OnDrop(PointerEventData eventData)
    {
        var data = eventData.pointerDrag.GetComponentInParent<CS_SelectUISet>();
        if (data == null) return;

        //スロットが空なら入れる
        if (mySlot.currentWeapon == null)
        {
            mySlot.SetUI(data);
            return;
        }

        //スロットに武器が入っている場合
        string slotWeaponName = mySlot.currentWeapon.GetData().weapon.weaponName;
        string dragWeaponName = data.GetData().weapon.weaponName;

        //同じ武器ならレベルアップ
        if (slotWeaponName == dragWeaponName)
        {
            mySlot.weaponLevelUpUI.LevelUp();
            return;
        }
    }
}