using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Grid grid;

    [SerializeField] private Tile grass;
    [SerializeField] private Tile forest;
    [SerializeField] private Tile hill;
    [SerializeField] private Tile water;
    [SerializeField] private Tile road;
    [SerializeField] private Tile village;
    [SerializeField] private GameObject highlight;

    private GameObject highlightObject;

    private Tile[,] generateMap = new Tile[10,10];

    public Dictionary<Vector2Int, Tile> tiles;

    public Vector2Int activeTile;

    public void TileClicked(Tile tile)
    {
        activeTile = tile.coordinates;

        if (highlightObject != null)
        {
            Destroy(highlightObject);
        }

        highlightObject = Instantiate(highlight, grid.CellToWorld(new Vector3Int(activeTile.x, activeTile.y)), Quaternion.identity);

        foreach (Tile t in tiles.Values)
        {
            t.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (tiles[activeTile].unit != null)
        {
            HexMap.findDistances(activeTile, tiles, tiles[activeTile].unit, this);
            foreach (Tile t in tiles.Values)
            {
                Debug.Log("echo");
                if (t.distance > tiles[activeTile].unit.CurrentMovement)
                {
                    t.GetComponent<SpriteRenderer>().color = Color.gray;
                    Debug.Log(t.GetComponent<SpriteRenderer>());
                    Debug.Log(t.name);
                }
            }
        }
    }

    public void GenerateMap()
    {
        tiles = new Dictionary<Vector2Int, Tile>();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                generateMap[i, j] = grass;
            }
        }

        for (int i = 0; i < generateMap.GetLength(0); i++)
        {
            for (int j = 0; j < generateMap.GetLength(1); j++)
            {
                if (generateMap[i,j] != null)
                {
                    var spawnedTile = Instantiate(generateMap[i, j], grid.CellToWorld(new Vector3Int(i, j)), Quaternion.identity);
                    spawnedTile.name = $"Tile {i} {j}";
                    spawnedTile.gridManager = this;
                    spawnedTile.coordinates = new Vector2Int(i, j);
                    tiles[new Vector2Int(i, j)] = spawnedTile;
                }
            }
        }
    }

    public Tile GetTileAtPosition(Vector2Int pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
