using UnityEngine;

public class ArrowController : MonoBehaviour
{
    protected IRouletteService rouletteService;
    protected IEventService eventService;
    [SerializeField] protected LayerMask layerMask;
    protected virtual void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();
        eventService = AppContainer.Get<IEventService>();
        eventService.Subscribe<StopWheelEvent>(checkPiece);

    }
    void OnDestroy()
    {
        eventService.Unsubscribe<StopWheelEvent>(checkPiece);
    }
    protected virtual void checkPiece(GameEventBase e) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.7f, layerMask);
        if (hit)
        {
            if (hit.collider.GetComponent<SegmentController>() != null)
            {
                hit.collider.GetComponent<SegmentController>().OnSelected();
            }
        }
    }
}
