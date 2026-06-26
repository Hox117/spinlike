using UnityEngine;
using UnityEngine.UI;

public class botonSilenciar : MonoBehaviour
{
    IAudioService audioService;
    Image imagenPuesta;
    [SerializeField] Sprite audioActivado;
    [SerializeField] Sprite audioDesactivado;
    [SerializeField] Sprite sonidoActivado;
    [SerializeField] Sprite sonidoDesactivado;
    [SerializeField] bool isMusic;

    private void Start()
    {
        audioService = AppContainer.Get<IAudioService>();
        imagenPuesta = GetComponent<Image>();
    }

    private void Update()
    {
        if (isMusic)
        {
            if (audioService.getMusicVolume() >= 1)
            {
                imagenPuesta.sprite = audioActivado;
            }
            else
            {
                imagenPuesta.sprite = audioDesactivado;
            }
        }
        else
        {
            if (audioService.getSoundVolume() >= 1)
            {
                imagenPuesta.sprite = sonidoActivado;
            }
            else
            {
                imagenPuesta.sprite = sonidoDesactivado;
            }
        }
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
