using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour, IHittable
{
    [SerializeField]Enemy enemyData;
    [SerializeField] Slider sliderVida;
    [SerializeField] Image ImageNextAttack;
    [SerializeField] TextMeshProUGUI ValueNextAttack;

    TextMeshProUGUI ValueNextAttackInstanciado;
    Image ImageNextAttackInstanciado;
    Slider sliderVidaInstanciado;

    Sprite[] sprites;

    int life;
    int shield;
    int AttackMod = 0;
    int ShieldMod = 0;
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
    void Awake()
    {
        enemyService = AppContainer.Get<IEnemyService>();
        eventService = AppContainer.Get<IEventService>();
        turnService = AppContainer.Get<ITurnService>();
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();
        sprites = Resources.LoadAll<Sprite>($"mapa");
    }
    void Start()
    {
        life = enemyData.Life;
        shield = enemyData.Shield;
        AttackMod = enemyData.attackMod;
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

        float scale = canvas.scaleFactor;
        //AQUI GABRIEL
        imagenAtaque.anchoredPosition = new Vector2(posicionUI.x + 40f / scale, posicionUI.y + imagenAtaque.localPosition.y / 2 / scale);
        ValorAtaque.anchoredPosition = new Vector2(posicionUI.x - 40f / scale, posicionUI.y + ValorAtaque.localPosition.y / 2 / scale);

        sliderVidaInstanciado = barra.GetComponent<Slider>();
        ImageNextAttackInstanciado = imagenAtaque.GetComponent<Image>();
        ValueNextAttackInstanciado = ValorAtaque.GetComponent<TextMeshProUGUI>();

        sliderVidaInstanciado.maxValue = life;
        sliderVidaInstanciado.value = life;

        sliderVidaInstanciado.minValue = 0;
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
                characterService.takeDamage(AccionElegida.value + AttackMod);
                Debug.Log($"El jugador recibe {AccionElegida.value} de daño");
                break;
            case ActionTypes.defense:
                shield += AccionElegida.value + ShieldMod;
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de escudo");
                break;
            case ActionTypes.BuffAttack:
                AttackMod = AccionElegida.value;
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de bufo al ataque durante {AccionElegida.duration} turnos");
                break;
            case ActionTypes.BuffDefense:
                ShieldMod = AccionElegida.value;
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


    public void OnHit(int damage)
    {
        //TODO: quitarle primero da�o al escudo si hay
        life -= damage;

        

        if (life <= 0) {

            sliderVidaInstanciado.value = 0;
            sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = "0";

            Die();
            return;
        }
        else
        {
            sliderVidaInstanciado.value = life;
            sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = life.ToString();
        }
        Debug.Log("me dañaste, me quedan " + life);

    }

    private void Die()
    {
        dead = true;

        animator.SetBool("isDead", true);
        Debug.Log("me Mori");
        enemyService.removeFirstEnemy();
        eventService.Unsubscribe<TurnChangeEvent>(TakeAction);

        if(enemyService.getEnemyList().Count <= 0) StartCoroutine(RewardChange());

        StartCoroutine("Disappear");
    }

    public IEnumerator RewardChange()
    {
        yield return new WaitForSeconds(2f);
        sceneService.LoadScene(SceneNames.RewardScene);
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3f);
        Destroy(sliderVidaInstanciado.gameObject);
        Destroy(gameObject);
    }
}
