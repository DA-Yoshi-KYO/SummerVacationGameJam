/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   武器選択UIをドラッグ＆ドロップできるようにするため
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_DragHandler : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("ドラッグ可能な領域")][SerializeField] private Transform draggableArea;

    [Header("ホバー時・ドラッグ時に見た目を変えるUI")]
    [SerializeField] private CS_ChangeUITexture frameTexture;
　　[SerializeField] private CS_ChangeUITexture textBackGroundTexture;
    [SerializeField] private CS_ChangeUIText text;

    [Header("拡縮アニメーションの速度")][SerializeField] private float duration;

    [Header("拡縮の最小値")][SerializeField] private float minScale;
    [Header("拡縮の最大値")][SerializeField] private float maxScale;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 originalPosition;//元の位置
    private bool canDrag = false;//ドラッグ可能かどうか
    private bool isDragging = false;//ドラッグ中かどうか
    private bool isSelected = false;//ホバー中か

    private Coroutine scaleRoutine;//拡縮アニメーション

    private Transform root;//拡大対象の親オブジェクト

    [HideInInspector] private CS_SlotVisual currentSlot;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        root = transform.parent;//親を拡大対象にする
    }

    //ホバー開始
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            isSelected = true;

            //見た目を変更
            frameTexture.ChangeTexture(true);
            textBackGroundTexture.ChangeTexture(true);
            text.ChangeTexture(true);

            //親を拡大
            root.localScale = new Vector3(maxScale, maxScale, 1.0f);
        }
    }

    //ホバー終了
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            isSelected = false;

            //見た目を元に戻す
            frameTexture.ChangeTexture(false);
            textBackGroundTexture.ChangeTexture(false);
            text.ChangeTexture(false);

            //親を元に戻す
            root.localScale = Vector3.one;
        }
    }

    //ドラッグ開始
    public void OnBeginDrag(PointerEventData eventData)
    {
        //ドラッグ可能領域内でドラッグ開始したか判定
        if (eventData.pointerEnter == draggableArea.gameObject ||
            eventData.pointerEnter.transform.IsChildOf(draggableArea))
        {
            canDrag = true;

            //半透明化&Raycast無効化
            originalPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            isDragging = true;

            //見た目を変更
            frameTexture.SetDragTexture();
            textBackGroundTexture.SetDragTexture();
            text.SetDragTexture();

            //親を拡大
            root.localScale = new Vector3(maxScale, maxScale, 1.0f);
        }
    }

    //ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;

        //マウス位置をUIのローカル座標に変換
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        rectTransform.anchoredPosition = pos;

        //スロット上にいるか判定
        if (IsOnSlot())
        {
            StartScaleAnimation();
        }
        else
        {
            StopScaleAnimation();
        }

        //マウスの下にあるスロットを取得
        CS_SlotVisual slot = GetSlotUnderMouse();

        //スロットが変わったら前のスロットを元に戻す
        if (slot != currentSlot)
        {
            if (currentSlot != null)
            {
                currentSlot.SetState(currentSlot.hasItem ? CS_SlotVisual.SlotState.SetItem : CS_SlotVisual.SlotState.UnSetItem);
            }

            currentSlot = slot;
        }

        //今スロットの上にいるならHovered状態にする
        if (currentSlot != null)
        {
            currentSlot.SetState(CS_SlotVisual.SlotState.Hovered);
            StartScaleAnimation();
        }
        else
        {
            StopScaleAnimation();
        }
    }

    //ドラッグ終了
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;

        //見た目を元に戻す
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        //元の位置へ戻す
        rectTransform.anchoredPosition = originalPosition;
        
        canDrag = false;
        isDragging = false;
        isSelected = false;

        //見た目の変更
        frameTexture.ResetDragTexture();
        textBackGroundTexture.ResetDragTexture();
        text.ResetDragTexture();

        root.localScale = Vector3.one;

        StopScaleAnimation();

        //スロットに乗っていたら状態を変更する
        if (currentSlot != null)
        {
            currentSlot.hasItem = true;
            currentSlot.SetState(CS_SlotVisual.SlotState.SetItem);
        }

        currentSlot = null;
    }

    //拡縮アニメーション開始
    private void StartScaleAnimation()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(AnimateScale());
    }

    //拡縮アニメーション停止
    private void StopScaleAnimation()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = null;
        rectTransform.localScale = Vector3.one; // 元に戻す
    }

    //拡縮ループ
    private IEnumerator AnimateScale()
    {
        float t = 0.0f;

        while (true)
        {
            t += Time.deltaTime / duration;
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(t, 1.0f));
            rectTransform.localScale = new Vector3(scale, scale, 1.0f);

            yield return null;
        }
    }

    //マウスがスロット上にあるか判定
    private bool IsOnSlot()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        //UIの中心位置をスクリーン座標に変換
        pointerData.position = RectTransformUtility.WorldToScreenPoint(
            canvas.worldCamera,
            rectTransform.position
        );

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var r in results)
        {
            if (r.gameObject.GetComponentInParent<CS_SlotDropData>() != null)
            {
                return true;
            }
        }
        return false;
    }

    //マウスの下にあるスロットを取得
    private CS_SlotVisual GetSlotUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        //UIの中心位置をスクリーン座標に変換
        pointerData.position = RectTransformUtility.WorldToScreenPoint(
            canvas.worldCamera,
            rectTransform.position
        );

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var r in results)
        {
            if (r.gameObject.CompareTag("SlotDataUI"))
            {
                return r.gameObject.GetComponentInParent<CS_SlotVisual>();
            }
        }

        return null;
    }

}
