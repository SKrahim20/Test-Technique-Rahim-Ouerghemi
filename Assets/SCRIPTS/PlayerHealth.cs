using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int maxHealth = 200;
    public int currentHealth;
    public TMP_Text healthText;


    private Animator animator;

    public bool dead = false;

    void Start()
    {
        currentHealth = maxHealth;
        instance = this;


        animator = GetComponent<Animator>();

        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthText();

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;

        if (animator != null)
        {
            animator.SetBool("IsDeads", true);
        }

        Debug.Log("Player has died.");
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = " " + currentHealth;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}