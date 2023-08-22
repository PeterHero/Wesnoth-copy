using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState { unseen, open, closed}
    public enum TerrainType { castle, flat, forest, hills, village, water}

    public GridManager gridManager { get; set; }
    public Vector2Int coordinates { get; set; }
    public TerrainType terrain;
    public Unit unit { get; set; }

    public int distance; //{ get; set; }
    public TileState tileState { get; set; }

    public Tile(GridManager gridManager, Vector2Int v)
    {
        this.gridManager = gridManager;
        coordinates = v;
    }

    private void OnMouseDown()
    {
        gridManager.TileClicked(this);
    }

    private void OnMouseOver()
    {
        gridManager.TileHovered(this);
    }
}

public class TileComparer : IComparer<Tile>
{
    public int Compare(Tile t1, Tile t2)
    {
        return t1.distance.CompareTo(t2.distance);
    }
}