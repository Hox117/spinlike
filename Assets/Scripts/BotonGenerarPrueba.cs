using System.Collections.Generic;
using UnityEngine;

public class BotonGenerarPrueba : MonoBehaviour
{
    IInventoryService inventoryService;
    IRouletteService rouletteService;
    ITurnService turnService;
    public int numeroSegmentos = 3;
    public List<Color[]> colores;
    //public List<string> textos;
    public List<Sprite> sprites;

    [SerializeField] private int SpinSpeed = 300;

    public bool SPRITE = false;

    public Texture2D sprite;

    void Awake()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        rouletteService = AppContainer.Get<IRouletteService>();
        turnService = AppContainer.Get<ITurnService>();
    }
    public void Generar()
    {
        if (turnService.IsPlayerTurn())
        {
            this.gameObject.SetActive(true);
            colores = new List<Color[]>();
            //textos = new List<string>();
            sprites = new List<Sprite>();


            
            inventoryService.ramdomizeList();

            rouletteService.ChangeSpeed(SpinSpeed);
            
            List<Ficha> listaFichas = inventoryService.getListaFichas();
            numeroSegmentos = listaFichas.Count;

            //para cada ficha sacamos su color y color secundario

            FindAnyObjectByType<Wheel_Manager>().Generate(numeroSegmentos, listaFichas);
        }
       
    }
}
