using UnityEngine;

public class BotonDañarJugador : MonoBehaviour
{
  ICharacterService characterService;
    private void Start()
    {
        characterService = AppContainer.Get<ICharacterService>();   
    }
    public void recibirDaño() {
        characterService.takeDamage(1);
    }
}
