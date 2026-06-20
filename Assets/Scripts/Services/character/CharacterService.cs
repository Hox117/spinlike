
using UnityEngine;
public class CharacterService : ICharacterService
{
    int life;

    public CharacterService() { 
        life = 10;
    }
    public void resetPlayer() { }
    public void heal(int value)
    {
        life += value;
    }

    public void takeDamege(int value)
    {
       life -= value;
        if (life <= 0) {
            Debug.Log("el jugador a muerto");
        }
    }
}
