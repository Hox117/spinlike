using System.Collections.Generic;
using UnityEngine;

public class RewardInitializer : MonoBehaviour
{
    IInventoryService inventoryService;
    ITurnService turnService;
    [SerializeField] List<FichaData> ListaDeFichas;

    public void setListaDeFichas(List<FichaData> ListaDeFichas)
    {
        this.ListaDeFichas = ListaDeFichas;
    }
    void Awake()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        turnService = AppContainer.Get<ITurnService>();
        
    }

    void Start()
    {
        if(!turnService.IsPlayerTurn())turnService.ChangeTurn();
        
        List<Ficha> listaFichasReales = new List<Ficha>();
        int numeroSegmentos = ListaDeFichas.Count;

        foreach (FichaData fichadata in ListaDeFichas)
        {
            Ficha ficha = new Ficha(fichadata);
            listaFichasReales.Add(ficha);
        }
        FindAnyObjectByType<Wheel_Manager>().Generate(numeroSegmentos, listaFichasReales);
    }
}
