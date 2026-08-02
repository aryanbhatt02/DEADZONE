using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

public static SaveManager instance;

    [SerializeField] PlayerController player;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] PlayerHealthUI playerHealth;
    [SerializeField] CampArea[] campAreas;
    [SerializeField] TMP_Text gameSavedText;

    
    public float saveTimer = 200f;

    public static bool shouldLoadGame = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
        StartCoroutine(AutoSave());
        gameSavedText.gameObject.SetActive(false);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);

        if (shouldLoadGame)
        {
            LoadGame();
        }
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveTimer);
            SaveGame();
            yield return new WaitForEndOfFrame();
        }
              
    }


    public void SaveGame()
    {
        AudioManager.instance.PlaySaveGameSound();

        gameSavedText.gameObject.SetActive(true);

        StartCoroutine(HideSavedText());

        PlayerPrefs.SetInt("Money", MoneyManager.instance.currentMoney);

        PlayerPrefs.SetInt("KillCount", KillCount.instance.currentKillCount);

        Vector3 pos = player.transform.position;

        PlayerPrefs.SetFloat("PlayerX", pos.x);
        PlayerPrefs.SetFloat("PlayerY", pos .y);
        PlayerPrefs.SetFloat("PlayerZ", pos.z);

        for(int i = 0; i < weaponManager.weapons.Length; i++)
        {
            WeaponController weapon = weaponManager.weapons[i];

            PlayerPrefs.SetInt($"WeaponUnlocked{i}", weapon.isUnloacked ? 1 : 0);
            PlayerPrefs.SetInt($"CurrentAmmo{i}", weapon.currentBullets);
            PlayerPrefs.SetInt($"TotalAmmo{i}", weapon.totalBullets);
        }

        for(int i = 0;i < campAreas.Length; i++)
        {
            PlayerPrefs.SetFloat($"AreaHealth{i}", campAreas[i].currentHealth);
        }

        PlayerPrefs.SetInt("CurrentWave", waveManager.currentWave);

        PlayerPrefs.SetInt("PlayerHealth", playerHealth.currentHealth);        

        PlayerPrefs.Save();

        
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        gameSavedText.gameObject.SetActive(false);

        MoneyManager.instance.currentMoney = PlayerPrefs.GetInt("Money", 200);

        KillCount.instance.currentKillCount = PlayerPrefs.GetInt("KillCount", 0);

        Vector3 position = new Vector3(
            PlayerPrefs.GetFloat("PlayerX", player.transform.position.x),
            PlayerPrefs.GetFloat("PlayerY", player.transform.position.y),
            PlayerPrefs.GetFloat("PlayerZ", player.transform.position.z));

        CharacterController cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        player.transform.position = position;
        cc.enabled = true;

        for(int i = 0; i< weaponManager.weapons.Length; i++)
        {
            WeaponController weapon = weaponManager.weapons[i];

            weapon.isUnloacked = PlayerPrefs.GetInt($"WeaponUnlocked{i}", i == 0 ? 1 : 0) == 1;
            weapon.currentBullets = PlayerPrefs.GetInt($"CurrentAmmo{i}", weapon.bulletsInMag);
            weapon.totalBullets = PlayerPrefs.GetInt($"TotalAmmo{i}", weapon.totalBullets);
        }

        for(int i = 0; i < campAreas.Length; i++)
        {
            campAreas[i].currentHealth = PlayerPrefs.GetFloat($"AreaHealth{i}", campAreas[i].maxHealth);
            campAreas[i].LoadDestroyedState();
        }

        waveManager.currentWave = PlayerPrefs.GetInt("CurrentWave", 0);

        playerHealth.currentHealth = PlayerPrefs.GetInt("PlayerHealth", playerHealth.maxHealth);

        UIManager.Instance.RefreshMoney();
        UIManager.Instance.RefreshKillCount();
        playerHealth.UpdateHealthBar();

        Debug.Log("Game Loaded");

    }

    IEnumerator HideSavedText()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameSavedText.gameObject.SetActive(false);
    }

}
