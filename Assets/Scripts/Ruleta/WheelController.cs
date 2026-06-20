using System.Collections;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Coroutine coroutine;
    void Start()
    {
        coroutine = StartCoroutine(letItRide());
    }


    public IEnumerator letItRide()
    {
        float speed = 10f;

        while (true)
        {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            yield return null;
        }
    }
}
