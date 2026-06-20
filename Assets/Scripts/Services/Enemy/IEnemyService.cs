using System.Collections.Generic;
using UnityEngine;

public interface IEnemyService
{
    public List<GameObject> getEnemyList();
    public EnemyBase getFirstEnemy();
    public void setEnemyList(List<GameObject> listaDeEnemigos);
    public void removeFirstEnemy();

}
