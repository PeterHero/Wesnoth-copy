using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2 coordinates;
    public string type;
    public Unit unit;

    public Tile(GridManager gridManager, Vector2 v)
    {
        this.gridManager = gridManager;
        coordinates = v;
    }

    private void OnMouseDown()
    {
        gridManager.TileClicked(this);
    }
}
