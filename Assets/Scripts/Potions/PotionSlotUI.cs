using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlotUI : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private Image icon;

    private IInventoryService inventoryService;
    private ICharacterService characterService;
    private IEventService eventService;
    private void Start()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        characterService = AppContainer.Get<ICharacterService>();
        eventService= AppContainer.Get<IEventService>();
        eventService.Subscribe<PotionChangeEvent>(Refresh);
        Refresh();
    }

    public void Refresh(GameEventBase e=null)
    {
        PotionData potion = inventoryService.GetPotion(slotIndex);

        if (potion == null)
        {
            icon.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = potion.Sprite;
         
    }

    public void OnClick()
    {
        PotionData potion = inventoryService.GetPotion(slotIndex);

        if (potion == null)
            return;

        UsePotion(potion);

        inventoryService.RemovePotion(slotIndex);

        Refresh();
    }

    private void UsePotion(PotionData potion)
    {
        //TODO:añadir todos los efectos
        switch (potion.effect)
        {
            case PotionEffect.Heal:
                characterService.heal((int)Math.Round(potion.value));
                break;

            case PotionEffect.Shield:
                characterService.addShield((int)Math.Round(potion.value));
                break;
            default:
                Debug.Log("pocion aun en desarrollo");
                break;
        }

    }
}