using Unity.VisualScripting;
using UnityEngine;

public class SegmentController : MonoBehaviour ,ISelectionable
{
    Ficha ficha;
    private bool _isSelected = true;
    public void OnSelected()
    {
        //Esto se regenera al volver a lanzar por lo que vuelve a ser true al regenerarse
        if (_isSelected)
        {
            Debug.Log("Gambleame esta " + ficha.id);
            _isSelected = false;
        }
        
    }

    public void addFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }
}
