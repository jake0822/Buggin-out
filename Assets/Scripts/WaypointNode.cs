using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    [Tooltip("Other nearby nodes connected to this one.")]
    public List<WaypointNode> neighbors = new List<WaypointNode>();

    private void OnDrawGizmos()
    {
        // Draw this node
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);

        // Draw lines to neighbors
        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}
