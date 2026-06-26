using UnityEngine;

public class PlayAgainButton : MonoBehaviour
{
    ICharacterService characterService;
    IMapService mapService;
    void Start()
    {
        characterService = AppContainer.Get<ICharacterService>();
    }
    public void PlayAgain()
    {
        characterService.resetPlayer();
        mapService.ResetMap();
        mapService.SetPositionPlayer(1, 1);
    }


}
