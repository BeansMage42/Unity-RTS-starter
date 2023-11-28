using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject Monster;
    private Monster spawnedMinion;
    private Commander commander;
    private float lastSpawnTime;
    private float currentTime;
    private WaitForSeconds waitTime;

    private void Awake()
    {
        commander = GameObject.Find("Commander").GetComponent<Commander>() ;
        waitTime = new WaitForSeconds(spawnTime);
    }

    private void Start()
    {
        /*spawnedMinion = Instantiate(Monster, transform.position, Quaternion.identity).GetComponent<Monster>();
        commander.AddMinion(spawnedMinion);*/
        StartCoroutine("SpawnLoop");
    }

   private IEnumerator SpawnLoop()
    {
        yield return waitTime;
        Spawn();

    }

    private void Spawn()
    {
        spawnedMinion = Instantiate(Monster, transform.position, Quaternion.identity).GetComponent<Monster>();
        commander.AddMinion(spawnedMinion);

        StartCoroutine("SpawnLoop");
    }
}
