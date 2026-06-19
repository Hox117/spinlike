using UnityEngine;

public class BotonGenerarPrueba : MonoBehaviour
{

    public int numeroSegmentos = 3;
    public (float, float) radio = (0, 2);
    public Color[] colores = {Color.black,Color.whiteSmoke};
    public int arcSubdivisiones = 2;

    public bool SPRITE = false;

    public Texture2D sprite;
  public void Generar()
    {
        if (!SPRITE)
        FindAnyObjectByType<Wheel_Manager>().Generate(numeroSegmentos,radio, colores, arcSubdivisiones);
        else
            FindAnyObjectByType<Wheel_Manager>().Generate(numeroSegmentos, radio, sprite, arcSubdivisiones);
    }
}
