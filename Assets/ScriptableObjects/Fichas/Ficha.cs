using UnityEngine;

[CreateAssetMenu(fileName = "Ficha", menuName = "Scriptable Objects/Ficha")]
public class Ficha : ScriptableObject
{
    public Color colorPrincipal;
    public Color colorSecundario;
    public int valor;
    public Sprite sprite;
    public FichaTypes tipoDeFicha;
    public int id;

    public bool useDescription;
    public string description;

}
