using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum State
{
    Idle,
    Combat,
    Move,
    Attack,
    Search,
    Check,
    Action,
};

[Serializable]
public class PlayerState
{
    public Character[] character; 
}

[Serializable]
public class Character
{
    public string Name;
    public int Money;
    public int Energy;
    public int Lv;
    public int Hp;
    public int Mp;
    public int O2;
    public int Str;
    public int Int;
    public int Dex;
    public int Con;
    public float X;
    public float Y;
    public float Z;
    public string CurrentMapName;
}
