using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IEnemyService
{
    List<GameObject> enemiesList;
    ITurnService turnService;
    public void enemyRemaining()
    {
        if(turnService == null) turnService = AppContainer.Get<ITurnService>();
        bool SiguienteEnemigo = false;

        foreach (GameObject enemyGO in enemiesList)
        {
            EnemyBase enemy = enemyGO.GetComponent<EnemyBase>();

            if (!enemy.isTurnEnded) SiguienteEnemigo = true;
        }
        if (!SiguienteEnemigo) turnService.ChangeTurn();
    }

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

    public void resetTurns()
    {
        foreach (GameObject enemyGO in enemiesList)
        {
            enemyGO.GetComponent<EnemyBase>().isTurnEnded = false;
        }
    }
    public void endTurn(GameObject enemy)
    {
        GameObject enemyFound = enemiesList.Find(x => x.gameObject.Equals( enemy));
        EnemyBase enemyFoundEnemyBase = enemyFound.GetComponent<EnemyBase>();
        enemyFoundEnemyBase.isTurnEnded = true;
        enemyRemaining();
    }
    public void setEnemyList(List<GameObject> listaDeEnemigos)
    {
        enemiesList = listaDeEnemigos;
    }
}
