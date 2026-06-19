using System.Collections.Generic;
using UnityEngine;

public interface IInventoryService
{
    public List<Ficha> getListaFichas();
    public void cargarInventario(List<Ficha> listaCompletaFichas);
    public void AddFicha(Ficha ficha);
    public void removeFicha(Ficha ficha);
    public bool CheckFicha(Ficha ficha);

}
