using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState { unseen, open, closed}

    public GridManager gridManager;
    public Vector2Int coordinates;
    public string type;
    public Unit unit;

    public int distance;
    public TileState tileState;

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

public class TileComparer : IComparer<Tile>
{
    public int Compare(Tile t1, Tile t2)
    {
        return t1.distance.CompareTo(t2.distance);
    }
}