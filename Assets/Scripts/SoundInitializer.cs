using UnityEngine;

public class SoundInitializer : MonoBehaviour
{
    IAudioService audioService;
    [SerializeField] AudioClip BackGroundMusic;
    void Start()
    {
        audioService = AppContainer.Get<IAudioService>();
        audioService.PlaySound(BackGroundMusic);
    }
    private void OnDestroy()
    {
        audioService.StopSound(BackGroundMusic);
    }
}
