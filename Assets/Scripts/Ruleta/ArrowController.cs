using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
    }
}
