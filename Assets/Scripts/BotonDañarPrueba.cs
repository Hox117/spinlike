using UnityEngine;

public class BotonDañarPrueba: MonoBehaviour {
    [SerializeField] GameObject EnemyList;
    public void dañarEnemigo() {
       EnemyBase enemy = EnemyList.transform.GetChild(0).GetComponent<EnemyBase>();
        enemy.OnHit(1);
    }
    
}
