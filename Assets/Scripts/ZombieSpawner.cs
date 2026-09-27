using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] zombiePrefab;
    [SerializeField] Transform[] spawnPoints;

    public CampArea connectedArea;


    public void SpawnZombie()
    {
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject zombie = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)], randomSpawn.position, randomSpawn.rotation);

        ZombieAI zombieAI = zombie.GetComponent<ZombieAI>();

        float runnerProb = Random.Range(0f, 1f);
        if(runnerProb > 0.8f)
        {
            zombieAI.runner = true;
        }

        zombieAI.homeArea = connectedArea;

        if (connectedArea.destroyed)
        {
            zombieAI.target = connectedArea.targetPlayer;
        }
        else
        {
            AreaTarget randomTarget = connectedArea.areaTargets[Random.Range(0, connectedArea.areaTargets.Length)];

            zombieAI.target = randomTarget.transform;
        } 
        
    }
 
}
