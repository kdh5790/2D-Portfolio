using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public float spawnInterval;


    public int maxCount;


    void Start()
    {
        for (int i = 0; i < maxCount; i++)
        {
            Instantiate(enemy, transform.position, Quaternion.identity, transform);
        }
        StartCoroutine(SpawnEnemy(spawnInterval, enemy));
    }

    IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        if(transform.childCount < maxCount)
        {
            Instantiate(enemy, transform.position, Quaternion.identity, transform);
        }
        StartCoroutine(SpawnEnemy(spawnInterval, enemy));
    }
}
