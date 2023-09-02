using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [HideInInspector] public Grid grid;
    [HideInInspector] public UIManager UIManager;
    [HideInInspector] public BattleManager battleManager;

    public Color tileColor = Color.white;

    [SerializeField] private Tile grass;
    [SerializeField] private Tile forest;
    [SerializeField] private Tile hills;
    [SerializeField] private Tile water;
    [SerializeField] private Tile road;
    [SerializeField] private Tile village;
    [SerializeField] private GameObject highlight;

    private GameObject highlightObject;

    private Tile[,] generateMap = new Tile[10,20];

    public Dictionary<Vector2Int, Tile> tiles;

    [HideInInspector] public bool ActionsDisabled;

    public Vector2Int ActiveTile;

    private Vector2Int activeUnit;
    public Vector2Int ActiveUnit
    {
        get => activeUnit;
        set
        {
            activeUnit = value;
            isActiveUnitSet = true;
        }
    }
    [HideInInspector] public bool isActiveUnitSet;

    public void TileHovered(Tile tile)
    {
        highlightObject.transform.position = grid.CellToWorld(new Vector3Int(tile.coordinates.x, tile.coordinates.y));

        if (ActionsDisabled)
            return;

        if (!isActiveUnitSet)
        {
            CleanTileColoring();
        }

        if (tile.unit != null)
        {
            UIManager.DisplayUnitStats(tile.unit);
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
        HexMap.FindDistances(startTile, tiles, tiles[startTile].unit, this);
        foreach (Tile t in tiles.Values)
        {
            if (t.distance > tiles[startTile].unit.CurrentMovement || t.distance == -1)
            {
                t.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    public void TileClicked(Tile tile)
    {
        if (ActionsDisabled)
            return;

        ActiveTile = tile.coordinates;

        CleanTileColoring();


        if (tiles[ActiveTile].unit == null)
        {
            if (isActiveUnitSet)
            {
                if (tiles[ActiveTile].distance <= tiles[ActiveUnit].unit.CurrentMovement && tiles[ActiveTile].distance != -1)
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
                    UIManager.battlePanel.SetActive(true);
                    UIManager.DisplayFightStats(tiles[ActiveUnit].unit, tiles[ActiveTile].unit);
                    ActionsDisabled = true;
                    
                    // ActiveUnit = attacker, ActiveTile = defender
                    
                }
                
                isActiveUnitSet = false;
            }
            else
            {
                if (tiles[ActiveTile].unit.Player != battleManager.playerOnTurn)
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

        if (newTile.terrain == Tile.TerrainType.village)
        {
            newTile.unit.CurrentMovement = 0;
            if (((Village)newTile).Player != null)
                ((Village)newTile).Player.controlledVillages--;
            ((Village)newTile).Player = battleManager.playerOnTurn;
            ((Village)newTile).Player.controlledVillages++;
        }

        newTile.unit.setDefense(newTile.terrain);
        newTile.unit.circle.color = (newTile.unit.CurrentMovement == 0) ? Color.red : Color.yellow;    
    }

    private void fillGenerateMap()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                generateMap[i, j] = grass;
            }
        }

        generateMap[9, 0] = road;
        generateMap[9, 1] = road;
        generateMap[8, 0] = road;
        generateMap[8, 1] = road;

        generateMap[0, 19] = road;
        generateMap[1, 19] = road;
        generateMap[0, 18] = road;
        generateMap[1, 18] = road;

        generateMap[0, 0] = water;
        generateMap[0, 1] = water;
        generateMap[1, 2] = water;
        generateMap[1, 3] = water;
        generateMap[2, 4] = water;
        generateMap[2, 5] = water;
        generateMap[3, 6] = water;
        generateMap[4, 8] = water;
        generateMap[4, 9] = water;
        generateMap[5, 10] = water;
        generateMap[5, 11] = water;
        generateMap[6, 13] = water;
        generateMap[7, 14] = water;
        generateMap[7, 15] = water;
        generateMap[8, 16] = water;
        generateMap[8, 17] = water;
        generateMap[9, 18] = water;
        generateMap[9, 19] = water;

        generateMap[8, 5] = village;
        generateMap[4, 1] = village;
        generateMap[5, 18] = village;
        generateMap[1, 14] = village;


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
            t.GetComponent<SpriteRenderer>().color = tileColor;
        }
    }
}
