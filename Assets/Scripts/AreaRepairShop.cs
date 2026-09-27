using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AreaRepairShop : MonoBehaviour
{

    [SerializeField] CampArea campArea;
    [SerializeField] WaveManager waveManager;

    [Header("Player")]
    [SerializeField] Transform player;

    [Header("Repair Settings")]
    [SerializeField] float repairDistance = 2f;
    [SerializeField] float repairTime = 3f;
    //public int repairAmount = 10000;
    [SerializeField] int repairCost = 200;

    [Header("UI")] 
    [SerializeField] GameObject repairGateText;
    [SerializeField] GameObject repairingText;
    [SerializeField] GameObject repairedText;
    [SerializeField] GameObject notEnoughMoneyText;
    [SerializeField] GameObject repairButton;
    [SerializeField] Image repairBar;
    [SerializeField] Image repaireWorldSpaceImage;

    [SerializeField] float imageRotateSpeed = 25f;

    float repairProgress;

    bool repairing;    

    private void Start()
    {
        repairGateText.SetActive(false);
        repairingText.SetActive(false);
        repairedText.SetActive(false);
        repairButton.SetActive(false);
        notEnoughMoneyText.SetActive(false);
        repaireWorldSpaceImage.gameObject.SetActive(true);

        repairBar.fillAmount = 0;
    }

    private void Update()
    {
        RepairWorldSpaceImage();

        float distance = Vector3.Distance(player.position, campArea.transform.position);        

        if(campArea.currentHealth < campArea.maxHealth && distance < repairDistance)
        {
            repairGateText.SetActive(true);
            repairButton.SetActive(true);
            repaireWorldSpaceImage.gameObject.SetActive(false);

            if (repairing)
            {
                if (MoneyManager.instance.HasEnoughMoney(repairCost))
                {                    
                    RepairGate(); 
                   
                }
                else
                {
                    AudioManager.instance.PlayInvalidSound();
                    AudioManager.instance.StopGateRepairingSound();

                    waveManager.waveBtwText.SetActive(false);
                    notEnoughMoneyText.SetActive(true);
                    //repairingText.SetActive(false);
                    //repairBar.fillAmount = 0;
                }

            }
            else
            {
                repairingText.SetActive(false);
                notEnoughMoneyText.SetActive(false);
                repairBar.fillAmount = 0;
                repairProgress = 0;
            }
        }
        else
        {
            repairGateText.SetActive(false);
            repairingText.SetActive(false);            
            repairButton.SetActive(false);            

            repairBar.fillAmount = 0;
        }

    }      

    void RepairGate()
    {       
        repairingText.SetActive(true);
        repairingText.GetComponent<Animator>().Play("Repairing Text");

        repairGateText.SetActive(false);

        repairProgress += Time.deltaTime;

        repairBar.fillAmount = repairProgress / repairTime;

        if(repairProgress >= repairTime)
        {
            AudioManager.instance.StopGateRepairingSound();
            AudioManager.instance.PlayCompletedSound();

            MoneyManager.instance.SpendMoney(repairCost);
            repairingText.SetActive(false);
            repairButton.SetActive(false);
            notEnoughMoneyText.SetActive(false);
            repairedText.SetActive(true);

            repairBar.fillAmount = 0;
            campArea.currentHealth = campArea.maxHealth;

            campArea.destroyed = false;

            campArea.fence.GetComponent<Collider>().enabled = true;
            campArea.fence.GetComponent<MeshRenderer>().enabled = true;
            campArea.fenceBorder.GetComponent<Collider>().enabled = true;
            campArea.fenceBorder.GetComponent<MeshRenderer>().enabled = true;

            campArea.fenceObstacle.enabled = true;

            Invoke(nameof(HideRepairedText), 1f);

            ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();

            foreach (ZombieAI zombie in zombies)
            {
                if (zombie.homeArea == campArea)
                {
                    zombie.target = campArea.areaTargets[0].transform;
                }
            }

        }
    }

    private void HideRepairedText()
    {
        repairProgress = 0;
        repairing = false;
        repairedText.SetActive(false);
    }

    public void StartRepairing()
    {
        if (MoneyManager.instance.HasEnoughMoney(repairCost))
        {
            AudioManager.instance.PlayGateRepairingSound();
        }
        repairing = true;
    }

    public void StopRepairing()
    {        
        repairing = false;

        AudioManager.instance.StopGateRepairingSound();
    }

    private void RepairWorldSpaceImage()
    {
        if(campArea.currentHealth < campArea.maxHealth)
        {
            repaireWorldSpaceImage.gameObject.SetActive(true);

            repaireWorldSpaceImage.transform.Rotate(0f, imageRotateSpeed * Time.deltaTime, 0f, Space.World);
        }
        else
        {
            repaireWorldSpaceImage.gameObject.SetActive(false);
        }
    }
        
}
