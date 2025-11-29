using UnityEngine;
using UnityEngine.EventSystems;

public class UIMover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Settings")]
    public RectTransform uiElement;
    [Header("Animation")]
    public float slideSpeed = 5f;
    private bool isHovered = false;
    [Header("Positions")]
    public Vector2 hiddenPosition;
    public Vector2 shownPosition;

    void Start()
    {
        uiElement = GetComponent<RectTransform>();
        uiElement.anchoredPosition = hiddenPosition;
    }


    void Update()
    {
        Vector2 target = isHovered ? shownPosition : hiddenPosition;
        uiElement.anchoredPosition = Vector2.Lerp(uiElement.anchoredPosition, target, Time.deltaTime * slideSpeed);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // Triggered when the cursor leaves the UI area
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
