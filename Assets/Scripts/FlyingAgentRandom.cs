using UnityEngine;
using System.Collections.Generic;

public class FlyingAgentRandom : MonoBehaviour
{
    public Random3DGraph graph;
    public float speed = 5f;
    public float arriveThreshold = 1f;

    private WaypointNode currentNode;
    private WaypointNode targetNode;
    private List<WaypointNode> path;
    private int pathIndex;

    void Start()
    {
        if (graph != null)
        {
            // Ensure the graph has generated nodes
            if (graph.nodes.Count == 0)
                graph.ConnectNodes(); // force connections if needed

            PickNewTarget();
        }
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        WaypointNode next = path[pathIndex];
        Vector3 dir = (next.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 3f);

        if (Vector3.Distance(transform.position, next.transform.position) < arriveThreshold)
        {
            pathIndex++;
            if (pathIndex >= path.Count)
                PickNewTarget(); // reached end, find a new path
        }
    }

    void PickNewTarget()
    {
        if (graph.nodes.Count < 2) return;

        currentNode = FindClosestNode();
        targetNode = graph.nodes[Random.Range(0, graph.nodes.Count)];

        path = AStarPathfinder.FindPath(currentNode, targetNode);
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
