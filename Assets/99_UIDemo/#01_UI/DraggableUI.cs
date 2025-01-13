using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] RectTransform clickArea;

    private RectTransform rectTransform;
    private Canvas        canvas;

    private Vector2 offset;
    private bool    isDragging = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas        = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        OnDragging();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 특정 부분을 잡고 드래그 해야 할 때
        if(clickArea != null)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(clickArea, eventData.position, eventData.pressEventCamera))
            {
                isDragging = true;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
            }
        }
        // UI 어디든 잡고 드래그가 되어도 될 때
        else
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera))
            {
                isDragging = true;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
            }
        }

        // UI를 최상단으로
        transform.SetAsLastSibling();
    }

    void OnDragging()
    {
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out Vector2 localPoint))
            {
                rectTransform.localPosition = localPoint - offset;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
