using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryService : IInventoryService
{
    public List<Ficha> listaFichas = new List<Ficha>();
    public PotionData[] potionList= new PotionData[3];

    public void AddFicha(FichaData ficha)
    {
        Ficha fichanueva = new Ficha(ficha);
        listaFichas.Add(fichanueva);
    }

    public void AddFicha(Ficha ficha)
    {
        listaFichas.Add(ficha);
    }

    public void cargarInventario(List<FichaData> listaCompletaFichas)
    {
        foreach (FichaData data in listaCompletaFichas)
        {
            AddFicha(data);
        }
    }
    public void cargarInventario(List<Ficha> listaCompletaFichas)
    {
        foreach (Ficha data in listaCompletaFichas)
        {
            AddFicha(data);
        }
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

    public void AddPotion(PotionData potion)
    {
        for (int i = 0; i < potionList.Length; i++)
        {
            if (potionList[i] == null)
            {
                potionList[i] = potion;
                return;
            }
        }
    }
    public PotionData GetPotion(int slot)
    {
        return potionList[slot];
    }


    public void RemovePotion(int index)
    {
        potionList[index]=null;
    }

    public bool IsPotionsFull()
    {
        bool full = true;
        for (int i = 0; i < potionList.Length; i++) {
            if (potionList[i] == null)
            {
                full = false; break;
            }
        }
        return full;

    }

    public void UpdateFicha(FichaData ficha, int index)
    {
        listaFichas[index] = new Ficha(ficha);
        
    }
}
