using UnityEngine;

public class MapInitializer : MonoBehaviour
{
    private ITurnService turnService;
    void Awake()
    {
        turnService = AppContainer.Get<ITurnService>();
        if (!turnService.IsPlayerTurn()) turnService.ChangeTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
