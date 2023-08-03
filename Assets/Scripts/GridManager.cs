using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class GridManager : MonoBehaviour
{
    public Grid grid;
    public CanvasManager canvasManager;
    public Battle battle;

    [SerializeField] private Tile grass;
    [SerializeField] private Tile forest;
    [SerializeField] private Tile hills;
    [SerializeField] private Tile water;
    [SerializeField] private Tile road;
    [SerializeField] private Tile village;
    [SerializeField] private GameObject highlight;

    private GameObject highlightObject;

    private Tile[,] generateMap = new Tile[10,10];

    public Dictionary<Vector2Int, Tile> tiles;

    private Vector2Int ActiveTile;

    private Vector2Int activeUnit;
    private Vector2Int ActiveUnit
    {
        get => activeUnit;
        set
        {
            activeUnit = value;
            isActiveUnitSet = true;
        }
    }
    public bool isActiveUnitSet;

    private void displayUnitStats(Unit unit)
    {
        canvasManager.Type = unit.UnitTypeName;
        canvasManager.Health = $"HP {unit.CurrentHP}/{unit.MaxHP}";
        canvasManager.Experience = $"XP {unit.CurrentXP}/{unit.MaxXP}";
        canvasManager.Movement = $"MP {unit.CurrentMovement}/{unit.MaxMovement}"; ;
        canvasManager.Defence = $"def {unit.Defence} %";
        canvasManager.Level = $"lvl {unit.Level}";
    }

    public void TileHovered(Tile tile)
    {
        highlightObject.transform.position = grid.CellToWorld(new Vector3Int(tile.coordinates.x, tile.coordinates.y));
        
        if (!isActiveUnitSet)
        {
            CleanTileColoring();
        }

        if (tile.unit != null)
        {
            displayUnitStats(tile.unit);
            if (!isActiveUnitSet)
            {
                CleanTileColoring();
                handleUnitDistances(tile.coordinates);
            }
        }
    }

    private bool isAdjecent(Vector2Int v1, Vector2Int v2)
    {
        return HexMap.GetAdjencentTiles(v1).Contains(v2);
    }

    private void handleUnitDistances(Vector2Int startTile)
    {
        HexMap.findDistances(startTile, tiles, tiles[startTile].unit, this);
        foreach (Tile t in tiles.Values)
        {
            if (t.distance > tiles[startTile].unit.CurrentMovement)
            {
                t.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    public void TileClicked(Tile tile)
    {
        ActiveTile = tile.coordinates;

        CleanTileColoring();


        if (tiles[ActiveTile].unit == null)
        {
            if (isActiveUnitSet)
            {
                if (tiles[ActiveTile].distance <= tiles[ActiveUnit].unit.CurrentMovement)
                {
                    MoveUnit(tiles[ActiveUnit], tiles[ActiveTile]);
                    isActiveUnitSet = false;
                }
                else
                {
                    isActiveUnitSet = false;
                }
            }
        }
        else // clicked tile with a unit
        {
            if (isActiveUnitSet && isAdjecent(ActiveUnit, ActiveTile) && tiles[ActiveUnit].unit.Player != tiles[ActiveTile].unit.Player)
            {
                if (tiles[ActiveUnit].unit.CanAttack)
                {
                    // ActiveUnit = attacker, ActiveTile = defender
                    battle.Fight(tiles[ActiveUnit].unit, tiles[ActiveUnit].unit.Attacks[0], tiles[ActiveTile].unit, tiles[ActiveTile].unit.Attacks[0]);
                    tiles[ActiveUnit].unit.CurrentMovement = 0;
                    tiles[ActiveUnit].unit.CanAttack = false;
                }
                
                isActiveUnitSet = false;
            }
            else
            {
                if (tiles[ActiveTile].unit.Player != battle.playerOnTurn)
                {
                    isActiveUnitSet = false;
                    return;
                }

                ActiveUnit = ActiveTile;
                handleUnitDistances(ActiveUnit);
            }
        }
    }

    public void MoveUnit(Tile oldTile, Tile newTile)
    {
        oldTile.unit.transform.position = grid.CellToWorld(new Vector3Int(ActiveTile.x, ActiveTile.y));
        oldTile.unit.CurrentMovement -= newTile.distance;
        newTile.unit = oldTile.unit;
        oldTile.unit = null;

        newTile.unit.setDefense(newTile.terrain);
    }

    private void fillGenerateMap()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                generateMap[i, j] = (j == 4 || j == 5) ? hills : grass;
            }
        }

        generateMap[3, 3] = forest;
    }

    public void GenerateMap()
    {
        tiles = new Dictionary<Vector2Int, Tile>();

        fillGenerateMap();

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

        highlightObject = Instantiate(highlight, grid.CellToWorld(new Vector3Int(0, 0)), Quaternion.identity);
    }

    public Tile GetTileAtPosition(Vector2Int pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }

    public void CleanTileColoring()
    {
        foreach (Tile t in tiles.Values)
        {
            t.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
