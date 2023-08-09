using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Player(string name, Color color, Vector2Int homeTile)
    {
        playerName = name;
        this.color = color;
        coins = 50;
        this.homeTile = homeTile;
        controlledVillages = 0;
    }

    public string playerName;
    public List<Unit> units = new List<Unit>();
    public List<Unit> recruitableUnits = new List<Unit>();
    public Color color;
    public Vector2Int homeTile;

    public int coins { get; set; }
    public int baseIncome = 2;
    public int villageIncome = 2;
    public int controlledVillages { get; set; }
}
