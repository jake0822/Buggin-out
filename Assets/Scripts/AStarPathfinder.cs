using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinder
{
    public static List<WaypointNode> FindPath(WaypointNode start, WaypointNode goal)
    {
        var openSet = new List<WaypointNode> { start };
        var cameFrom = new Dictionary<WaypointNode, WaypointNode>();
        var gScore = new Dictionary<WaypointNode, float>();
        var fScore = new Dictionary<WaypointNode, float>();

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            // Find node with lowest fScore
            WaypointNode current = openSet[0];
            foreach (var n in openSet)
            {
                if (!fScore.ContainsKey(n)) continue;
                if (fScore[n] < fScore[current]) current = n;
            }

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (var neighbor in current.neighbors)
            {
                float tentativeG = gScore[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        Debug.LogWarning("No path found between " + start.name + " and " + goal.name);
        return null;
    }

    private static float Heuristic(WaypointNode a, WaypointNode b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    private static List<WaypointNode> ReconstructPath(Dictionary<WaypointNode, WaypointNode> cameFrom, WaypointNode current)
    {
        var totalPath = new List<WaypointNode> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}
