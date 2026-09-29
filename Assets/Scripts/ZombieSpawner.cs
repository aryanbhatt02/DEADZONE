using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ZombieSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] zombiePrefab;
    [SerializeField] Transform[] spawnPoints;

    public CampArea connectedArea;

    private ObjectPool<ZombieAI>[] zombiePools;

    private void Awake()
    {
        zombiePools = new ObjectPool<ZombieAI>[zombiePrefab.Length];

        for (int i = 0; i < zombiePools.Length; i++)
        {
            int index = i;

            zombiePools[index] = new ObjectPool<ZombieAI>(() => CreateZombie(index, zombiePools[index]),
                OnGetZombie, OnReleaseZombie, OnDestroyZombie, true, 5, 30);
        }
    }

    private ZombieAI CreateZombie(int index, ObjectPool<ZombieAI> pool)
    {
        GameObject zombieObject = Instantiate(zombiePrefab[index]);

        ZombieAI zombie = zombieObject.GetComponent<ZombieAI>();
        zombie.pool = pool;
        zombieObject.SetActive(false);
        return zombie;
    }

    private void OnGetZombie(ZombieAI zombie)
    {

    }
    private void OnReleaseZombie(ZombieAI zombie)
    {
        zombie.gameObject.SetActive(false);
    }
    private void OnDestroyZombie(ZombieAI zombie)
    {
        Destroy(zombie.gameObject);
    }

    public void SpawnZombie()
    {
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int zombieType = Random.Range(0, zombiePrefab.Length);

        ZombieAI zombie = zombiePools[zombieType].Get();
        zombie.transform.SetPositionAndRotation(randomSpawn.position, randomSpawn.rotation);       

        float runnerProb = Random.Range(0f, 1f);

        bool runner = false;

        if(runnerProb > 0.8f)
        {
            runner = true;
        }

        Transform target;

        if (connectedArea.destroyed)
        {
            target = connectedArea.targetPlayer;
        }
        else
        {
            AreaTarget randomTarget = connectedArea.areaTargets[Random.Range(0, connectedArea.areaTargets.Length)];

            target = randomTarget.transform;
        }

        zombie.gameObject.SetActive(true);

        zombie.SetupZombie(runner, connectedArea, target);
    }
 
}
