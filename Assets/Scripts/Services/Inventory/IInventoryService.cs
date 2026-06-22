using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryService
{
    public List<Ficha> getListaFichas();
    public void cargarInventario(List<FichaData> listaCompletaFichas);
    public void AddFicha(FichaData ficha);
    public void AddFicha(Ficha ficha);
    public void removeFicha(Ficha ficha);
    public void removeAllFicha();
    public void AddPotion(PotionData potion);
    public void RemovePotion(int index);
    public bool IsPotionsFull();
    public void UpdateFicha(FichaData ficha, int index);   
    public PotionData GetPotion(int index);
    public void ramdomizeList();
}
