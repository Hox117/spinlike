using System;
using System.Collections;
using UnityEngine;

public class WheelController : MonoBehaviour
{

    IRouletteService rouletteService;
    IAudioService audioService;
    [SerializeField] AudioClip audioSpin;
    [SerializeField] AudioClip audioStopSpin;
    [SerializeField] float pitchSpin = 1;
    


    void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();

        audioService = AppContainer.Get<IAudioService>();
    }

    public void StopSpin()
    {
        
        StartCoroutine(StopRoulette());
    }
    public void StartSpin()
    {
        StartCoroutine(letItRide());

        audioService.PlayLoopSound(audioSpin, pitchSpin);
        rouletteService.StartRoulette();
    }
    private IEnumerator StopRoulette()
    {
        while (rouletteService.GetSpeed() >= 0.1f)
        {
            rouletteService.ChangeSpeed(rouletteService.GetSpeed() - rouletteService.GetStop());
            yield return new WaitForSeconds(0.2f);
        }
        rouletteService.StopRoulette();
        audioService.StopSound(audioSpin);
        audioService.PlaySound(audioStopSpin);
        StopAllCoroutines();
        
    }

    private IEnumerator letItRide()
    {
        rouletteService.ResetSpeed();
        
        while (true)
        {
            transform.Rotate(Vector3.forward, rouletteService.GetSpeed() * Time.deltaTime);
            yield return null;
        }
    }
}
