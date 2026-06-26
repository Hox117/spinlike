using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    public void toggle() { 
    this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
