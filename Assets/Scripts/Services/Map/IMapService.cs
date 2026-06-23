using System.Collections.Generic;
using UnityEngine;

public interface IMapService 
{

    public List<(MapTypess, int altura, int ancho)> generateMap(int longitud, int ancho);
    public List<(MapTypess, int altura, int ancho)> ReadMap();
    public (int altura, int ancho) GetPosicionJugador();
    public void SetPositionPlayer(int altura, int ancho);

    public void ResetMap();

    public int returnLongitud();
    public int returnAncho();
}
