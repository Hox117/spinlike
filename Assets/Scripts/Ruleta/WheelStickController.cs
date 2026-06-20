using UnityEngine;

public class WheelStickController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}
