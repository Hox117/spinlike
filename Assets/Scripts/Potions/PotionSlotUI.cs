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
    private IAudioService audioService;
    private IRouletteService rouletteService;
    private IEventService eventService;
    private IBuffService BuffSer;
    private IEnemyService enemyService;
    private void Start()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        characterService = AppContainer.Get<ICharacterService>();
        audioService = AppContainer.Get<IAudioService>();
        rouletteService = AppContainer.Get<IRouletteService>();
        eventService = AppContainer.Get<IEventService>();
        eventService.Subscribe<PotionChangeEvent>(Refresh);
        enemyService = AppContainer.Get<IEnemyService>();
        BuffSer = AppContainer.Get<IBuffService>();  
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
        foreach (Action action in potion.actions) {
            switch (action.type)
            {
                case ActionTypes.Heal:
                    characterService.heal(action.value);
                    break;

                case ActionTypes.defense:
                    characterService.addShield(action.value);
                    break;
                case ActionTypes.SlowRulette:
                    rouletteService.ChangeSpeed((int)Math.Floor(rouletteService.GetSpeed()*0.5f));
                    break;
                case ActionTypes.BuffAttack:
                    Buff buffatq = new Buff(BuffType.attack, action.value, action.duration,characterService.getGuid() );
                    BuffSer.AddBuff(buffatq);  
                    break;
                case ActionTypes.BuffDefense:
                    Buff buffdef = new Buff(BuffType.defense, action.value, action.duration, characterService.getGuid());
                    BuffSer.AddBuff(buffdef);
                    break;

                case ActionTypes.attack:
                    enemyService.getFirstEnemy().OnHit(action.value);
                    break;
                default:
                    Debug.Log("pocion aun en desarrollo");
                    break;
            }
        }
        

    }

    private void OnDestroy()
    {
        eventService.Unsubscribe<PotionChangeEvent>(Refresh);
    }
}