using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] Slider sliderVida;
    Slider sliderVidaInstanciado;

    int lastLife = 0;

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
       

        sliderVidaInstanciado = barra.GetComponent<Slider>();

        lastLife = characterService.getLife();
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = lastLife.ToString();
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30;


        sliderVidaInstanciado.maxValue = lastLife;
        sliderVidaInstanciado.value = lastLife;

        sliderVidaInstanciado.minValue = 0;
    }

    private void updateUI(GameEventBase e)
    {
        updateVIdaUI();
        if (lastLife <= 0)
        {
            playerAnimator.SetBool("isDead", true);
        }
        
        
        
    }

    private void updateVIdaUI()
    {
        lastLife = characterService.getLife();
        sliderVidaInstanciado.value = lastLife;
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = lastLife.ToString();
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
