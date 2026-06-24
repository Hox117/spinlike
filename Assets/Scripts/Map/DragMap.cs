using UnityEngine;
using UnityEngine.EventSystems;

public class DragMap : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform mapa;

    private float minY;
    private float maxY;

    private IMapService mapService;

    void Start()
    {
        mapService =
            AppContainer.Get<IMapService>();

        CalcularLimites();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (
            mapService != null &&
            mapService.GetMoving()
        )
            return;

        Vector2 pos =
            mapa.anchoredPosition;

        pos.y += eventData.delta.y;

        pos.y =
            Mathf.Clamp(
                pos.y,
                minY,
                maxY
            );

        mapa.anchoredPosition =
            pos;
    }

    public void CalcularLimites()
    {
        float hijoMasAlto = float.MinValue;
        float hijoMasBajo = float.MaxValue;

        foreach (RectTransform child in mapa)
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

        Debug.Log($"Limites: {minY} / {maxY}");
    }
}