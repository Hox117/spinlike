using UnityEngine;

public class ArrowController : MonoBehaviour
{
    IRouletteService rouletteService;
    [SerializeField]LayerMask layerMask;
    void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();
        
    }
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector2.right*0.7f, Color.red, 1);
        if (rouletteService.GetRouletteStatus()) return; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right , 0.7f, layerMask);
        if (hit)
        {
            if (hit.collider.GetComponent<SegmentController>() != null)
            {
                hit.collider.GetComponent<SegmentController>().OnSelected();
            }
        }
        
    }
}
