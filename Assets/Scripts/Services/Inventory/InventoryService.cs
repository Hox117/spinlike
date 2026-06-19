using System.Collections.Generic;
using UnityEngine;

public class InventoryService : IInventoryService
{
    public List<Ficha> listaFichas;

    public void AddFicha(Ficha ficha)
    {
        listaFichas.Add(ficha);
    }

    public void cargarInventario(List<Ficha> listaCompletaFichas)
    {
        listaFichas = listaCompletaFichas;
    }

    public bool CheckFicha(Ficha ficha)
    {
        if(listaFichas.Contains(ficha))return true;
        else return false;
    }

    public List<Ficha> getListaFichas()
    {
        return listaFichas;
    }

    public void removeFicha(Ficha ficha)
    {
        listaFichas.Remove(ficha);
    }
}
