using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    private MapTypess tileType;
    private ISceneService sceneService;
    private ICharacterService characterService;
    private IEnemyService enemyService;

    [SerializeField] private List<GameObject> Enemies;
    [SerializeField] private List<GameObject> Bosses;
    private void Start()
    {
        sceneService = AppContainer.Get<ISceneService>();
        characterService = AppContainer.Get<ICharacterService>();
        enemyService = AppContainer.Get<IEnemyService>();
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
                case MapTypess.combat:
                setListEnemiesBase(Random.Range(0, 3));
                changeSceneCombat(); 
                break;
            case MapTypess.Reward:
                reward();
                break;
            case MapTypess.Heal:
                Heal();
                break;
            case MapTypess.Elite:
                setListEnemiesBase(Random.Range(0, 4));
                changeSceneCombat();
                break;
            case MapTypess.Boss:
                enemyService.setEnemyList(Bosses);
                changeSceneCombat();
                break;
                
          
        }
    }

    private void damage()
    {
        characterService.takeDamage(Random.Range(1,6));
    }

    public MapTypess getTileType()
    {
        return tileType;
    }
    private void reward()
    {
        sceneService.LoadScene(SceneNames.RewardScene);
    }

    private void changeSceneCombat()
    {
        sceneService.LoadScene(SceneNames.SampleScene);
    }
    private void Heal()
    {
        characterService.heal(Random.Range(1, 10));
    }

    private void setListEnemiesBase(int numberOfEnemies)
    {
        List<GameObject> listadeEnemigos = new List<GameObject>();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            listadeEnemigos.Add(Enemies[Random.Range(0, Enemies.Count )]);
        }
        enemyService.setEnemyList(listadeEnemigos);
    }
}
