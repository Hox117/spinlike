
using UnityEngine;
public class CharacterService : ICharacterService
{
    int life;
    int shield;
    public CharacterService() { 
        life = 10;
        shield = 0;
    }
    public void resetPlayer() { }
    public void heal(int value)
    {
        life += value;
    }

    public void takeDamage(int value)
    {//TODO: quitarle primero daño al escudo si hay
        life -= value;
        if (life <= 0) {
            Debug.Log("el jugador a muerto");
        }
    }
    public void addShield(int value) { 
        shield+=value;
    }
}
