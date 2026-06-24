using UnityEngine;

public class ArrowMapController : ArrowController
{
    protected override void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();
        eventService = AppContainer.Get<IEventService>();
        eventService.Subscribe<StopMapWheelEvent>(checkPiece);

    }
    protected override void checkPiece(GameEventBase e)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.7f, layerMask);
        if (hit)
        {
            if (hit.collider.GetComponent<SegmentController>() != null)
            {
                RouletterMapTileSelectedEvent evento = new RouletterMapTileSelectedEvent(hit.collider.transform.GetSiblingIndex());
                eventService.Publish(evento);
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {

        eventService.Unsubscribe<StopMapWheelEvent>(checkPiece);
    }
}
