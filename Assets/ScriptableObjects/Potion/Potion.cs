using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
public class Potion : ScriptableObject
{
    public PotionEffect effect;
    public float value;
    public float duration=0;

}
