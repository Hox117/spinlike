using Unity.VisualScripting;
using UnityEngine;

public class SegmentController : MonoBehaviour ,ISelectionable
{
    Ficha ficha;
    public void OnSelected()
    {
        Debug.Log("Gambleame esta " + ficha.id);
    }

    public void addFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }
}
