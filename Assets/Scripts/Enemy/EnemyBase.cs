using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class EnemyBase : MonoBehaviour, IHittable
{
    [SerializeField]Enemy enemyData;
    [SerializeField] Slider sliderVida;
    [SerializeField] Image ImageNextAttack;
    [SerializeField] TextMeshProUGUI ValueNextAttack;
    [SerializeField] Slider sliderEscudo;

    Slider sliderEscudoInstanciado;
    TextMeshProUGUI textoEscudoInstanciad;

    TextMeshProUGUI ValueNextAttackInstanciado;
    Image ImageNextAttackInstanciado;
    Slider sliderVidaInstanciado;


    Sprite[] sprites;

    int life;
    int shield;
    Guid guid;
    bool dead;
    Action AccionElegida;
    List<Action> PossibleActions;

    public bool isTurnEnded = false;

    Animator animator;

    ICharacterService characterService;
    IEnemyService enemyService;
    ITurnService turnService;
    IEventService eventService;
    ISceneService sceneService;
    IBuffService buffService;
    void Awake()
    {
        enemyService = AppContainer.Get<IEnemyService>();
        eventService = AppContainer.Get<IEventService>();
        turnService = AppContainer.Get<ITurnService>();
        characterService = AppContainer.Get<ICharacterService>();
        buffService = AppContainer.Get<IBuffService>();
        sceneService = AppContainer.Get<ISceneService>();
        sprites = Resources.LoadAll<Sprite>($"mapa");
    }
    void Start()
    {
        guid = Guid.NewGuid();
        life = enemyData.Life;
        shield = enemyData.Shield;
        PossibleActions = enemyData.ActionList;

        instantiateSlider();

        animator = GetComponent<Animator>();

        elegirAccion();

        eventService.Subscribe<TurnChangeEvent>(TakeAction);

    }

    private Sprite ActionImage(ActionTypes tipo)
    {

        return Resources.Load<Sprite>($"Actions/{tipo.ToString()}");

    }

    private void instantiateSlider()
    {
        var canvas = FindAnyObjectByType<Canvas>();

        RectTransform barra =
            Instantiate(
                sliderVida.GetComponent<RectTransform>(),
                canvas.transform
            );

        RectTransform barraEscudo = Instantiate(sliderEscudo.GetComponent<RectTransform>(), canvas.transform);


        RectTransform imagenAtaque =
            Instantiate(
                ImageNextAttack.GetComponent<RectTransform>(),
                canvas.transform
            );
        RectTransform ValorAtaque =
            Instantiate(
                ValueNextAttack.GetComponent<RectTransform>(),
                canvas.transform
            );
        Vector3 pantalla =
            Camera.main.WorldToScreenPoint(
                transform.position + Vector3.up
            );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            pantalla,
            null,
            out Vector2 posicionUI
        );

        barra.anchoredPosition = posicionUI;
        barraEscudo.anchoredPosition = posicionUI;

        float scale = canvas.scaleFactor;
        //AQUI GABRIEL
        imagenAtaque.anchoredPosition = new Vector2(posicionUI.x + 20f, posicionUI.y + +60f);
        ValorAtaque.anchoredPosition = new Vector2(posicionUI.x - 20f, posicionUI.y + 60f);

        sliderVidaInstanciado = barra.GetComponent<Slider>();
        sliderEscudoInstanciado = barraEscudo.GetComponent<Slider>();
        ImageNextAttackInstanciado = imagenAtaque.GetComponent<Image>();
        ValueNextAttackInstanciado = ValorAtaque.GetComponent<TextMeshProUGUI>();


        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = life.ToString();
        sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = shield.ToString();


        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30;

        sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30;


        sliderVidaInstanciado.maxValue = life;
        sliderVidaInstanciado.value = life;

        sliderVidaInstanciado.minValue = 0;

        sliderEscudoInstanciado.maxValue = 0.1f;
        sliderEscudoInstanciado.value = shield;
        sliderEscudoInstanciado.minValue = 0;

        textoEscudoInstanciad = sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>();

        RectTransform fillVida =
      sliderVidaInstanciado.fillRect;

        RectTransform fillAreaEscudo =
            sliderEscudoInstanciado.fillRect.parent
                as RectTransform;

        // Meter el área del escudo dentro del fill de vida
        fillAreaEscudo.SetParent(
            fillVida,
            false
        );

        // Ocupar exactamente el espacio del fill
        fillAreaEscudo.anchorMin =
            Vector2.zero;

        fillAreaEscudo.anchorMax =
            Vector2.one;

        fillAreaEscudo.offsetMin =
            Vector2.zero;

        fillAreaEscudo.offsetMax =
            Vector2.zero;

        fillAreaEscudo.localScale =
            Vector3.one;

        fillAreaEscudo.anchoredPosition =
            Vector2.zero;

        // Encima visualmente
        fillAreaEscudo.SetAsLastSibling();
    }

    private void elegirAccion()
    {
        AccionElegida = PossibleActions[UnityEngine.Random.Range(0, PossibleActions.Count)];

        ValueNextAttackInstanciado.text = AccionElegida.value.ToString();
        ImageNextAttackInstanciado.sprite = ActionImage(AccionElegida.type);
    }
    private void TakeAction(GameEventBase game = null)
    {
        if (turnService.IsPlayerTurn())
        {
            elegirAccion();



            return;
        }
        StartCoroutine(ExecuteTurn());

    }

    

    private IEnumerator ExecuteTurn()
    {
        //TODO Revisar esto si da tiempo 100% se puede mejorar me he metido una fumada buena
        yield return new WaitForSeconds(2f);
        bool turnoEnemigoListo = false;
        bool SiguienteEnemigo = false;

        List<GameObject> listaEnemigos = enemyService.getEnemyList();

        switch (AccionElegida.type)
        {
            case ActionTypes.attack:
                Buff attackBuff = buffService.GetBuff(guid, BuffType.attack);
                int attackMod = attackBuff != null ? attackBuff.value : 0;
                characterService.takeDamage(AccionElegida.value + attackMod);
                animator.SetTrigger("Attack");
                Debug.Log($"El jugador recibe {AccionElegida.value} de daño");
                break;
            case ActionTypes.defense:
                Buff defenceBuff = buffService.GetBuff(guid, BuffType.defense);
                int defenseMod = defenceBuff != null ? defenceBuff.value : 0;
                characterService.takeDamage(AccionElegida.value + defenseMod);
                shield += AccionElegida.value + defenseMod;
                updateEscudoUI();
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de escudo");
                break;
            case ActionTypes.BuffAttack:
                buffService.AddBuff(new Buff(BuffType.attack, AccionElegida.value, AccionElegida.duration, guid));
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de bufo al ataque durante {AccionElegida.duration} turnos");
                break;
            case ActionTypes.BuffDefense:
                buffService.AddBuff(new Buff(BuffType.defense, AccionElegida.value, AccionElegida.duration, guid));
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de bufo a la defensa durante {AccionElegida.duration} turnos");
                break;
            case ActionTypes.Heal:
                life += AccionElegida.value;
                Debug.Log($"{this.gameObject.name} se cura {AccionElegida.duration} ");
                break;

        }


        for (int i = 0; i < listaEnemigos.Count ; i++)
        {
            EnemyBase enemy = listaEnemigos[i].GetComponent<EnemyBase>();
            
            if (enemy != null)
            {
                if(!enemy.isTurnEnded)
                {
                    enemy.isTurnEnded = true;
                    listaEnemigos[i] = enemy.gameObject;
                    break;
                }
            }
        }

        enemyService.setEnemyList(listaEnemigos);

        foreach (GameObject enemyGO in listaEnemigos)
        {
            EnemyBase enemy = enemyGO.GetComponent<EnemyBase>();

            if(!enemy.isTurnEnded)SiguienteEnemigo = true;
        }
        if(!SiguienteEnemigo)turnoEnemigoListo = true;
        if(turnoEnemigoListo)turnService.ChangeTurn();    
    }

    private void updateEscudoUI()
    {
        
        if (sliderEscudoInstanciado.maxValue < shield && sliderEscudoInstanciado.maxValue < life)
        {

            sliderEscudoInstanciado.maxValue = shield;
        }


        
        sliderEscudoInstanciado.value = shield;
        textoEscudoInstanciad.text = shield.ToString();
    }

    public void OnHit(int damage)
    {
        //TODO: quitarle primero da�o al escudo si hay
        for (int i = 1; i <= damage; i++)
        {
            if (shield > 0)
            {
                shield--;
            }
            else
            {
                life--;
            }
        }


        if (life <= 0) {

            if (sliderVidaInstanciado != null) { 
            sliderVidaInstanciado.value = 0;
            sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = "0";
            }
            Die();
            return;
        }
        else
        {
            if (sliderVidaInstanciado != null)
            {
                sliderVidaInstanciado.value = life;
                sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = life.ToString();
            }
        }
        updateEscudoUI();
        Debug.Log("me dañaste, me quedan " + life);

    }

    private void Die()
    {
        dead = true;

        animator.SetBool("isDead", true);
        Debug.Log("me Mori");
        enemyService.removeFirstEnemy();
        eventService.Unsubscribe<TurnChangeEvent>(TakeAction);
        buffService.RemoveBuffByGUID(guid); 
        if (enemyService.getEnemyList().Count <= 0) StartCoroutine(RewardChange());

        StartCoroutine("Disappear");
    }

    public IEnumerator RewardChange()
    {
        buffService.ClearBuffList();
        yield return new WaitForSeconds(2f);
        sceneService.LoadScene(SceneNames.RewardScene);
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3f);
        Destroy(sliderVidaInstanciado.gameObject);
        Destroy(ImageNextAttackInstanciado.gameObject);
        Destroy(ValueNextAttackInstanciado.gameObject);
        Destroy(gameObject);
    }
}
