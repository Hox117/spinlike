using System.Collections.Generic;
using UnityEngine;

public class Ficha
{
    public Color colorPrincipal;
    public Color colorSecundario;
    public Sprite sprite;
    public string nombre;
    public List<Action> actions;

    public string description;
    public string SegmentData;

    public AudioClip audioClip;
    public Ficha(FichaData data)
    {
        colorPrincipal = data.colorPrincipal;
        colorSecundario = data.colorSecundario;
        sprite = data.sprite;
        actions = data.actions;
        nombre = data.nombre;
        description = data.description;
        SegmentData = data.SegmentData;
        audioClip = data.audioClip;
    }
}
