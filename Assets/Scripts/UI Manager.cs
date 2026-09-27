using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] WeaponManager weaponManager;

    [SerializeField] CanvasGroup zombieHintGroup;

    [SerializeField] WaveManager waveManager;

    [SerializeField] GameObject playerHealthUi;

    [Header("Bullets")]
    [SerializeField] TMP_Text currentBullets;
    [SerializeField] TMP_Text totalBullets;

    [Header("Money")]
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text moneyAnimText;

    [Header("KillCount")]
    [SerializeField] TMP_Text killCountText;
    [SerializeField] Image killCountImage;

    [Header("Wave Countdown")]
    [SerializeField] TMP_Text waveCountDownText;

    [Header("Colors")]
    [SerializeField] Color fullColor = Color.white;
    [SerializeField] Color emptyColor = Color.red;
    [SerializeField] Color addMoneyColor = Color.green;
    
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject backButton;
    [SerializeField] TMP_Text totalKillText;
    [SerializeField] TMP_Text totalMoneyEarnedText;

    [Header("Paused Menu Panel")]
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject pausedMenuPanel;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject mainMenuButton;

    //[Header("MainMenu Scene")]
    //[SerializeField] GameObject newGameButton;
    //[SerializeField] GameObject 
    
    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        moneyText.text = $"$ {MoneyManager.instance.currentMoney}";
        gameOverPanel.SetActive(false);        
        pausedMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    
    void Update()
    {
        HandleBullets();
        //HandleZombieHint();
    }

    private void HandleBullets()
    {
        currentBullets.text = weaponManager.GetCurrentWeapon().currentBullets.ToString();
        totalBullets.text = weaponManager.GetCurrentWeapon().totalBullets.ToString();

        if(weaponManager.GetCurrentWeapon().currentBullets == weaponManager.GetCurrentWeapon().bulletsInMag)
        {
            currentBullets.color = fullColor;
        }
        else if (weaponManager.GetCurrentWeapon().currentBullets == 0)
        {
            currentBullets.color = emptyColor;
        }
        else
        {
            currentBullets.color = fullColor;
        }

        if(weaponManager.GetCurrentWeapon().totalBullets == 0)
        {
            totalBullets.color = emptyColor;
        }
        else
        {
            totalBullets.color=fullColor;
        }
    }

    //private void HandleZombieHint()
    //{
    //    if (waveManager.spawning)
    //    {
    //        zombieHintGroup.alpha = 1.0f;
    //        zombieHintText.text = $"Zombies coming to the {waveManager.selectedSpawner.connectedArea.areaName}";
    //    }
    //    else
    //    {
    //        zombieHintGroup.alpha = 0f;
    //    }
    //}

    public void AddMoney(int money)
    {
        moneyText.text = $"$ {MoneyManager.instance.currentMoney}";
        moneyAnimText.text = "+" + money.ToString();
        moneyAnimText.color = addMoneyColor;
        Animator anim = moneyAnimText.GetComponent<Animator>();
        anim.Play("add money", 0, 0f);
    }
    public void RemoveMoney(int money)
    {
        moneyText.text = $"$ {MoneyManager.instance.currentMoney}";
        moneyAnimText.text = "-" + money.ToString();
        moneyAnimText.color = emptyColor;
        moneyAnimText.GetComponent<Animator>().Play("remove money");
    }

    public void AddKillCount(int add)
    {
        killCountText.text = $"{KillCount.instance.currentKillCount}";
    }

    public void WaveCountDown()
    {
        for(int i = 0; i < killCountText.text.Length; i++)
        {

        }
    }

    void CheckInputs()
    {

    }

    public void GameOverPanal()
    {               
        gameOverPanel.SetActive(true);
        totalKillText.text = $"{KillCount.instance.currentKillCount}";
        totalMoneyEarnedText.text = $"${MoneyManager.instance.currentMoney}";
        TotalMoneyEarned();
        gameOverPanel.GetComponent<Animator>().Play("GameOverPanal");

        Invoke(nameof(SlowGameTime), 1f);

        killCountText.gameObject.SetActive(false);
        killCountImage.gameObject.SetActive(false);
        moneyText.gameObject.SetActive(false);
        moneyAnimText.gameObject.SetActive(false);
        currentBullets.gameObject.SetActive(false);
        totalBullets.gameObject.SetActive(false);
        waveCountDownText.gameObject.SetActive(false);
        playerHealthUi.gameObject.SetActive(false);

    }

    public void RestartGame()
    {
        AudioManager.instance.PlayButtonSound();

        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
        StartCoroutine(SceneFader.instance.StartNewGameFadeOut());
        killCountText.gameObject.SetActive(true);
        killCountImage.gameObject.SetActive(true);
        moneyText.gameObject.SetActive(true);
        moneyAnimText.gameObject.SetActive(true);
        currentBullets.gameObject.SetActive(true);
        totalBullets.gameObject.SetActive(true);        
        playerHealthUi.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        AudioManager.instance.PlayButtonSound();

        Time.timeScale = 1f;
        StartCoroutine(SceneFader.instance.MainMenuFadeOut());
    }

    private void TotalKills(int add)
    {
        totalKillText.text = $"{KillCount.instance.currentKillCount}";
    }

    private void TotalMoneyEarned()
    {
        totalMoneyEarnedText.text = $"${MoneyManager.instance.currentMoney}";
    }

    public void PauseGame()
    {
        AudioManager.instance.PlayButtonSound();        

        pausedMenuPanel.SetActive(true);
        pausedMenuPanel.GetComponent<Animator>().Play("PauseMenu");

        Invoke(nameof(PauseGameTime), 0.6f);

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        pauseButton.SetActive(false);
        killCountText.gameObject.SetActive(false);
        killCountImage.gameObject.SetActive(false);
        moneyText.gameObject.SetActive(false);
        moneyAnimText.gameObject.SetActive(false);
        currentBullets.gameObject.SetActive(false);
        totalBullets.gameObject.SetActive(false);
        waveCountDownText.gameObject.SetActive(false);
        playerHealthUi.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        AudioManager.instance.PlayButtonSound();

        pausedMenuPanel.SetActive(false);
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        pauseButton.SetActive(true);
        killCountText.gameObject.SetActive(true);
        killCountImage.gameObject.SetActive(true);
        moneyText.gameObject.SetActive(true);
        moneyAnimText.gameObject.SetActive(true);
        currentBullets.gameObject.SetActive(true);
        totalBullets.gameObject.SetActive(true);
        playerHealthUi.gameObject.SetActive(true);
    }
    
    private void PauseGameTime()
    {        

        Time.timeScale = 0f;
    }

    private void SlowGameTime()
    {
        Time.timeScale = 0.2f;
    }

    public void NewGame()
    {
        AudioManager.instance.PlayButtonSound();

        Time.timeScale = 1f;

        SaveManager.shouldLoadGame = false;

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

        StartCoroutine(SceneFader.instance.StartNewGameFadeOut());
    }

    public void LoadGame()
    {
        AudioManager.instance.PlayButtonSound();
        SaveManager.shouldLoadGame = true;
        Time.timeScale = 1f;
        StartCoroutine(SceneFader.instance.StartNewGameFadeOut());
    }

    public void ExitGame()
    {
        AudioManager.instance.PlayButtonSound();
        StartCoroutine(SceneFader.instance.ExitGameFadeOut());
    }

    public void RefreshMoney()
    {
        moneyText.text = $"$ {MoneyManager.instance.currentMoney}";
    }

    public void RefreshKillCount()
    {
        killCountText.text = KillCount.instance.currentKillCount.ToString();
    }

    
}
