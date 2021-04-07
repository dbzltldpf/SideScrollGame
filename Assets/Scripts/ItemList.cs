using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    Use,
    Equip,
    Quest,
    ETC,
}
[Serializable]
public class ItemList
{
    public List<ItemData> item;
}

[Serializable]
public class ItemData
{
    public int ID;//고유 값, 중복 불가
    public string Name; //중복가능
    public string Description;// 아이템 설명
    public string Type; //string으로 하면 가져와 지는데 ItemType
    public int Level;
    public int Exp;
    public int nextLevelExp;
    public int Count;
}

