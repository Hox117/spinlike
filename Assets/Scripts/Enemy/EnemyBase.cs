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
    [SerializeField] Enemy enemyData;
    [SerializeField] Slider sliderVida;
    [SerializeField] Image ImageNextAttack;
    [SerializeField] TextMeshProUGUI ValueNextAttack;
    [SerializeField] Slider sliderEscudo;
    [SerializeField] Image BufoEscudo;
    [SerializeField] Image BufoDano;
    [SerializeField] bool isBoss = false;
    [SerializeField] GameObject Explosion;

    Slider sliderEscudoInstanciado;
    TextMeshProUGUI textoEscudoInstanciad;

    TextMeshProUGUI ValueNextAttackInstanciado;
    Image ImageNextAttackInstanciado;
    Slider sliderVidaInstanciado;

    Image bufoEscudoInstanciado;
    Image bufoDanoInstanciado;

    TextMeshProUGUI BufoEscudoTexto;
    TextMeshProUGUI BufoDanoTexto;


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
    IAudioService audioService;
    void Awake()
    {
        enemyService = AppContainer.Get<IEnemyService>();
        eventService = AppContainer.Get<IEventService>();
        turnService = AppContainer.Get<ITurnService>();
        characterService = AppContainer.Get<ICharacterService>();
        buffService = AppContainer.Get<IBuffService>();
        sceneService = AppContainer.Get<ISceneService>();
        audioService = AppContainer.Get<IAudioService>();
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

        RectTransform imagenBufoEscudo = Instantiate(BufoEscudo.GetComponent<RectTransform>(), canvas.transform);
        RectTransform imagenDanoEscudo = Instantiate(BufoDano.GetComponent<RectTransform>(), canvas.transform);
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
        imagenBufoEscudo.anchoredPosition = posicionUI + Vector2.up * 50 + Vector2.left * 30;
        imagenDanoEscudo.anchoredPosition = posicionUI + Vector2.up * 50 + Vector2.left * -90;

        float scale = canvas.scaleFactor;
        //AQUI GABRIEL
        imagenAtaque.anchoredPosition = new Vector2(posicionUI.x + 20f, posicionUI.y + +60f);
        ValorAtaque.anchoredPosition = new Vector2(posicionUI.x - 20f, posicionUI.y + 60f);

        sliderVidaInstanciado = barra.GetComponent<Slider>();
        sliderEscudoInstanciado = barraEscudo.GetComponent<Slider>();
        ImageNextAttackInstanciado = imagenAtaque.GetComponent<Image>();
        ValueNextAttackInstanciado = ValorAtaque.GetComponent<TextMeshProUGUI>();
        bufoEscudoInstanciado = imagenBufoEscudo.GetComponent<Image>();
        bufoDanoInstanciado = imagenDanoEscudo.GetComponent<Image>();

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





        BufoEscudoTexto = bufoEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>();
        BufoDanoTexto = bufoDanoInstanciado.GetComponentInChildren<TextMeshProUGUI>();
        updatebufos();
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
        yield return new WaitForSeconds(2f);


        switch (AccionElegida.type)
        {
            case ActionTypes.attack:
                Buff attackBuff = buffService.GetBuff(guid, BuffType.attack);
                int attackMod = attackBuff != null ? attackBuff.value : 0;
                characterService.takeDamage(AccionElegida.value + attackMod);
                audioService.PlaySound(enemyData.AttackAudio);
                animator.SetTrigger("Attack");
                Debug.Log($"El jugador recibe {AccionElegida.value} de daño");
                break;
            case ActionTypes.defense:
                Buff defenceBuff = buffService.GetBuff(guid, BuffType.defense);
                int defenseMod = defenceBuff != null ? defenceBuff.value : 0;
                shield += AccionElegida.value + defenseMod;
                audioService.PlaySound(enemyData.DefenseSound);
                updateEscudoUI();
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de escudo");
                break;
            case ActionTypes.BuffAttack:
                audioService.PlaySound(enemyData.BuffSound);
                buffService.AddBuff(new Buff(BuffType.attack, AccionElegida.value, AccionElegida.duration, guid));
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de bufo al ataque durante {AccionElegida.duration} turnos");

                break;
            case ActionTypes.BuffDefense:
                audioService.PlaySound(enemyData.BuffSound);
                buffService.AddBuff(new Buff(BuffType.defense, AccionElegida.value, AccionElegida.duration, guid));
                Debug.Log($"{this.gameObject.name} recibe {AccionElegida.value} de bufo a la defensa durante {AccionElegida.duration} turnos");


                break;
            case ActionTypes.Heal:
                life += AccionElegida.value;
                audioService.PlaySound(enemyData.DefenseSound);
                Debug.Log($"{this.gameObject.name} se cura {AccionElegida.duration} ");
                updateEscudoUI();
                sliderVidaInstanciado.value = life;
                sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = life.ToString();
                break;

        }
        if (AccionElegida.type != ActionTypes.attack)
        {
            finalizarTurno();
        }
        updatebufos();
    }

    public void finalizarTurno()
    {
        enemyService.endTurn(this.gameObject);
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
    private void updatebufos()
    {
        if (buffService.GetBuff(guid, BuffType.defense) != null && buffService.GetBuff(guid, BuffType.defense).duration > 0)
        {
            BufoEscudoTexto.text = $"{buffService.GetBuff(guid, BuffType.defense).value}/{buffService.GetBuff(guid, BuffType.defense).duration}";
            bufoEscudoInstanciado.enabled = true;
        }
        else
        {
            BufoEscudoTexto.text = " ";
            bufoEscudoInstanciado.enabled = false;
        }

        if (buffService.GetBuff(guid, BuffType.attack) != null && buffService.GetBuff(guid, BuffType.attack).duration > 0)
        {
            BufoDanoTexto.text = $"{buffService.GetBuff(guid, BuffType.attack).value}/{buffService.GetBuff(guid, BuffType.attack).duration}";

            bufoDanoInstanciado.enabled = true;
        }
        else
        {
            BufoDanoTexto.text = " ";
            bufoDanoInstanciado.enabled = false;
        }


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


        if (life <= 0)
        {

            if (sliderVidaInstanciado != null)
            {
                sliderVidaInstanciado.value = 0;
                sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = "0";
            }
            Die();

            audioService.PlaySound(enemyData.DieSound);
            return;
        }
        else
        {
            if (sliderVidaInstanciado != null)
            {
                audioService.PlaySound(enemyData.DamagedSound);
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
        if(!isBoss)sceneService.LoadScene(SceneNames.RewardScene);
        else sceneService.LoadScene(SceneNames.WIN);
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3f);
        GameObject explo = Instantiate(Explosion, transform.position,Quaternion.identity);
        explo.transform.SetParent(null);
        Destroy(sliderVidaInstanciado.gameObject);
        Destroy(ImageNextAttackInstanciado.gameObject);
        Destroy(ValueNextAttackInstanciado.gameObject);
        Destroy(bufoDanoInstanciado.gameObject);
        Destroy(bufoEscudoInstanciado.gameObject);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        eventService.Unsubscribe<TurnChangeEvent>(TakeAction);
    }
}
