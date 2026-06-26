using System.Collections;

using UnityEngine;

public class WheelMapController : WheelController
{
    private IMapService _mapService;

    protected override void Awake()
    {
        base.Awake();
        _mapService = AppContainer.Get<IMapService>();
    }
    protected override IEnumerator StopRoulette()
    {
        while (rouletteService.GetSpeed() >= 0)
        {
            rouletteService.ChangeSpeed(rouletteService.GetSpeed() - rouletteService.GetStop());
            yield return new WaitForSeconds(0.2f);
        }

        audioService.StopSound(audioSpin);
        audioService.PlaySound(audioStopSpin);
        
        StopAllCoroutines();
        
        StartCoroutine(waitToDestroy());

    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(1);
        StopMapWheelEvent stopwheelEvent = new StopMapWheelEvent();
        _mapService.ToggleMoving();
        eventService.Publish(stopwheelEvent);
        
        GameObject parent = this.transform.parent.gameObject;
        
        Destroy(parent);
    }

    protected override IEnumerator letItRide()
    {
        rouletteService.ResetSpeed();

        while (true)
        {
            transform.Rotate(Vector3.forward, Random.Range(200, 600) * Time.deltaTime);
            yield return null;
        }
    }
}
