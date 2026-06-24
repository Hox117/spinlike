using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WheelController : MonoBehaviour
{

    protected IRouletteService rouletteService;
    protected IEventService eventService;
    protected IAudioService audioService;
    ITurnService turnService;
    [SerializeField] protected AudioClip audioSpin;
    [SerializeField] protected AudioClip audioStopSpin;
    [SerializeField] float pitchSpin = 1;
    


    protected virtual void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();
        turnService = AppContainer.Get<ITurnService>();
        audioService = AppContainer.Get<IAudioService>();
        eventService = AppContainer.Get<IEventService>();
    }
    void Start()
    {
        StartSpin();
    }
    public void StopSpin()
    {
        if (turnService.IsPlayerTurn())
        {
            StartCoroutine(StopRoulette());
        }
        
    }
    public void StartSpin(EventBase e=null)
    {
        if (turnService.IsPlayerTurn())
        {
            StartCoroutine(letItRide());

            audioService.PlayLoopSound(audioSpin, pitchSpin);
        }
        
    }
    public void StartSpin()
    {
        if (turnService.IsPlayerTurn())
        {
            StartCoroutine(letItRide());

            audioService.PlayLoopSound(audioSpin, pitchSpin);
        }

    }
    protected virtual IEnumerator StopRoulette()
    {
        while (rouletteService.GetSpeed() >= 0)
        {
            rouletteService.ChangeSpeed(rouletteService.GetSpeed() - rouletteService.GetStop());
            yield return new WaitForSeconds(0.2f);
        }
        
        audioService.StopSound(audioSpin);
        audioService.PlaySound(audioStopSpin);
        
        StopAllCoroutines();
        eventService.Publish(new StopWheelEvent());
    }

    protected virtual IEnumerator letItRide()
    {
        rouletteService.ResetSpeed();
        
        while (true)
        {
            transform.Rotate(Vector3.forward, rouletteService.GetSpeed() * Time.deltaTime);
            yield return null;
        }
    }
}
