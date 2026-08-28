/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   スロットの見た目を管理する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CS_SlotVisual : MonoBehaviour
{
    [Header("フレームの画像")][SerializeField] private Image frameImage;

    [Header("アイテムがセットされていないときのフレーム画像")][SerializeField] private Sprite unSetItemFrameSprite;
    [Header("アイテムがセットされているときのフレーム画像")][SerializeField] private Sprite setItemFrameSprite;
    [Header("アイテムがアップグレードされるときの画像")][SerializeField] private Sprite upGradeItemFrameSprite;

    //ステータス
    public enum SlotState
    {
        UnSetItem,//アイテムなし
        SetItem,//アイテムあり
        Hovered,//ドラッグ中に上に乗ってる
    }

    private Coroutine hoverAnim;//点滅アニメーションのコルーチン
    private SlotState currentState = SlotState.UnSetItem;//現在のステータス
    public bool hasItem = false;//アイテムを持っているかどうか

    //スロットの状態を変更する
    public void SetState(SlotState state)
    {
        currentState = state;

        //すでに点滅アニメーションが動いていたら止める
        if (hoverAnim != null)
        {
            StopCoroutine(hoverAnim);
            hoverAnim = null;
        }

        switch (state)
        {
            case SlotState.UnSetItem:
                frameImage.sprite = unSetItemFrameSprite;
                frameImage.color = Color.white;
                break;

            case SlotState.SetItem:
                frameImage.sprite = setItemFrameSprite;
                frameImage.color = Color.white;
                break;

            case SlotState.Hovered:
                //点滅開始
                hoverAnim = StartCoroutine(HoverAnimation());
                break;
        }
    }

    //ホバー中の点滅アニメーション
    private IEnumerator HoverAnimation()
    {
        float t = 0f;

        while (true)
        {
            t += Time.unscaledDeltaTime;

            float ping = Mathf.PingPong(t, 1f);


            if (hasItem)
            {
                //アイテムがあった場合
                frameImage.sprite = (ping < 0.5f) ? setItemFrameSprite : upGradeItemFrameSprite;
            }
            else
            {
                //アイテムがなかった場合
                frameImage.sprite = (ping < 0.5f) ? setItemFrameSprite : unSetItemFrameSprite;
            }

            yield return null;
        }
    }
}
