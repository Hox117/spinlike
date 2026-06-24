using UnityEngine;

public class TestAutodestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.LogWarning("Me mataron :V");
    }
}
