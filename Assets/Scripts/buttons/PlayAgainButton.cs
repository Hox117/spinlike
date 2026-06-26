using System.Collections.Generic;
using UnityEngine;

public class PlayAgainButton : MonoBehaviour
{
    ICharacterService characterService;
    IMapService mapService;
    ISceneService sceneService;
    IBuffService buffService;
    IInventoryService inventoryService;
    ITurnService turnService;
    IEnemyService enemyService;
    List<PotionData> potionList;
    void Start()
    {
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();
        mapService = AppContainer.Get<IMapService>();
        buffService = AppContainer.Get<IBuffService>();
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        enemyService = AppContainer.Get<IEnemyService>();
    }
    public void PlayAgain()
    {
        characterService.resetPlayer();
        mapService.ResetMap();
        mapService.SetPositionPlayer(1, 1);
        turnService.resetTurn();
        buffService.ClearBuffList();
        inventoryService.removeAllFicha();
        enemyService.setEnemyList(new List<GameObject>());
       

        for (int i = 0; i < 3; i++) 
        {   
            if (inventoryService.GetPotion(i)!= null)
            {
                inventoryService.RemovePotion(i);
            }
        }
        potionList = new List<PotionData>(Resources.LoadAll<PotionData>("Objects/Potions"));
        if (potionList == null || potionList.Count == 0)
        {
            Debug.LogWarning("No hay pociones configuradas");
            return;
        }

        PotionData randomPotion = potionList[Random.Range(0, potionList.Count)];
        inventoryService.AddPotion(randomPotion);
        sceneService.LoadScene(SceneNames.Map);
    }



}
