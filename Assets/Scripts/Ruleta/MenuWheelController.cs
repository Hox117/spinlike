using UnityEngine;

public class MenuWheelController : MonoBehaviour
{
    IRouletteService rouletteService;
    [SerializeField] private int Speed;
    void Start()
    {

        rouletteService = AppContainer.Get<IRouletteService>();
        
    }
    public void slowRulette()
    {
        rouletteService.ChangeSpeed(Speed);
    }
}
