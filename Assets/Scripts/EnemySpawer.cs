using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;

    public List<GameObject> ActiveEnemies;
    public float space = 2f;

    private IEnemyService enemyService;
    void Awake()
    {
        enemyService = AppContainer.Get<IEnemyService>();
       


    }
    private void Start()
    {
        Vector3 posicionSpawn = transform.position;
        if (enemyService.getEnemyList() != null && enemyService.getEnemyList().Count >0)
        {
            foreach (GameObject enemy in enemyService.getEnemyList())
            {
                ActiveEnemies.Add(Instantiate(enemy, posicionSpawn, Quaternion.identity, transform));
                posicionSpawn += Vector3.right * space;
            }
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                ActiveEnemies.Add(Instantiate(enemy, posicionSpawn, Quaternion.identity, transform));
                posicionSpawn += Vector3.right * space;
            }
        }
        enemyService.setEnemyList(ActiveEnemies);

    
}

}