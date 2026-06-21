using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    IInventoryService inventoryService;
    ITurnService turnService;
    [SerializeField] List<Ficha> ListaDeFichas;

    void Awake()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        
    }
    void Start()
    {

        inventoryService.cargarInventario(ListaDeFichas);
        if(!turnService.IsPlayerTurn())turnService.ChangeTurn();
        
    }
}
