using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject lichEnemyPrefab;
    [SerializeField] private GameObject fieryEnemyPrefab;
    private float randomWaitTime;


    void Start()
    {
        randomWaitTime = Random.Range(10, 15);
        StartCoroutine(SpawnAtRandomPoint());
    }

    IEnumerator SpawnAtRandomPoint()
    {
        yield return new WaitForSeconds(randomWaitTime);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyType = Random.Range(0, 4);
        if (randomEnemyType <= 2)
        {
            Instantiate(fieryEnemyPrefab, spawnPoints[randomSpawnPointIndex].position, Quaternion.identity).GetComponent<IEnemy>().TriggerBattle(player);
        }
        else
        {
            Instantiate(lichEnemyPrefab, spawnPoints[randomSpawnPointIndex].position, Quaternion.identity).GetComponent<IEnemy>().TriggerBattle(player);
        }
        randomWaitTime = Random.Range(20, 25);
        StartCoroutine(SpawnAtRandomPoint());
    }
}
