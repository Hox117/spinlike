using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public float space = 2f;

    void Start()
    {
        Vector3 posicionSpawn = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Instantiate(enemy, posicionSpawn, Quaternion.identity, transform);
            posicionSpawn += Vector3.right * space;
        }
    }
}