using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CampArea : MonoBehaviour

{
    public string areaName;
    public float currentHealth;
    public float maxHealth;

    public AreaTarget[] areaTargets;

    [SerializeField] PlayerHealthUI playerHealth;    

    public Transform targetPlayer;

    [SerializeField] GameObject fenceExplosionEffect;
    public GameObject fenceBorder;
    public GameObject fence;

    public bool destroyed;

    public NavMeshObstacle fenceObstacle;

    [Header("Audio")]
    [SerializeField] AudioClip gateHitSfx;
    private AudioSource source;

    void Start()
    {
        currentHealth = maxHealth;

        source = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public void DamageArea(float damage)
    {
        if (destroyed) return;
        currentHealth -= damage;
        PlayGateHitSound();
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);        

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlayGateBreakSound();
            destroyed = true;
            Instantiate(fenceExplosionEffect, transform.position, Quaternion.identity);

            //Destroy(fence);
            //Destroy(fenceBorder);

            fence.GetComponent<Collider>().enabled = false;
            fence.GetComponent<MeshRenderer>().enabled = false;
            fenceBorder.GetComponent<Collider>().enabled = false;
            fenceBorder.GetComponent<MeshRenderer>().enabled = false;

            fenceObstacle.enabled = false;

            //Destroy(gameObject);
            Destroyed();
            ChangeZombieTarget();
        }
    }

    public void Destroyed()
    {
        //Destroy Logic
        Debug.Log($"{areaName} Destroyed");
    }

    private void ChangeZombieTarget()
    {
        Debug.Log("Changing Zombie Target");
        ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();

        foreach(ZombieAI zombie in zombies)
        {
            if (zombie.homeArea == this)
            {
                zombie.target = targetPlayer;
            }            
        }
    }

    public void LoadDestroyedState()
    {
        if (currentHealth <= 0)
        {
            destroyed = true;

            fence.GetComponent<Collider>().enabled = false;
            fence.GetComponent<MeshRenderer>().enabled = false;

            fenceBorder.GetComponent<Collider>().enabled = false;
            fenceBorder.GetComponent<MeshRenderer>().enabled = false;

            ChangeZombieTarget();
        }
    }

    private void PlayGateHitSound()
    {
        if (source.isPlaying) return;
        source.volume = 0.5f;
        source.PlayOneShot(gateHitSfx);

    }
}
