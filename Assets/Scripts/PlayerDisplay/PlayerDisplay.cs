using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] Slider sliderVida;
    [SerializeField] Slider sliderEscudo;
    [SerializeField] Image BufoEscudo;
    [SerializeField] Image BufoDano;
    
    Slider sliderVidaInstanciado;
    Slider sliderEscudoInstanciado;
    TextMeshProUGUI textoEscudoInstanciad;

    Image bufoEscudoInstanciado;
    Image bufoDanoInstanciado;

    TextMeshProUGUI BufoEscudoTexto;
    TextMeshProUGUI BufoDanoTexto;

    int vida = 0;
    int escudo = 0;

    ICharacterService characterService;
    IEventService eventService;
    Animator playerAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterService = AppContainer.Get<ICharacterService>();
        instantiateSlider();
        eventService = AppContainer.Get<IEventService>();
        eventService.Subscribe<UpdatePlayerUI>(updateUI);
        playerAnimator = GetComponent<Animator>();
        eventService.Subscribe<PlayerAttackEvent>(Attack);
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


        sliderVidaInstanciado = barra.GetComponent<Slider>();
        sliderEscudoInstanciado = barraEscudo.GetComponent<Slider>();

        bufoEscudoInstanciado = imagenBufoEscudo.GetComponent<Image>();
        bufoDanoInstanciado = imagenDanoEscudo.GetComponent<Image>();
        

        vida = characterService.getLife();
        escudo = characterService.getShield();

        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = vida.ToString();
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30;

        sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = escudo.ToString();
        sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        sliderEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30;

        BufoEscudoTexto = bufoEscudoInstanciado.GetComponentInChildren<TextMeshProUGUI>();
        BufoDanoTexto = bufoDanoInstanciado.GetComponentInChildren<TextMeshProUGUI>();
        updatebufos();
        

        sliderVidaInstanciado.maxValue = vida;
        sliderVidaInstanciado.value = vida;
        sliderVidaInstanciado.minValue = 0;


        sliderEscudoInstanciado.maxValue = 0.1f;
        sliderEscudoInstanciado.value = escudo;
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

    private void updateUI(GameEventBase e)
    {
        updateVIdaUI();
        updateEscudoUI();
        updatebufos();
        if (vida <= 0)
        {
            playerAnimator.SetBool("isDead", true);
        }



    }

    private void updateVIdaUI()
    {
        if (sliderVidaInstanciado.maxValue < characterService.getLife())
        {

            sliderVidaInstanciado.maxValue = characterService.getLife();
        }
        vida = characterService.getLife();
        sliderVidaInstanciado.value = vida;
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = vida.ToString();
    }
    private void updateEscudoUI()
    {
        if (sliderEscudoInstanciado.maxValue < characterService.getShield() && sliderEscudoInstanciado.maxValue < characterService.getLife())
        {

            sliderEscudoInstanciado.maxValue = characterService.getShield();
        }


        escudo = characterService.getShield();
        sliderEscudoInstanciado.value = escudo;
        textoEscudoInstanciad.text = escudo.ToString();
    }

    private void updatebufos()
    {
        if (characterService.getBuff( BuffType.defense) != null && characterService.getBuff(BuffType.defense).duration > 0)
        {
            BufoEscudoTexto.text = $"{characterService.getBuff(BuffType.defense).value}/{characterService.getBuff(BuffType.defense).duration}";
            bufoEscudoInstanciado.enabled = true;
        }
        else
        {
            BufoEscudoTexto.text = " ";
            bufoEscudoInstanciado.enabled = false;
        }

        if (characterService.getBuff( BuffType.attack) != null && characterService.getBuff( BuffType.attack).duration > 0)
        {
            BufoDanoTexto.text = $"{characterService.getBuff( BuffType.attack).value}/{characterService.getBuff(BuffType.attack).duration}";

            bufoDanoInstanciado.enabled = true;
        }
        else
        {
            BufoDanoTexto.text = " ";
            bufoDanoInstanciado.enabled = false;
        }


    }
    private void Attack(GameEventBase e)
    {
        playerAnimator.SetTrigger("Attack");
    }

    private void OnDestroy()
    {
        eventService.Unsubscribe<UpdatePlayerUI>(updateUI);
        eventService.Unsubscribe<PlayerAttackEvent>(Attack);
    }

}