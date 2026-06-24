using UnityEngine;

public class MapTile : MonoBehaviour
{
    private MapTypess tileType;
    public void SetType(MapTypess tipoAPoner)
    {
        tileType = tipoAPoner;
    }

    public void Execute()
    {
            //logica en la que mediante un switch se establece el codigo
            Debug.Log(tileType.ToString());
    }

    public MapTypess getTileType()
    {
        return tileType;
    }
}
