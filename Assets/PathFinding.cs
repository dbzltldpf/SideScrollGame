using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PathFinding : MonoBehaviour
{
    //public Dictionary<Vector3Int, Node> nodes = new Dictionary<Vector3Int, Node>();
    //public Tilemap tilemap;
    //public Tilemap tilemap2;
    public List<Node> pathNodes;
    private Tilemaping tilemaping;

    private void Awake()
    {
        pathNodes = new List<Node>();
        tilemaping = FindObjectOfType<Tilemaping>();
        //foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        //{
        //    if (tilemap.HasTile(pos))
        //    {
        //        Node node = new Node(pos, true);
        //        nodes.Add(pos, node);
        //    }
        //}
        //foreach (var pos in tilemap2.cellBounds.allPositionsWithin)
        //{
        //    if (tilemap2.HasTile(pos))
        //    {
        //        nodes[pos].Walkable = false;
        //    }
        //}
    }


    public void FindPath(Vector3 startPos, Vector3 targetPos, Action<Stack<Node>, bool> callback)
    {
        Node startNode = GetNodeFromPosition(startPos);
        Node targetNode = GetNodeFromPosition(targetPos);

        Vector3Int end = tilemaping.tilemap.WorldToCell(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F < currentNode.F || (openSet[i].F == currentNode.F && openSet[i].H < currentNode.H))
                {
                    currentNode = openSet[i];
                }

            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                callback(GetPath(startNode, targetNode), true);
                pathNodes.AddRange(GetPath(startNode, targetNode));
                return;
            }

            foreach (var neighbourNode in GetNeighbours(currentNode.Position))
            {
                if (closeSet.Contains(neighbourNode) || neighbourNode.Walkable == false)
                {
                    continue;
                }

                int newNeighbourNodeGvalue = currentNode.G + GetDistance(currentNode, neighbourNode);

                if (openSet.Contains(neighbourNode))
                {
                    if (newNeighbourNodeGvalue < neighbourNode.G)
                    {
                        neighbourNode.G = newNeighbourNodeGvalue;
                        neighbourNode.Parent = currentNode;
                    }
                }
                else
                {
                    neighbourNode.G = newNeighbourNodeGvalue;
                    neighbourNode.Parent = currentNode;
                    neighbourNode.H = GetDistance(neighbourNode, tilemaping.nodes[end]);
                    openSet.Add(neighbourNode);
                }
            }
        }



        callback(new Stack<Node>(), false);

    }

    private List<Node> GetNeighbours(Vector3Int currentNode)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //정사각형에서 십자가모양으로 검색(대각선 검색)   //대각선 검색 X
                if (Mathf.Abs(x) != Mathf.Abs(y))             //if(x != 0 || y != 0 )
                {
                    Vector3Int neighbourPos = new Vector3Int(currentNode.x - x, currentNode.y - y, currentNode.z);

                    if (tilemaping.nodes.ContainsKey(neighbourPos))
                    {
                        neighbours.Add(tilemaping.nodes[neighbourPos]);
                    }
                }
            }

        }

        return neighbours;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int distY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    public Node GetNodeFromPosition(Vector3 worldPosition)
    {
        Vector3Int pos = tilemaping.tilemap.WorldToCell(worldPosition);
        return tilemaping.nodes[pos];
    }

    private Stack<Node> GetPath(Node startNode, Node targetNode)
    {
        Stack<Node> path = new Stack<Node>();

        Node currentNode = targetNode;
        path.Push(currentNode);

        while (currentNode != startNode)
        {
            currentNode = currentNode.Parent;
            path.Push(currentNode);
        }

        return path;

    }

    //private void OnDrawGizmos()
    //{
    //    if (pathNodes.Count > 0)
    //    {
    //        foreach (var node in nodes)
    //        {
    //            //갈수있는 곳이면 그린 갈수없는 지역이면 레드
    //            Gizmos.color = node.Value.Walkable ? Color.green * 0.5f : Color.red * 0.5f;
    //            Gizmos.DrawCube(node.Value.Position + new Vector3(0.5f, 0.5f, 0f), Vector3.one * 1);
    //        }
    //    }
    //
    //}
}
