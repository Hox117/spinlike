
using System;
using System.Collections.Generic;
using UnityEngine;



public class MapService : IMapService
{
    List<(MapTypess, int altura, int ancho)> map = new List<(MapTypess, int, int)>();

    private (int altura, int ancho) posicionJugador = (1,1);

    private int longitudMapa = 0;

    private int anchoMapa = 0;

    private bool moving = false;

    public List<(MapTypess, int altura, int ancho)> generateMap(int longitud,int ancho)
    {
        map.Clear();

        if (longitud <= 2)
        {
            Debug.Log("El mapa es demasiado Corto");
            return null;
        }

        for (int i = 1; i <= longitud; i++)
        {
            if (i == 1)
            {
                map.Add((MapTypess.empty, i, 1));
            }
            if (i == 2)
            {
                map.Add((MapTypess.combat, i, 1));
            }
            if (i > 2 && i < longitud)
            {
                generateInAncho(i,ancho);
            }
            if (i == longitud)
            {
                map.Add((MapTypess.Boss,i,1));
            }
            
        }
        longitudMapa = longitud;
        anchoMapa = ancho;
        return map;



    }

    private void generateInAncho(int altura,int ancho)
    {
        int numeroAnchura = UnityEngine.Random.Range(2, ancho+1);

        for (int i = 1; i <= numeroAnchura; i++)
        {
            var seleccionado = ObtenerEnumAleatorio<MapTypess>();
            while (seleccionado == MapTypess.Boss)
            {
                seleccionado = ObtenerEnumAleatorio<MapTypess>();
            }
            map.Add((seleccionado, altura, i));

        }
    }

    private T ObtenerEnumAleatorio<T>() where T : Enum
    {
        Array valores = Enum.GetValues(typeof(T));
        System.Random random = new System.Random();
        return (T)valores.GetValue(random.Next(valores.Length));
    }


 
    public List<(MapTypess, int altura, int ancho)> ReadMap()
    {
        if (map.Count <= 0) return null;
        return map;
    }

    public (int altura, int ancho) GetPosicionJugador()
    {
        return posicionJugador;
    }

    public void SetPositionPlayer(int altura, int ancho)
    {
        posicionJugador = (altura, ancho);

    }

    public void ResetMap()
    {
        map.Clear();
        posicionJugador = (1, 1);
        
    }

    public int returnLongitud()
    {
        return longitudMapa;
    }

    public int returnAncho()
    {
        return anchoMapa;
    }

    public void ToggleMoving()
    {
        moving = !moving;
    }
    public bool GetMoving()
    {
        return moving;
    }
}
