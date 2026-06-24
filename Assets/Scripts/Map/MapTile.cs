using UnityEngine;

public class MapTile : MonoBehaviour
{
    private MapTypess tileType;
    private ISceneService sceneService;
    private ICharacterService characterService;
    private void Start()
    {
        sceneService = AppContainer.Get<ISceneService>();
        characterService = AppContainer.Get<ICharacterService>();
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
            case MapTypess.Damage:
                damage();
                break;
            default:
                combat();
                break;
        }
    }

    private void damage()
    {
        characterService.takeDamage(1);
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
