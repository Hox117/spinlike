using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ficha", menuName = "Scriptable Objects/FichaData")]
public class FichaData : ScriptableObject
{
    public Color colorPrincipal;
    public Color colorSecundario;
    public Sprite sprite;
    public string nombre;
    public List<Action> actions;

    public string description;

    public AudioClip audioClip;

}
