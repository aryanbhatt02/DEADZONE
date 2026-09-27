using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Bullet Fire Settings")]
    [SerializeField] float bulletRange = 100f;
    [SerializeField] float fireRate = 0.5f;
    float nextFireTime = 0f;

    [Header("Basic Weapon Settings")]
    public int currentBullets = 0;
    public int bulletsInMag = 6;
    public int totalBullets = 30;
    public int maxBullets = 90;
    [SerializeField] int weaponDamage = 25;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip magIn;
    [SerializeField] AudioClip magOut;
    [SerializeField] Transform playerCamera;

    public bool isUnloacked = false;
    public bool reloding = false;

    [Header("Hit Effects")]
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject normalHitEffect;

    Animator animator;
    [SerializeField] PlayerController playerController;

    [SerializeField] LayerMask shootingMask;

    private bool aiming;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = transform.parent.root.GetComponent<PlayerController>();
        currentBullets = bulletsInMag;
        //InputManager.Instance.playerInput.Player.Aim.performed += HandleAim;
        //InputManager.Instance.playerInput.Player.Aim.canceled += HandleAim;
    }

    
    void Update()
    {
        if (InputManager.Instance.firing)
        {
            if (!playerController.IsRunning)
            {
                if(InputManager.Instance.firing && currentBullets <= 0)
                {
                    AudioManager.instance.PlayEmptyGunSound();
                }
                if (Time.time >= nextFireTime && currentBullets > 0 && !reloding)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
            
        }

        

        animator.SetBool("Walk", playerController.IsWalking);
        animator.SetBool("Run", playerController.IsRunning);

        //aiming = InputManager.Instance.playerInput.Player.Aim.IsPressed();
        //animator.SetBool("Aim", aiming);        
    }

    public void WeaponReload()
    {
        if (!reloding && totalBullets > 0 && currentBullets < bulletsInMag)
        {
            reloding = true;
            animator.SetTrigger("Reload");
        }       
    }

    public void AddBullets()
    {
        reloding = false;
        int bulletsToReload = (bulletsInMag - currentBullets);
        if(bulletsToReload <= totalBullets)
        {
            currentBullets = bulletsInMag;
            totalBullets -= bulletsToReload;
        }
        else
        {
            currentBullets += totalBullets;
            totalBullets = 0;
        }
    }

    public void MagIn()
    {
        source.PlayOneShot(magIn);
    }

    public void MagOut()
    {
        source.PlayOneShot(magOut);
    }

    void Shoot()
    {
        currentBullets--;
        if (muzzleFlash)
        {
            muzzleFlash.Play();
        }
        if (aiming)
        {
            animator.SetTrigger("Aim Shoot");            
        }
        else
        {
            animator.SetTrigger("Shoot");
        }        

        if(source && shootSound)
        {
            source.PlayOneShot(shootSound);
        }

        RaycastHit hit;
        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, bulletRange, shootingMask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            ZombieAI z = hit.collider.GetComponent<ZombieAI>();

            if(z != null)
            {
                Debug.Log("Zombie Hit");
                z.TakeDamage(weaponDamage);
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));                
            }
            else
            {
                Debug.Log("Object Hit");
                Instantiate(normalHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    public void RefillAmmo()
    {
        totalBullets = maxBullets;
        currentBullets = bulletsInMag;
    }

    private void HandleAim(InputAction.CallbackContext context)
    {
        aiming = context.performed;
    }

}
