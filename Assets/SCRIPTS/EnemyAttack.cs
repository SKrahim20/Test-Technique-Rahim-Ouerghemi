using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{

    
    public delegate void EnemyDeathAction();
    public static event EnemyDeathAction OnEnemyDeath;

    public int maxHealth = 300;
    private int currentHealth;
    private Animator enemyAnimator;
    private NavMeshAgent navMeshAgent;

    

    public GameObject coinPrefab;
    private bool hasSpawnedCoins = false;

    private bool canAttack = true;
    public float attackCooldown = 2f;

    public GameObject lootObject;

    public int attackDamage = 50; 

    public GameObject swordPrefab;

    public float attackRange = 1f;

    private bool dead = false;
    

    void Start()
    {
        
        currentHealth = maxHealth;
        enemyAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!PlayerHealth.instance.dead)
        {
            if (Vector3.Distance(transform.position, CharacterController.instance.transform.position) <= attackRange)
            {
                if (canAttack && !dead)
                {
                    PlayAttackAnimation();
                    StartCoroutine(StartAttackCooldown());
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(CharacterController.instance.attackDamage);
            PlayDmgAnimation();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == 6)
        {
            CharacterController.instance.canHit = true;

            if (CharacterController.instance.hit)
            {
                Debug.Log("enter");
                TakeDamage(CharacterController.instance.attackDamage);
                CharacterController.instance.hit = false;
                CharacterController.instance.canHit = false;
            }
        }
    }

    public void AttEvent()
    {
        PlayerHealth.instance.TakeDamage(attackDamage);
    }

    void PlayAttackAnimation()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("IsAttc");

           
        }
    }

    IEnumerator StartAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (damage == 50)
        {
            PlayDmgAnimation();
        }

        Debug.Log("Enemy HP: " + currentHealth);
    }

    void PlayDmgAnimation()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("IsDmg");
        }
    }

    void Die()
    {
        if (dead) 
        {
            return;
        }

        dead = true;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("IsDead", true);
        }

        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.enabled = false;
        }

        
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        
        if (lootObject != null)
        {
            Collider lootCollider = lootObject.GetComponent<Collider>();
            if (lootCollider != null)
            {
                lootCollider.enabled = false;
            }
        }

        if (!hasSpawnedCoins)
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnCoin();
            }

            hasSpawnedCoins = true;
        }

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
        }

        SpawnSword();

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
        }
    }

    void SpawnCoin()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    void SpawnSword()
    {
        if (swordPrefab != null)
        {
            Instantiate(swordPrefab, transform.position, Quaternion.identity);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
