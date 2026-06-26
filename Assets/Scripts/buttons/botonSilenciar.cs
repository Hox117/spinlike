using UnityEngine;

public class botonSilenciar : MonoBehaviour
{
    IAudioService audioService;

    private void Start()
    {
        audioService = AppContainer.Get<IAudioService>();

    }

    void Silenciar()
    {
        audioService.SetMusicVolume(1f);

    }
}
