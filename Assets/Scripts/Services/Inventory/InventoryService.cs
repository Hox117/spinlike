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

    public void ramdomizeList()
    {
        System.Random rng = new System.Random();
        int n = listaFichas.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var valor = listaFichas[k];
            listaFichas[k] = listaFichas[n];
            listaFichas[n] = valor;
        }
    }

    public void removeFicha(Ficha ficha)
    {
        listaFichas.Remove(ficha);
    }
}
