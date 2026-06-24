using UnityEngine;

public class MapTile : MonoBehaviour
{
    private MapTypess tileType;
    private ISceneService sceneService;
    private void Start()
    {
        sceneService = AppContainer.Get<ISceneService>();
    }
    public void SetType(MapTypess tipoAPoner)
    {
        tileType = tipoAPoner;
    }

    public void Execute()
    {
            //logica en la que mediante un switch se establece el codigo
            Debug.Log(tileType.ToString());

        switch (tileType)
        {
            default:
                combat();
                break;
        }
    }

    public MapTypess getTileType()
    {
        return tileType;
    }

    public void combat()
    {
        sceneService.LoadScene(SceneNames.SampleScene);
    }
}
