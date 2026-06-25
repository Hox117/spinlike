using System;
using Unity.VisualScripting;
using UnityEngine;
public class SegmentController : MonoBehaviour ,ISelectionable, IRewardable
{
    private PotionData[] potionList;
    private Ficha ficha;
    private bool _isSelected = true;
    private IEnemyService enemyService;
    private ICharacterService characterService;
    private ISceneService sceneService;
    private IAudioService audioService;
    private ITurnService turnService;
    private IInventoryService inventoryService;
    private IEventService eventService;
    private IBuffService buffService;
    void Awake()
    {

        enemyService = AppContainer.Get<IEnemyService>();
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();
        audioService = AppContainer.Get<IAudioService>();
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        eventService = AppContainer.Get<IEventService>();
        buffService = AppContainer.Get<IBuffService>();

        potionList = Resources.LoadAll<PotionData>("Objects/Potions");
    }

    public void OnSelected()
    {
        //Esto se regenera al volver a lanzar por lo que vuelve a ser true al regenerarse
        if (_isSelected)
        {
            audioService.PlaySound(ficha.audioClip);
            _isSelected = false;


            foreach (Action action in ficha.actions)
            {
                switch (action.type)
                {
                    case ActionTypes.attack:
                        if (action.value >= 0) {
                            int attackBuffValue = 0;
                            Buff attackBuff = buffService.GetBuff(characterService.getGuid(), BuffType.attack);
                            if (attackBuff != null) {
                                attackBuffValue = attackBuff.value;
                            }
                            enemyService.getFirstEnemy().OnHit(action.value + attackBuffValue);
                            eventService.Publish(new PlayerAttackEvent());
                        }
                        else
                            characterService.takeDamage(Math.Abs(action.value ) );
                        break;
                    case ActionTypes.defense:
                        int defenseBuffValue = 0;
                        Buff defenseBuff = buffService.GetBuff(characterService.getGuid(), BuffType.defense);
                        if (defenseBuff != null) {
                            defenseBuffValue = defenseBuff.value;
                        }
                        characterService.addShield(action.value );
                        break;
                    case ActionTypes.BuffAttack:
                        buffService.AddBuff(new Buff(BuffType.attack, action.value, action.duration, characterService.getGuid()));
                        break;
                    case ActionTypes.BuffDefense:
                        buffService.AddBuff(new Buff(BuffType.defense, action.value, action.duration, characterService.getGuid()));
                        break;
                    case ActionTypes.debuff:
                        characterService.takeDamage(-action.value );
                        break;
                    case ActionTypes.Heal:
                        characterService.heal(action.value);
                        break;
                    case ActionTypes.menu:
                
                        sceneService.LoadScene((SceneNames) action.value );
                        break;
                }
            }


            

            
            turnService.ChangeTurn();
        }
        
    }
    public Ficha getFicha()
    {
        return ficha;
    }
    public void addFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }

    public void onReward(Ficha ficha)
    {
        if (_isSelected)
        {
            audioService.PlaySound(ficha.audioClip);
            _isSelected = false;

            switch (ficha.actions[0].type)
            {
                case ActionTypes.Heal:
                    GenerarPocion();
                    break;
                default:
                    inventoryService.AddFicha(ficha);
                    break;
            }
        }
    }

    public void GenerarPocion()
    {
        if (inventoryService.IsPotionsFull())
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

        inventoryService.AddPotion(randomPotion);

        Debug.Log($"Se ha añadido la poción: {randomPotion.Name}");
        eventService.Publish(new PotionChangeEvent());

    }
}
