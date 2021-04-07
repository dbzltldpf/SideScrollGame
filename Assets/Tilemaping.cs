using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemaping : MonoBehaviour
{
    public Dictionary<Vector3Int, Node> nodes = new Dictionary<Vector3Int, Node>();
    public Tilemap tilemap;
    public Tilemap tilemap2;

    private void Awake()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                Node node = new Node(pos, true);
                nodes.Add(pos, node);
            }
        }
        foreach (var pos in tilemap2.cellBounds.allPositionsWithin)
        {
            if (tilemap2.HasTile(pos))
            {
                nodes[pos].Walkable = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        foreach (var node in nodes)
        {
            //갈수있는 곳이면 그린 갈수없는 지역이면 레드
            Gizmos.color = node.Value.Walkable ? Color.green * 0.5f : Color.red * 0.5f;
            Gizmos.DrawCube(node.Value.Position + new Vector3(0.5f, 0.5f, 0f), Vector3.one * 1);
        }
    }
}
