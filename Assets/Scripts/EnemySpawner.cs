using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnerParent;
    public GameObject enemyPrefab;

    public int maxEnemies = 10;
    public float spawnCooldown = 3f;
    public static int enemyCount = 0;

    private float currentCooldown = 0f;

    // Update is called once per frame
    void Update()
    {
        if(currentCooldown <= 0)
        {
            currentCooldown = spawnCooldown;
            //spawn enemy
            if(enemyCount < maxEnemies)
            {
                Instantiate(enemyPrefab, transform.GetChild(Random.Range(0, transform.childCount)));
                enemyCount++;
            }
        }
        currentCooldown -= Time.deltaTime;
    }
}
