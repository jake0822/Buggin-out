using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

public class FlyingAgentRandom : MonoBehaviour
{
    public Random3DGraph graph;
    public float speed = 5f;
    public float arriveThreshold = 1f;

    private WaypointNode currentNode;
    private WaypointNode targetNode;
    private List<WaypointNode> path;
    private int pathIndex;

    IEnumerator Start()
    {
        // Wait until the graph has generated its nodes
        while (graph == null || graph.GetComponent<Random3DGraph>() == null || graph.GetComponent<Random3DGraph>().nodeCount == 0)
            yield return null;

        // Wait a frame or two to ensure generation finished
        yield return new WaitForSeconds(0.1f);

        // Now the graph should have nodes
        PickNewTarget();
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        // Find next waypoint
        WaypointNode next = path[pathIndex];
        //Calculate direction toward it
        //Vector3 dir = (next.transform.position - transform.position).normalized;
        Vector3 moveDir = (next.transform.position - transform.position).normalized;
        //Rotate to face movement direction
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
        }
        //transform.position += dir * speed * Time.deltaTime;
        //transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 3f);
        transform.position += moveDir * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, next.transform.position) < arriveThreshold)
        {
            pathIndex++;
            if (pathIndex >= path.Count)
                PickNewTarget(); // reached end, find a new path
        }
    }

    void PickNewTarget()
    {
        if (graph.nodes.Count < 2)
        {
            Debug.LogWarning("Graph has too few nodes for pathfinding!");
            return;
        }

        currentNode = FindClosestNode();
        if (currentNode == null)
        {
            Debug.LogWarning("No current node found!");
            return;
        }

        targetNode = graph.nodes[Random.Range(0, graph.nodes.Count)];
        path = AStarPathfinder.FindPath(currentNode, targetNode);

        if (path == null || path.Count == 0)
            Debug.LogWarning("No path found from " + currentNode.name + " to " + targetNode.name);

        pathIndex = 0;
    }

    WaypointNode FindClosestNode()
    {
        WaypointNode closest = null;
        float bestDist = Mathf.Infinity;
        foreach (var n in graph.nodes)
        {
            float d = Vector3.Distance(transform.position, n.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                closest = n;
            }
        }
        return closest;
    }
}
