using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    [SerializeField]
    HiveGenerator hiveGenerator;

    static public List<HiveCell> GetPath(HiveCell startNode, HiveCell targetNode, int coalition = 0)
    {
        List<HiveCell> nodes = new List<HiveCell>();

        Dictionary<HiveCell, HiveCell> parents = new Dictionary<HiveCell, HiveCell>();

        // find shortest path from a > b 
        List<HiveCell> openSet = new List<HiveCell>();
        HashSet<HiveCell> closedSet = new HashSet<HiveCell>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            HiveCell currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (coalition == 0)
                {
                    // we will use shortest path
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                else 
                {
                    // enemy can take into account high dead zones and aviod those 
                    if (openSet[i].fEnemyCost < currentNode.fEnemyCost || openSet[i].fEnemyCost == currentNode.fEnemyCost && openSet[i].hCost < currentNode.hCost )
                    {
                        currentNode = openSet[i];
                    }
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                // retrace the path 
                HiveCell cNode = targetNode;
                while (cNode != startNode)
                {
                    HiveCell pn = parents[cNode];
                    nodes.Add(pn);
                    cNode = pn;
                }
                nodes.Reverse();
                return nodes;
            }

            if (currentNode == null) continue;
            foreach (HiveCell neighbour in currentNode.GetNeighbours())
            {
                if (neighbour.walkable == 0 || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCost = currentNode.gCost + GetNodeDistance(currentNode, neighbour);
                if (newMovementCost < neighbour.gCost || openSet.Contains(neighbour) == false)
                {
                    neighbour.gCost = newMovementCost;
                    neighbour.hCost = GetNodeDistance(neighbour, targetNode);

                    if(!parents.ContainsKey(neighbour))
                        parents.Add(neighbour, currentNode);

                    if (openSet.Contains(neighbour) == false)
                    {
                        openSet.Add(neighbour);
                    }
                }

            }
        }
        return nodes;
    }
    static protected int GetNodeDistance(HiveCell a, HiveCell b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

}
