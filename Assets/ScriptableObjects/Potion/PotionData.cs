using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
public class PotionData : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public List<Action> actions;
    public string description;

}
