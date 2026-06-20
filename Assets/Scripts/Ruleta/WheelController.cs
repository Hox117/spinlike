using System;
using System.Collections;
using UnityEngine;

public class WheelController : MonoBehaviour
{

    IRouletteService rouletteService;

    void Awake()
    {
        rouletteService = AppContainer.Get<IRouletteService>();

    }

    public void StopSpin()
    {
        
        StartCoroutine(StopRoulette());
    }
    public void StartSpin()
    {
        StartCoroutine(letItRide());
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
