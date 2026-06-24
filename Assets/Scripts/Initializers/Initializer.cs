using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    IInventoryService inventoryService;
    ITurnService turnService;
    IEventService eventService;
    [SerializeField] List<FichaData> ListaDeFichas;
    [SerializeField] Wheel_Manager ruleta;
    public void setListaDeFichas(List<FichaData> ListaDeFichas)
    {
        this.ListaDeFichas = ListaDeFichas;
    }
    void Awake()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        eventService = AppContainer.Get<IEventService>();

        if (inventoryService.getListaFichas().Count < 1) inventoryService.cargarInventario(ListaDeFichas);
        if (!turnService.IsPlayerTurn()) turnService.ChangeTurn();

        eventService.Subscribe<TurnChangeEvent>(AleatorizarRuleta);
        


    }

    void AleatorizarRuleta(GameEventBase e)
    {
        if (turnService.IsPlayerTurn())
        {
            ruleta.GenerateRoulette();
        }
    }
}
