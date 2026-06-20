using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IEnemyService
{
    List<GameObject> enemiesList;
    public List<GameObject> getEnemyList()
    {
        return enemiesList;
    }

    public EnemyBase getFirstEnemy()
    {
        return enemiesList[0].GetComponent<EnemyBase>();
    }

    public void removeFirstEnemy()
    {
        enemiesList.RemoveAt(0);
    }

    public void setEnemyList(List<GameObject> listaDeEnemigos)
    {
        enemiesList = listaDeEnemigos;
    }
}
