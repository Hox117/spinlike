using UnityEditor.MPE;
using UnityEngine;

public class BotonGenerarPocion : MonoBehaviour
{
   private PotionData[] potionList;

    private IInventoryService _inventoryService;
    private IEventService _eventService;
    private void Start()
    {
        _inventoryService = AppContainer.Get<IInventoryService>();
        _eventService = AppContainer.Get<IEventService>();
    }

    public void GenerarPocion()
    {
        if (_inventoryService.IsPotionsFull())
        {
            Debug.Log("El inventario de pociones esta lleno");
            return;
        }
        potionList = Resources.LoadAll<PotionData>("Objects/Potions");
        if (potionList == null || potionList.Length == 0)
        {
            Debug.LogWarning("No hay pociones configuradas");
            return;
        }

        PotionData randomPotion = potionList[UnityEngine.Random.Range(0, potionList.Length)];

        _inventoryService.AddPotion(randomPotion);

        Debug.Log($"Se ha añadido la poción: {randomPotion.Name}");
        _eventService.Publish(new PotionChangeEvent());

    }
}