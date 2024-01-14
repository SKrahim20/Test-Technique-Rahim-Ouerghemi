using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public Animator playerAnimator;  
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI coinText; 
    public TextMeshProUGUI messageText;
    public Button botrkButton;
    public Button heartButton;
    public Button bootsButton;

    private int currentDamage = 50;
    private int currentHP = 200;
    private float initialSpeed = 200f;
    private int botrkCount = 0;
    private int heartCount = 0;
    private bool botrkMerged = false;
    private bool heartMerged = false;
    private bool bootsPurchased = false;
    private int playerCoins = 0 ; 

    private int botrkCost = 6;
    private int heartCost = 7;
    private int bootsCost = 5;



    private void Start()
    {
        UpdateCoinText();
    }

    public void BuyBotrk()
    {
        if (playerCoins >= botrkCost && !botrkMerged)
        {
            playerCoins -= botrkCost;
            UpdateCoinText();

            CharacterController.instance.attackDamage += 30;
            

            currentDamage += 30;
            botrkCount++;

            UpdateStats();

            if (botrkCount == 1 && heartCount == 1)
            {
               mergeHeartBotrk();
            }
            else if (botrkCount >= 2)
            {
                MergeBotrkItems();
            }

            
            playerAnimator.SetBool("IsUpgraded", true);
            StartCoroutine(ResetUpgradeFlag());
        }
    }

    public void BuyHeart()
    {
        if (playerCoins >= heartCost && !heartMerged)
        {
            playerCoins -= heartCost;
            UpdateCoinText();

            PlayerHealth.instance.maxHealth += 50;
            PlayerHealth.instance.currentHealth += 50;
            

            currentHP += 50;
            heartCount++;

            UpdateStats();

            if (botrkCount == 1 && heartCount == 1)
            {
                mergeHeartBotrk();
            }
            else if (heartCount >= 2)
            {
                MergeHeartItems();
            }

            
            playerAnimator.SetBool("IsUpgraded", true);
            StartCoroutine(ResetUpgradeFlag());
        }
    }

    public void BuyBoots()
    {
        if (playerCoins >= bootsCost && !bootsPurchased)
        {
            CharacterController.instance.moveSpeed += 1;
            CharacterController.instance.sprintSpeed += 1;
            playerCoins -= bootsCost;
            UpdateCoinText();

            initialSpeed *= 1.05f;
            bootsPurchased = true;
            DisableButton(bootsButton);

            UpdateStats();

            
            playerAnimator.SetBool("IsUpgraded", true);
            StartCoroutine(DisplayMessage("Boots purchased ", 3f));
            StartCoroutine(ResetUpgradeFlag());
        }
    }

    private void MergeBotrkItems()
    {
        currentDamage -= 80;
        currentDamage += 150;

        CharacterController.instance.attackDamage += 70;

        DisableButton(botrkButton);

        UpdateStats();

        botrkMerged = true;

        StartCoroutine(DisplayMessage("Super Damage", 3f));
    }

    private void MergeHeartItems()
    {
        
        PlayerHealth.instance.currentHealth = 400 ;
        PlayerHealth.instance.maxHealth = 400;
        currentHP = 400 ;

        


        DisableButton(heartButton);

        UpdateStats();

        heartMerged = true;

        StartCoroutine(DisplayMessage("Super Health", 3f));
    }

    private void mergeHeartBotrk()
    {
        currentDamage = 100;
        currentHP = 300;

        PlayerHealth.instance.currentHealth = 300;
        PlayerHealth.instance.maxHealth = 300;

        CharacterController.instance.attackDamage = 100;

        currentHP = 300;

        DisableButton(botrkButton);
        DisableButton(heartButton);

        UpdateStats();

        botrkMerged = true;
        heartMerged = true;

        StartCoroutine(DisplayMessage("Super Damage and Health", 3f));
    }

    private void DisableButton(Button button)
    {
        button.interactable = false;
    }

    private void UpdateStats()
    {

        damageText.text = currentDamage.ToString();
        hpText.text = (" ") + currentHP.ToString();
        speedText.text = initialSpeed.ToString("F0");
    }

    private void UpdateCoinText()
    {
        coinText.text = "" + playerCoins.ToString();
    }

    private IEnumerator ResetUpgradeFlag()
    {
        yield return new WaitForSeconds(0.1f); 
        playerAnimator.SetBool("IsUpgraded", false);
    }

    private IEnumerator DisplayMessage(string message, float duration)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        messageText.gameObject.SetActive(false);
    }

    
    public void CollectCoins(int collectedCoins)
    {
        playerCoins += collectedCoins;
        UpdateCoinText();
    }

}
