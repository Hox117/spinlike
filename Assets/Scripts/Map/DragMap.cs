using UnityEngine;
using UnityEngine.EventSystems;

public class DragMap : MonoBehaviour,
    
    IDragHandler
{
    private RectTransform rect;

    private float minY;
    private float maxY;

    private IMapService _mapService;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        _mapService = AppContainer.Get<IMapService>();
        CalcularLimites();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_mapService != null)
        {
            if (_mapService.GetMoving()) return;
            Vector2 pos = rect.anchoredPosition;

            pos.y += eventData.delta.y;

            pos.y = Mathf.Clamp(
                pos.y,
                minY,
                maxY
            );

            rect.anchoredPosition = pos;
        }
    }

    public void CalcularLimites()
    {
        float hijoMasAlto = 0;
        float hijoMasBajo = 0;

        foreach (RectTransform child in rect)
        {
            hijoMasAlto =
                Mathf.Max(
                    hijoMasAlto,
                    child.anchoredPosition.y
                );

            hijoMasBajo =
                Mathf.Min(
                    hijoMasBajo,
                    child.anchoredPosition.y
                );
        }

        minY = -hijoMasAlto;
        maxY = -hijoMasBajo;
    }
}