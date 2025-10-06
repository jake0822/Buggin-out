using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Threading;
using JetBrains.Annotations;
public class DeerMovement : MonoBehaviour
{
    Vector3 center;
    Vector3 size;

    private NavMeshAgent agent;
    public float timer;
    public float wanderCoolDown=5f;
    public float wanderRadius=10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        timer = wanderCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= wanderCoolDown) {
            Vector3 targetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(targetPosition);
            timer = 0;
        }
        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
