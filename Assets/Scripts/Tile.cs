using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int coordinates;
    public string type;
    public Unit unit;

    public Tile(GridManager gridManager, Vector2Int v)
    {
        this.gridManager = gridManager;
        coordinates = v;
    }

    private void OnMouseDown()
    {
        gridManager.TileClicked(this);
    }
}
