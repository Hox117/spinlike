using UnityEngine;

public class PlayAgainButton : MonoBehaviour
{
    ICharacterService characterService;
    IMapService mapService;
    ISceneService sceneService;
    IBuffService buffService;
    IInventoryService inventoryService;
    ITurnService turnService;
    void Start()
    {
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();
        mapService = AppContainer.Get<IMapService>();
        buffService = AppContainer.Get<IBuffService>();
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
    }
    public void PlayAgain()
    {
        characterService.resetPlayer();
        mapService.ResetMap();
        mapService.SetPositionPlayer(1, 1);
        sceneService.LoadScene(SceneNames.Map);
        buffService.ClearBuffList();
        inventoryService.removeAllFicha();
        if (!turnService.IsPlayerTurn())
        {
            turnService.ChangeTurn();
        }

        for (int i = 0; i < 3; i++) 
        {   
            if (inventoryService.GetPotion(i)!= null)
            {
                inventoryService.RemovePotion(i);
            }
        }
       }



}
