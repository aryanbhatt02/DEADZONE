using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class ZombieAI : MonoBehaviour
{
    public IObjectPool<ZombieAI> pool;

    public Transform target;
    public bool runner;
    public CampArea homeArea;

    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 6f;

    public int rewardMoney = 50;
    public int addKillCount = 1;

    [Header("Health")]
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth = 100;
    private bool dead;

    [Header("Combat")]
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float hitDuration = 0.4f;

    [Header("Audio")]
    [SerializeField] AudioClip idleSfx;
    [SerializeField] AudioClip deathSfx;
    private AudioSource source;

    private float attackTime = 1f;

    private NavMeshAgent agent;
    private bool hit;

    Animator anim;

    private void Awake()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
        

        source = GetComponent<AudioSource>();                
    }
    public void SetupZombie(bool isRunner, CampArea area, Transform newTarget)
    {
        runner = isRunner;
        homeArea = area;
        target = newTarget;

        currentHealth = maxHealth;
        dead = false;
        hit = false;

        attackTime = Time.time + attackCooldown;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.enabled = true;

        agent.isStopped = false;
        agent.ResetPath();
        agent.speed = runner ? runSpeed : walkSpeed;
        agent.stoppingDistance = attackRange;
        agent.SetDestination(target.position);

        anim.Rebind();
        anim.Update(0f);
        anim.SetBool("Death", false);
        anim.SetBool("Run", runner);
        anim.SetFloat("Speed", 0f);

        source.Stop();
        source.clip = idleSfx;
        source.loop = true;
        source.volume = 1f;
        source.Play();
    }
    void Update()
    {
        if (dead) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if(distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            agent.speed = runner ? runSpeed : walkSpeed;
        }
        else
        {
            agent.isStopped = true;
            TryAttack();

        }

        anim.SetFloat("Speed", agent.velocity.magnitude);
        anim.SetBool("Run", runner);

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;

        source.Stop();
        source.PlayOneShot(deathSfx);

        agent.ResetPath();
        agent.speed = 0f;
        currentHealth = 0;
        MoneyManager.instance.AddMoney(rewardMoney);
        KillCount.instance.PlusKillCount(addKillCount);
        GetComponent<CapsuleCollider>().enabled = false;
        anim.SetBool("Death", true);        
        StartCoroutine(ReturnToPool());
    }

    IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(5f);
        pool.Release(this);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            anim.SetTrigger("hit");
            StartCoroutine(HitFunction());
        }
    }

    public void TryAttack()
    {
        if (hit || dead) return;
        if (Time.time >= attackTime)
        {
            anim.SetTrigger("attack");
            attackTime = Time.time + attackCooldown;
        }
    }

    public void Damage()
    {
        Debug.Log("Damaged");
        if (target.TryGetComponent(out AreaTarget area))
        {
            Debug.Log("Damaging fence");
            area.DamageArea(Random.Range(20, 30));
        }
        else if (target.TryGetComponent(out PlayerHealthUI player))
        {
            Debug.Log("Damaging Player");
            player.TakeDamage(Random.Range(20, 30));
        }
        else
        {
            Debug.Log("Target has no AreaTarget or PlayerHealthUI");
        }
    }

    IEnumerator HitFunction()
    {
        hit = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(hitDuration);
        agent.isStopped = false;
        hit = false;
    }

}
