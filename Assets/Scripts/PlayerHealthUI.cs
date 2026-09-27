using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthbar;

    private bool isDead;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        AudioManager.instance.PlayPlayerHitSound();

        currentHealth -= damage;

        Debug.Log("Player Health: " + currentHealth);

        healthbar.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            isDead = true;

            AudioManager.instance.PlayPlayerDeathSound();

            currentHealth = 0;
            Debug.Log($"Player Destroyed");
            //Invoke(nameof (Restart), 5f);
            UIManager.Instance.GameOverPanal();
        }
    }

    //public void Restart()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    public void RefillHealth()
    {
        currentHealth = maxHealth;
        healthbar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void UpdateHealthBar()
    {
        healthbar.fillAmount = (float)currentHealth / maxHealth;
    }

}
