using UnityEngine;
using System;

[Serializable]
public class Node
{
    public bool Walkable { get; set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F { get { return G + H; } }

    public Node Parent { get; set; }
    public Vector3Int Position { get; private set; }
    public int X { get => Position.x; }
    public int Y { get => Position.y; }

    public Node(Vector3Int pos, bool walkable)
    {
        Position = pos;
        Walkable = walkable;
    }

}