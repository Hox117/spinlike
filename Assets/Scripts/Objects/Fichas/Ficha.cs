using System.Collections.Generic;
using UnityEngine;

public class Ficha
{
    public Color colorPrincipal;
    public Color colorSecundario;
    public Sprite sprite;

    public List<Action> actions;

    public string description;

    public AudioClip audioClip;
    public Ficha(FichaData data)
    {
        colorPrincipal = data.colorPrincipal;
        colorSecundario = data.colorSecundario;
        sprite = data.sprite;
        actions = data.actions;
        description = data.description;
        audioClip = data.audioClip;
    }
}
