using System.Collections.Generic;
using UnityEngine;

public interface IEnemyService
{
    public List<GameObject> getEnemyList();
    public EnemyBase getFirstEnemy();
    public void setEnemyList(List<GameObject> listaDeEnemigos);
    public void removeFirstEnemy();
    public void endTurn(GameObject enemy);
    public void enemyRemaining();
    public void resetTurns();

}
