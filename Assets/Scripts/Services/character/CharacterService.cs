
using UnityEngine;
public class CharacterService : ICharacterService
{
    int life;
    int shield;
    IEventService eventService;
    public CharacterService() { 
        life = 10;
        shield = 0;
        eventService = AppContainer.Get<IEventService>();
    }
    public void resetPlayer() { }
    public void heal(int value)
    {
        life += value;
        UpdatePlayerUI updateEvent = new UpdatePlayerUI();
        eventService.Publish(updateEvent);
        Debug.Log(life);
    }

    public void takeDamage(int value)
    {//TODO: quitarle primero daño al escudo si hay
        life -= value;
        Debug.Log(life);

        UpdatePlayerUI updateEvent = new UpdatePlayerUI();
        eventService.Publish(updateEvent);

        if (life <= 0) {
            Debug.Log("el jugador a muerto");
        }
    }
    public void addShield(int value) { 
        shield+=value;
        Debug.Log("escudo de "+shield+" puntos");  
    }

    public int getLife()
    {
        return life;
    }
   
}
