using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHittable
{
    [SerializeField]Enemy enemyData;

    int life;
    int shield;
    int AttackMod;
    bool dead;

    public bool isTurnEnded = false;

    Animator animator;

    ICharacterService characterService;
    IEnemyService enemyService;
    ITurnService turnService;
    IEventService eventService;
    ISceneService sceneService;
    void Awake()
    {
        enemyService = AppContainer.Get<IEnemyService>();
        eventService = AppContainer.Get<IEventService>();
        turnService = AppContainer.Get<ITurnService>();
        characterService = AppContainer.Get<ICharacterService>();
        sceneService = AppContainer.Get<ISceneService>();

    }
    void Start()
    {
        life = enemyData.Life;
        shield = enemyData.Shield;
        AttackMod = enemyData.attackMod;
        animator = GetComponent<Animator>();
        
        eventService.Subscribe<TurnChangeEvent>(TakeAction);

    }

    private void TakeAction(GameEventBase game = null)
    {
        if(turnService.IsPlayerTurn())return;
 
        StartCoroutine(ExecuteTurn());

    }

    

    private IEnumerator ExecuteTurn()
    {
        //TODO Revisar esto si da tiempo 100% se puede mejorar me he metido una fumada buena
        yield return new WaitForSeconds(2f);
        bool turnoEnemigoListo = false;
        bool SiguienteEnemigo = false;

        List<GameObject> listaEnemigos = enemyService.getEnemyList();

        characterService.takeDamage(1);
        Debug.Log("El jugador recibe 1 de daño");

        for(int i = 0; i < listaEnemigos.Count ; i++)
        {
            EnemyBase enemy = listaEnemigos[i].GetComponent<EnemyBase>();
            
            if (enemy != null)
            {
                if(!enemy.isTurnEnded)
                {
                    enemy.isTurnEnded = true;
                    listaEnemigos[i] = enemy.gameObject;
                    break;
                }
            }
        }

        enemyService.setEnemyList(listaEnemigos);

        foreach (GameObject enemyGO in listaEnemigos)
        {
            EnemyBase enemy = enemyGO.GetComponent<EnemyBase>();

            if(!enemy.isTurnEnded)SiguienteEnemigo = true;
        }
        if(!SiguienteEnemigo)turnoEnemigoListo = true;
        if(turnoEnemigoListo)turnService.ChangeTurn();    
    }


    public void OnHit(int damage)
    {
        //TODO: quitarle primero da�o al escudo si hay
        life -= damage;
        if (life <= 0) {
            Die();
            return;
        }
        Debug.Log("me dañaste, me quedan " + life );

    }

    private void Die()
    {
        dead = true;

        animator.SetBool("isDead", true);
        Debug.Log("me Mori");
        enemyService.removeFirstEnemy();
        eventService.Unsubscribe<TurnChangeEvent>(TakeAction);

        if(enemyService.getEnemyList().Count <= 0) StartCoroutine(RewardChange());

        StartCoroutine("Disappear");
    }

    public IEnumerator RewardChange()
    {
        yield return new WaitForSeconds(2f);
        sceneService.LoadScene(SceneNames.RewardScene);
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
