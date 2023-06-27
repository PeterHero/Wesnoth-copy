using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid grid;

    [SerializeField] private Tile grass;
    [SerializeField] private Tile forest;
    [SerializeField] private Tile hill;
    [SerializeField] private Tile water;
    [SerializeField] private Tile road;
    [SerializeField] private Tile village;
    [SerializeField] private Tile highlight;

    private GameObject highlightObject;

    private Tile[,] generateMap = new Tile[10,10];

    private Dictionary<Vector2, Tile> tiles;

    public Vector2 activeTile;

    public void TileClicked(Tile tile)
    {
        activeTile = tile.coordinates;

    }

    public void GenerateMap()
    {
        tiles = new Dictionary<Vector2, Tile>();

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
                    spawnedTile.coordinates = new Vector2(i, j);
                    tiles[new Vector2(i, j)] = spawnedTile;
                }
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
