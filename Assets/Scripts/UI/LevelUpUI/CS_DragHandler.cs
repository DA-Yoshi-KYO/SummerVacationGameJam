using UnityEngine;
using UnityEngine.EventSystems;

public class CS_DragHandler : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform draggableArea;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 originalPosition;
    private bool canDrag = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == draggableArea.gameObject ||
            eventData.pointerEnter.transform.IsChildOf(draggableArea))
        {
            canDrag = true;

            originalPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.anchoredPosition = originalPosition;
        canDrag = false;
    }
}
