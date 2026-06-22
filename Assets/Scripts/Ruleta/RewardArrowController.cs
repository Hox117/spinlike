using UnityEngine;

public class RewardArrowController : MonoBehaviour
{
    IRouletteService rouletteService;
    IEventService eventService;
    [SerializeField]LayerMask layerMask;
    void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();
        eventService = AppContainer.Get<IEventService>();
        eventService.Subscribe<StopWheelEvent>(checkPiece);

    }
    void checkPiece(GameEventBase e) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.7f, layerMask);
        Ficha ficha = hit.collider.GetComponent<SegmentController>().getFicha();
        if (hit)
        {
            if (hit.collider.GetComponent<SegmentController>() != null)
            {
                hit.collider.GetComponent<SegmentController>().onReward(ficha);
            }
        }
    }
}
