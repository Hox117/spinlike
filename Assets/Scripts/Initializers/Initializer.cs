using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    IInventoryService inventoryService;
    ITurnService turnService;
    [SerializeField] List<FichaData> ListaDeFichas;

    public void setListaDeFichas(List<FichaData> ListaDeFichas)
    {
        this.ListaDeFichas = ListaDeFichas;
    }
    void Awake()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        if (inventoryService.getListaFichas().Count < 1) inventoryService.cargarInventario(ListaDeFichas);
        if (!turnService.IsPlayerTurn()) turnService.ChangeTurn();

    }
    void Start()
    {


        
    }
}
