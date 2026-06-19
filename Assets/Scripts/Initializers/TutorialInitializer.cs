using System.Collections.Generic;
using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    IInventoryService inventoryService;
    [SerializeField] List<Ficha> ListaDeFichas;
    void Start()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        inventoryService.cargarInventario(ListaDeFichas);
    }
}
