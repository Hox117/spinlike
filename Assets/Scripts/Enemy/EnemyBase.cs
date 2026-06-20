using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHittable
{
    [SerializeField]Enemy enemyData;

    int life;
    int shield;
    int AttackMod;
    bool dead;
    Animator animator;

    IEnemyService enemyService;
    void Start()
    {
        life = enemyData.Life;
        shield = enemyData.Shield;
        AttackMod = enemyData.attackMod;
        animator = GetComponent<Animator>();
        enemyService = AppContainer.Get<IEnemyService>();
    }

    // Update is called once per frame
    void Update()
    {

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
        StartCoroutine("Disappear");
    }
    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
