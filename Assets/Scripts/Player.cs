using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Player(string name, Color color)
    {
        playerName = name;
        this.color = color;
    }

    public string playerName;
    public List<Unit> units = new List<Unit>();
    public Color color;
}
