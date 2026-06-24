using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] Slider sliderVida;
    Slider sliderVidaInstanciado;

    ICharacterService characterService;
    IEventService eventService;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterService = AppContainer.Get<ICharacterService>();
        instantiateSlider();
        eventService = AppContainer.Get<IEventService>();
        eventService.Subscribe<UpdatePlayerUI>(updateUI);
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

        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = characterService.getLife().ToString();
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30;


        sliderVidaInstanciado.maxValue = characterService.getLife();
        sliderVidaInstanciado.value = characterService.getLife();

        sliderVidaInstanciado.minValue = 0;
    }

    private void updateUI(GameEventBase e)
    {
        sliderVidaInstanciado.value = characterService.getLife();
        sliderVidaInstanciado.GetComponentInChildren<TextMeshProUGUI>().text = characterService.getLife().ToString();
    }
    private void OnDestroy()
    {
        eventService.Unsubscribe<UpdatePlayerUI>(updateUI);
    }

}
