
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 20f;
    public float stopDistance = 2f;

    private NavMeshAgent agent;
    private EnemyLife enemyLife;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLife = GetComponent<EnemyLife>(); 
    }

    void Update()
    {
       
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            return;
        }

       
        if (enemyLife.vidaEnemigo <= 0)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

      
        if (distance < detectionRange)
        {
            if (distance > stopDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }
    }
}