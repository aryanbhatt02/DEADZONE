using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("Basic Game Sound")]
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip buttonSfx;
    [SerializeField] AudioClip zombieBgScreams;
    [SerializeField] AudioClip purchasedSfx;
    [SerializeField] AudioClip saveGameSfx;
    [SerializeField] AudioClip invalidSfx;
    [SerializeField] AudioClip waveCountdownSfx;


    [Header("Weapons")]
    [SerializeField] AudioClip emptyGunSfx;
    //[SerializeField] AudioClip gunSwitchSfx;

    [Header("Gate")]
    [SerializeField] AudioClip gateRepairingMusic;
    [SerializeField] AudioClip completedSfx;
    [SerializeField] AudioClip gateBreakSfx;    

    [Header("Health")]
    [SerializeField] AudioClip healingMusic;
    [SerializeField] AudioClip healSuccessSfx;

    [Header("Zombies")]
    [SerializeField] AudioClip zombieIdleSound;
    [SerializeField] AudioClip zombieDeathSfx;

    [Header("Player")]
    [SerializeField] AudioClip playerHitSfx;
    [SerializeField] AudioClip playerDeathSfx;
    [SerializeField] AudioClip playerWalkSfx;
    [SerializeField] AudioClip playerRunSfx;

    private AudioSource musicSource;
    private AudioSource uiSource;
    private AudioSource zombieScreamsSource;
    private AudioSource footStepSource;    
    private AudioSource repairSource;
    private AudioSource healSource;
 

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        AudioSource[] source = GetComponents<AudioSource>();
        musicSource = source[0];
        uiSource = source[1];
        zombieScreamsSource = source[2];
        footStepSource = source[3];
        repairSource = source[4];
        healSource = source[5];

        SceneManager.sceneLoaded += OnSceneLoaded;

        Application.targetFrameRate = 60;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu")
        {
            PlayMainMenuMusic();
            PlayZombieScreams();
        }
        else
        {
            musicSource.Stop();
            zombieScreamsSource.Stop();
        }
    }

    public void PlayMainMenuMusic()
    {        
        musicSource.clip = mainMenuMusic;
        musicSource.playOnAwake = true;
        musicSource.loop = true;
        musicSource.volume = 0.1f;
        musicSource.Play();
    }

    public void PlayZombieScreams()
    {        
        zombieScreamsSource.clip = zombieBgScreams;
        zombieScreamsSource.playOnAwake = true;
        zombieScreamsSource.loop = true;
        zombieScreamsSource.volume = 0.07f;
        zombieScreamsSource.Play();
    }

    public void PlayButtonSound()
    {
        uiSource.volume = 2f;
        uiSource.PlayOneShot(buttonSfx);
    }

    public void PlayPurchasedSound()
    {
        uiSource.volume = 2f;
        uiSource.PlayOneShot(purchasedSfx);
    }

    public void PlaySaveGameSound()
    {
        uiSource.volume = 2f;
        uiSource.PlayOneShot(saveGameSfx);
    }

    public void PlayInvalidSound()
    {
        if (uiSource.isPlaying) return;
        uiSource.volume = 0.5f;
        uiSource.PlayOneShot(invalidSfx);
    }

    public void PlayEmptyGunSound()
    {
        if (uiSource.isPlaying) return;
        uiSource.volume = 2f;
        uiSource.PlayOneShot(emptyGunSfx);
    }

    public void PlayGateRepairingSound()
    {
        repairSource.clip = gateRepairingMusic;
        repairSource.loop = true;
        repairSource.volume = 1f;
        repairSource.Play();
    }
    public void StopGateRepairingSound()
    {
        repairSource.clip = gateRepairingMusic;
        repairSource.loop = false;
        repairSource.volume = 1f;
        repairSource.Stop();
    }

    public void PlayCompletedSound()
    {
        uiSource.volume = 1f;
        uiSource.PlayOneShot(completedSfx);
    }

    public void PlayGateBreakSound()
    {
        uiSource.volume = 1f;
        uiSource.PlayOneShot(gateBreakSfx);
    }    

    public void PlayPlayerHitSound()
    {
        if (uiSource.isPlaying) return;

        uiSource.volume = 2f;
        uiSource.PlayOneShot(playerHitSfx);
    }

    public void PlayPlayerWalkSound()
    {
        if (footStepSource.clip == playerWalkSfx && footStepSource.isPlaying) return;

        footStepSource.clip = playerWalkSfx;
        footStepSource.loop = true;
        footStepSource.volume = 1f;
        footStepSource.Play();
    }
    public void PlayPlayerRunSound()
    {
        if (footStepSource.clip == playerRunSfx && footStepSource.isPlaying) return;

        footStepSource.clip = playerRunSfx;
        footStepSource.loop = true;
        footStepSource.volume = 1f;
        footStepSource.Play();
    }
    public void StopFootStepSound()
    {        
        footStepSource.Stop();
        footStepSource.clip = null;
    }
    public void PlayPlayerDeathSound()
    {        
        uiSource.volume = 1f;
        uiSource.PlayOneShot(playerDeathSfx);
    }

    public void PlayHealingSound()
    {
        healSource.clip = healingMusic;
        healSource.volume = 1f;
        healSource.loop = true;
        healSource.Play();
    }
    public void StopHealingSound()
    {
        healSource.clip = null;
        healSource.Stop();
    }

    public void PlayHealSuccessSound()
    {
        uiSource.volume = 1f;
        uiSource.PlayOneShot(healSuccessSfx);
    }

    public void PlayWaveCountdownSound()
    {
        uiSource.volume = 0.7f;
        uiSource.PlayOneShot(waveCountdownSfx);
    }
}
