using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedkitShop : MonoBehaviour
{
    [SerializeField] PlayerHealthUI playerHealthUI;

    [SerializeField] Transform medkitBox;

    [Header("Player")]
    [SerializeField] Transform player;

    [Header("Medkit Setting")]
    [SerializeField] float refillDistance = 2f;    
    [SerializeField] float refillTime = 3f;
    [SerializeField] int refillCost = 200;

    [Header("UI")]
    [SerializeField] GameObject medkitButton;
    [SerializeField] GameObject medkitHintText;
    [SerializeField] GameObject applyingText;
    [SerializeField] GameObject appliedText;
    [SerializeField] GameObject notHaveMoneyText;
    [SerializeField] GameObject alreadyFullHealthText;
    [SerializeField] Image refillBar;

    [Header("World Space Image")]
    [SerializeField] Image medkitWorlSpaceImage;
    [SerializeField] float ImageRotateSpeed = 25f;

    float refillProgress;
    private bool refilling;

    void Start()
    {
        medkitButton.SetActive(false);
        medkitHintText.SetActive(false);
        notHaveMoneyText.SetActive(false);
        applyingText.SetActive(false);
        appliedText.SetActive(false);
        notHaveMoneyText.SetActive(false);
        alreadyFullHealthText.SetActive(false);
        refillBar.fillAmount = 0;
        medkitWorlSpaceImage.gameObject.SetActive(true);
    }
    
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, medkitBox.transform.position);

        if(distance < refillDistance) //&& playerHealthUI.currentHealth < playerHealthUI.maxHealth  <<----- DON'T FORGOT TO ADD THIS
        {
            medkitButton.SetActive(true);
            medkitHintText.SetActive(true);
            medkitWorlSpaceImage.gameObject.SetActive(false);

            if (refilling)
            {
                if (MoneyManager.instance.HasEnoughMoney(refillCost))
                {
                    RefillHealth();
                }
                else
                {
                    AudioManager.instance.StopHealingSound();
                    AudioManager.instance.PlayInvalidSound();
                    notHaveMoneyText.SetActive(true);
                    Invoke(nameof(HideNotHaveMoneyText), 0.5f);
                    
                }
            }

            else
            {
                medkitHintText.SetActive(true);
                medkitButton.SetActive(true);                                                
                refillBar.fillAmount = 0;
                refillProgress = 0f;
            }
            
        }
        else
        {
            medkitButton.SetActive(false);
            medkitHintText.SetActive(false);
            notHaveMoneyText.SetActive(false);
            appliedText.SetActive(false);
            applyingText.SetActive(false);
            alreadyFullHealthText.SetActive(false);
            refillBar.fillAmount= 0;
            medkitWorlSpaceImage.gameObject.SetActive(true);
            medkitWorlSpaceImage.transform.Rotate(0f, ImageRotateSpeed * Time.deltaTime, 0f, Space.World);
        }
    }

    private void RefillHealth()
    {
        if (playerHealthUI.currentHealth >= playerHealthUI.maxHealth)
        {
            refilling = false;
            refillProgress = 0f;

            AudioManager.instance.StopHealingSound();
            AudioManager.instance.PlayInvalidSound();

            alreadyFullHealthText.SetActive(true);
            Invoke(nameof(HideAlreadyFullHealthText), 1f);

            return;
        }
            alreadyFullHealthText.SetActive(false);
            medkitHintText.SetActive(false);

            applyingText.SetActive(true);
            applyingText.GetComponent<Animator>().Play("Repairing Text");

            refillProgress += Time.deltaTime;

            refillBar.fillAmount = refillProgress / refillTime;

        if (refillProgress >= refillTime)
        {
            AudioManager.instance.StopHealingSound();
            AudioManager.instance.PlayHealSuccessSound();

            MoneyManager.instance.SpendMoney(refillCost);
            applyingText.SetActive(false);

            appliedText.SetActive(true);
            Animator anim = appliedText.GetComponent<Animator>();
            anim.Play("Applied Text");

            refillBar.fillAmount = 0;

            playerHealthUI.currentHealth = playerHealthUI.maxHealth;
            playerHealthUI.RefillHealth();

            Invoke(nameof(HideNotHaveMoneyText), 2f);
        }                                                                               
     }

    public void StartRefill()
    {
        if (playerHealthUI.currentHealth >= playerHealthUI.maxHealth)
        {
            AudioManager.instance.PlayInvalidSound();
            alreadyFullHealthText.SetActive(true);
            Invoke(nameof(HideAlreadyFullHealthText), 1f);
            return;
        }

        if (!MoneyManager.instance.HasEnoughMoney(refillCost))
        {
            AudioManager.instance.PlayInvalidSound();
            notHaveMoneyText.SetActive(true);
            Invoke(nameof(HideNotHaveMoneyText), 1f);
            return;
        }

        AudioManager.instance.PlayHealingSound();
        refilling = true;
    }

    public void StopRefill()
    {
        refilling = false;
        AudioManager.instance.StopHealingSound();
    }

    private void HideAppliedText()
    {
        appliedText.SetActive(false);
        refillProgress = 0f;
    }
    private void HideNotHaveMoneyText()
    {
        notHaveMoneyText.SetActive(false);
    }
    private void HideAlreadyFullHealthText()
    {
        alreadyFullHealthText.SetActive(false);
    }
}
