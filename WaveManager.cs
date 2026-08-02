using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Spawner")]
    public ZombieSpawner[] Spawners;
    public ZombieSpawner selectedSpawner;

    [Header("Wave Settings")]
    public int currentWave = 1;
    [SerializeField] int zombiePerWave = 20;
    [SerializeField] float spawnDelay = 2f;
    [SerializeField] float waveWaitTime = 20f;
    [SerializeField] GameObject gameStartText;
    public GameObject waveBtwText;
    [SerializeField] TMP_Text waveCountDownText;

    public bool spawning;
    
    
    void Start()
    {         
        StartCoroutine(StartWave()); 
    }

    
    IEnumerator StartWave()
    {
        gameStartText.SetActive(true);
        waveBtwText.SetActive(false);
        waveCountDownText.gameObject.SetActive(false);
        yield return new WaitForSeconds(7f);
        gameStartText.SetActive(false);


        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            spawning = true;            

            for(int i = 0; i < zombiePerWave; i++)
            {
                selectedSpawner = Spawners[Random.Range(0, Spawners.Length)];
                selectedSpawner.SpawnZombie();

                yield return new WaitForSeconds(spawnDelay);
            }

            while (FindObjectsOfType<ZombieAI>().Length > 0)
            {
                yield return null;
            }
            waveBtwText.SetActive(true);
            waveCountDownText.gameObject.SetActive(true);
            for(int i = 20;i >= 0; i--)
            {
                waveCountDownText.text = $"Wave {currentWave + 1} coming in {i.ToString()}";
                AudioManager.instance.PlayWaveCountdownSound();
                yield return new WaitForSeconds(1f);
            }
            waveBtwText.SetActive(false);
            waveCountDownText.gameObject.SetActive(false);
            //yield return new WaitForSeconds(20f);
            currentWave++;
            selectedSpawner = null;
            spawning = false;
            zombiePerWave += 20;

        }        
    }

}
