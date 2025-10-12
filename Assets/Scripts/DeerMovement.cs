using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Threading;
using JetBrains.Annotations;
public class DeerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private float timer;

    [Header("Wandering Settings")]
    public float wanderCoolDown = 5f;
    public float wanderRadius = 10f;
    public float forwardOffset = 5f;   // how far ahead to look for next destination
    public float turnAngle = 60f; // how wide the deer can turn (degrees)

    private float delay;
    void Start()
    {
        delay = UnityEngine.Random.Range(0, 2.5f);
        agent = GetComponent<NavMeshAgent>();
        timer = wanderCoolDown;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderCoolDown + delay)
        {
            delay = UnityEngine.Random.Range(0, 2.5f);
            Vector3 newPos = GetForwardWanderPosition();
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    Vector3 GetForwardWanderPosition()
    {
        for (int i = 0; i < 5; i++) // Try up to 5 times
        {
            float randomAngle = Random.Range(-turnAngle, turnAngle);
            Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
            Vector3 direction = rotation * transform.forward;
            Vector3 target = transform.position + direction * forwardOffset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(target, out hit, wanderRadius, NavMesh.AllAreas))
            {
                // Check distance from edges
                if (hit.hit && Vector3.Distance(hit.position, transform.position) > 2f)
                {
                    return hit.position;
                }
            }
        }

        // fallback
        return transform.position;
    }

}
