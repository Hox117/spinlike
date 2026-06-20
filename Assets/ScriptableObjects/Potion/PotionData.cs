using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
public class PotionData : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public PotionEffect effect;
    public float value;
    public float duration=0;

}
