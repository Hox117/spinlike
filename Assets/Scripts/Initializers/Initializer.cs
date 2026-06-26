using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    IInventoryService inventoryService;
    ITurnService turnService;
    IEventService eventService;
    [SerializeField] List<FichaData> ListaDeFichas;
    [SerializeField] Wheel_Manager ruleta;
    private IAudioService _audioService;
    [SerializeField] private AudioClip backGroundMusic;
    public void setListaDeFichas(List<FichaData> ListaDeFichas)
    {
        this.ListaDeFichas = ListaDeFichas;
    }
    void Awake()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        eventService = AppContainer.Get<IEventService>();
        _audioService = AppContainer.Get<IAudioService>();

        if (backGroundMusic != null) _audioService.PlayMusic(new AudioClip[] { backGroundMusic });

        if (inventoryService.getListaFichas().Count < 1) inventoryService.cargarInventario(ListaDeFichas);
        if (!turnService.IsPlayerTurn()) turnService.ChangeTurn();

        eventService.Subscribe<TurnChangeEvent>(AleatorizarRuleta);

    }

    void AleatorizarRuleta(GameEventBase e)
    {
        if (turnService.IsPlayerTurn())
        {
            ruleta.GenerateRoulette();
        }
    }

    private void OnDestroy()
    {
        eventService.Unsubscribe<TurnChangeEvent>(AleatorizarRuleta);
        if(backGroundMusic != null) _audioService.DestroyAudioSources();
    }
}
