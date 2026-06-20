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


    void Start()
    {
        life = enemyData.Life;
        shield = enemyData.Shield;
        AttackMod = enemyData.attackMod;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnHit(int damage)
    {
        life -= damage;
        if (life <= 0) {
            Die();
            return;
        }
        Debug.Log("me dañaste, me quedan "+life+ "diablos");

    }

    private void Die()
    {
        dead = true;
        animator.SetBool("isDead", true);
        Debug.Log("me Mori");
        StartCoroutine("Disappear");
    }
    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
