using UnityEngine;

public class PrepareForCombatButton : MonoBehaviour
{
    IInventoryService inventoryService;
    void Start()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
    }
    
    public void startRun()
    {
        inventoryService.removeAllFicha();
    }
}
