using System;
using System.Collections;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Coroutine coroutine;
    float speed = 100f;
    float stop = 10f;

    public void StopSpin()
    {
        
        StartCoroutine(StopRoulette());
    }
    public void StartSpin()
    {
        StartCoroutine(letItRide());
    }
    private IEnumerator StopRoulette()
    {
        float time = UnityEngine.Random.Range(0.5f, 2f );
        while (speed >= 0.1f)
        {
            speed -= stop;
            yield return new WaitForSeconds(0.2f);
        }
        
        StopAllCoroutines();
    }

    private IEnumerator letItRide()
    {
        speed = 100f;

        while (true)
        {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            yield return null;
        }
    }
}
