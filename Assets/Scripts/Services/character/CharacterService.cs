
using System;
using UnityEngine;
public class CharacterService : ICharacterService
{
    int life;
    int shield;
    IEventService eventService;
    public Guid guid;
    IBuffService buffService;
    public CharacterService() { 
        life = 10;
        shield = 0;
        eventService = AppContainer.Get<IEventService>();
        buffService = AppContainer.Get<IBuffService>();
        guid= Guid.NewGuid();
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
       if (shield > value)
        {
            shield -= value;
        }
        else
        {
            life += shield - value;
            shield = 0;
        }
        
        Debug.Log(life);

        UpdatePlayerUI updateEvent = new UpdatePlayerUI();
        eventService.Publish(updateEvent);

        if (life <= 0) {
            Debug.Log("el jugador a muerto");
        }
    }
    public void addShield(int value) { 
       
        shield+=value;
        UpdatePlayerUI updateEvent = new UpdatePlayerUI();
        eventService.Publish(updateEvent);
        Debug.Log("escudo de "+shield+" puntos");  
    }

    public int getLife()
    {
        return life;
    }

    public int getShield()
    {
        return shield;
    }

    public Guid getGuid()
    {
        return guid;
    }

    public Buff getBuff(BuffType bufo)
    {
        return buffService.GetBuff(guid, bufo);
    }
}
