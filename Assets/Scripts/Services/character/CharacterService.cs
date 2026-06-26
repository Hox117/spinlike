
using System;
using UnityEngine;
public class CharacterService : ICharacterService
{
    int life;
    int shield;
    IEventService eventService;
    public Guid guid;
    IBuffService buffService;
    IAudioService audioService;
    ISceneService sceneService;
    public CharacterService() { 
        life = 10;
        shield = 0;
        eventService = AppContainer.Get<IEventService>();
        buffService = AppContainer.Get<IBuffService>();
        audioService = AppContainer.Get<IAudioService>();
        guid = Guid.NewGuid();
        
        sceneService = AppContainer.Get<ISceneService>();
    }
    public void resetPlayer() 
    {
        life = 10;
        shield = 0;
    }
    public void heal(int value)
    {
        life += value;
        UpdatePlayerUI updateEvent = new UpdatePlayerUI();
        eventService.Publish(updateEvent);
        Debug.Log(life);
    }

    public void takeDamage(int value)
    {//TODO: quitarle primero da�o al escudo si hay
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
            sceneService.LoadScene(SceneNames.GameOver);
        }
    }

    public void die()
    {
        AudioClip sound = Resources.Load<AudioClip>("Audios/game over sound");
        audioService.PlaySound(sound);
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
