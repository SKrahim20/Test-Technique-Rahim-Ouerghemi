using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public static int aliveEnemies = 0;  
    public NavMeshAgent enemy;
    public Transform player;
    public float followRange = 10f;

    void Start()
    {
        aliveEnemies++;  
        EnemyAttack.OnEnemyDeath += HandleEnemyDeath;
    }

    void Update()
    {
        if (enemy != null && enemy.enabled)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= followRange)
            {
                enemy.SetDestination(player.position);
            }
        }
    }

    private void HandleEnemyDeath()
    {
        aliveEnemies--;  

        
        if (aliveEnemies <= 0)
        {         
            Debug.Log("enemies dead");
        }
    }

    private void OnDestroy()
    {
        EnemyAttack.OnEnemyDeath -= HandleEnemyDeath;
    }
}
