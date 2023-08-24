using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : Tile
{
    private Player player;
    public Player Player {
        get => player;
        set
        {
            player = value;
            triangle.color = player.color;
        }
    }
    public SpriteRenderer triangle;

    void Start()
    {
        triangle = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }
}
