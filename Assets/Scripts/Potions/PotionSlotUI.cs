using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlotUI : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private TextMeshProUGUI text;

    private IInventoryService inventoryService;
    private ICharacterService characterService;

    private void Start()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        characterService = AppContainer.Get<ICharacterService>();

        Refresh();
    }

    public void Refresh()
    {
        PotionData potion = inventoryService.GetPotion(slotIndex);

        //if (potion == null)
        //{
        //    icon.enabled = false;
        //    return;
        //}

        //icon.enabled = true;
        //icon.sprite = potion.Sprite;
        text.text = potion.name;
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
        }

    }
}