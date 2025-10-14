using UnityEngine;
using System.Collections.Generic;

public class Random3DGraph : MonoBehaviour
{
    public int nodeCount = 100;
    public Vector3 zoneSize = new Vector3(50, 30, 50);
    public float connectionRadius = 15f;
    public LayerMask obstacleMask;
    public bool requireLineOfSight = true;
    public GameObject nodePrefab; // optional visual sphere

    [HideInInspector] public List<WaypointNode> nodes = new List<WaypointNode>();

    void Start()
    {
        GenerateNodes();
        ConnectNodes();
    }

    public void GenerateNodes()
    {
        nodes.Clear();

        for (int i = 0; i < nodeCount; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-zoneSize.x / 2, zoneSize.x / 2),
                Random.Range(-zoneSize.y / 2, zoneSize.y / 2),
                Random.Range(-zoneSize.z / 2, zoneSize.z / 2)
            );

            GameObject nodeObj = nodePrefab
                ? Instantiate(nodePrefab, randomPos, Quaternion.identity, transform)
                : new GameObject("Node_" + i, typeof(WaypointNode));

            WaypointNode node = nodeObj.GetComponent<WaypointNode>();
            nodes.Add(node);
        }
    }

    public void ConnectNodes()
    {
        foreach (var node in nodes)
        {
            node.neighbors.Clear();
            foreach (var other in nodes)
            {
                if (other == node) continue;
                float dist = Vector3.Distance(node.transform.position, other.transform.position);

                if (dist <= connectionRadius)
                {
                    if (requireLineOfSight)
                    {
                        if (!Physics.Raycast(node.transform.position, (other.transform.position - node.transform.position).normalized, dist, obstacleMask))
                            node.neighbors.Add(other);
                    }
                    else
                    {
                        node.neighbors.Add(other);
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
