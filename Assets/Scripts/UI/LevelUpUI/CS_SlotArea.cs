/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   スロットの範囲にカーソルが入ったか確認する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_SlotArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]public bool isHover = false;//ホバー状態かどうか

    //マウスカーソルがこのUIの範囲に入った瞬間に呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    //マウスカーソルがこのUIの範囲から離れた瞬間に呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
