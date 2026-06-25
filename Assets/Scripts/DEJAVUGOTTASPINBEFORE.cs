using UnityEngine;

public class DEJAVUGOTTASPINBEFORE : MonoBehaviour
{

    [SerializeField] WheelController wheel;

    private void Start()
    {
        if (wheel == null) wheel = FindAnyObjectByType<WheelController>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = wheel.transform.rotation;
    }
}
