using System;
using Unity.VisualScripting;
using UnityEngine;
public class SegmentController : MonoBehaviour ,ISelectionable, IRewardable
{
    [SerializeField] private PotionData[] potionList;
    private Ficha ficha;
    private bool _isSelected = true;
    private IEnemyService enemyService;
    private ICharacterService characterService;
    private ISceneService sceneService;
    private IAudioService audioService;
    private ITurnService turnService;
    private IInventoryService inventoryService;
    private IEventService eventService;
    void Awake()
    {

        enemyService = AppContainer.Get<IEnemyService>();
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();
        audioService = AppContainer.Get<IAudioService>();
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        eventService = AppContainer.Get<IEventService>();


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
                        if (action.value >= 0)
                            enemyService.getFirstEnemy().OnHit(action.value );
                        else
                            characterService.takeDamage(Math.Abs(action.value ) );
                        break;
                    case ActionTypes.defense:
                        characterService.addShield(action.value );
                        break;
                    case ActionTypes.BuffAttack:
                        break;
                    case ActionTypes.BuffDefense:
                        break;
                    case ActionTypes.debuff:
                        characterService.takeDamage(-action.value );
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


            foreach (Action action in ficha.actions)
            {
                switch (action.type)
                {
                    case ActionTypes.attack:
                        inventoryService.AddFicha(ficha);
                        break;
                    case ActionTypes.defense:
                        inventoryService.AddFicha(ficha);
                        break;
                    case ActionTypes.debuff:
                        inventoryService.AddFicha(ficha);
                        break;
                    case ActionTypes.Heal:
                        GenerarPocion();
                        break;
                }
            }
        }
    }

    public void GenerarPocion()
    {
        if (inventoryService.IsPotionsFull())
        {
            Debug.Log("El inventario de pociones est� lleno");
            return;
        }

        if (potionList == null || potionList.Length == 0)
        {
            Debug.LogWarning("No hay pociones configuradas");
            return;
        }

        PotionData randomPotion = potionList[UnityEngine.Random.Range(0, potionList.Length)];

        inventoryService.AddPotion(randomPotion);

        Debug.Log($"Se ha a�adido la poci�n: {randomPotion.Name}");
        eventService.Publish(new PotionChangeEvent());

    }
}
