using System.Collections.Generic;
using UnityEngine;

public class BotonGenerarPrueba : MonoBehaviour
{
    IInventoryService inventoryService;
    IRouletteService rouletteService;
    public int numeroSegmentos = 3;
    public List<Color[]> colores;
    public List<string> textos;
    public List<Sprite> sprites;

    [SerializeField] private int SpinSpeed = 300;

    public bool SPRITE = false;

    public Texture2D sprite;
  public void Generar()
    {

        colores = new List<Color[]>();
        textos = new List<string>();
        sprites = new List<Sprite>();


        inventoryService = AppContainer.Get<IInventoryService>();
        rouletteService = AppContainer.Get<IRouletteService>();
        inventoryService.ramdomizeList();

        rouletteService.ChangeSpeed(SpinSpeed);
        
        List<Ficha> listaFichas = inventoryService.getListaFichas();
        numeroSegmentos = listaFichas.Count;

        for (int i = 0; i < numeroSegmentos; i++)
        {
            colores.Add(new Color[]{ listaFichas[i].colorPrincipal, listaFichas[i].colorSecundario });
            textos.Add(listaFichas[i].valor.ToString());
            sprites.Add(listaFichas[i].sprite);
        }
        //para cada ficha sacamos su color y color secundario

        FindAnyObjectByType<Wheel_Manager>().Generate(numeroSegmentos, colores,textos,sprites, listaFichas);
       
    }
}
