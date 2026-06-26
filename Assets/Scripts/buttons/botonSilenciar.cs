using UnityEngine;

public class botonSilenciar : MonoBehaviour
{
    IAudioService audioService;

    private void Start()
    {
        audioService = AppContainer.Get<IAudioService>();

    }

    public void SilenciarMusica()
    {
       if (audioService.getMusicVolume() >= 1)
        {
            audioService.SetMusicVolume(0);
        }
        else
        {
            audioService.SetMusicVolume(1);
        }

    }
   public void SilenciarSonido()
    {
        if (audioService.getSoundVolume() >= 1)
        {
            audioService.SetSFXVolume(0);
        }
        else
        {
            audioService.SetSFXVolume(1);
        }

    }
}
