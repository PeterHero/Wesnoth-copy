using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I used some of this website's code to handle hex coordinates systems https://www.redblobgames.com/grids/hexagons/

public struct HexCoordinates
{
    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x;
    public int y;
}
public struct OffsetCoordinates
{
    public OffsetCoordinates(int col, int row)
    {
        this.col = col;
        this.row = row;
    }

    public int col;
    public int row;
}

public static class HexMap
{
    // this method is copied from the website
    public static OffsetCoordinates AxialToOddq(HexCoordinates hex)
    {
        int col = hex.x;
        int row = hex.y + (hex.x - (hex.x & 1)) / 2;
        return new OffsetCoordinates(col, row);
    }

    // this method is copied from the website
    public static HexCoordinates OddqToAxial(OffsetCoordinates hex)
    {
        int x = hex.col;
        int y = hex.row - (hex.col - (hex.col & 1)) / 2;
        return new HexCoordinates(x, y);
    }

    public static List<HexCoordinates> GetAdjencentTiles(HexCoordinates thisTile)
    {
        List<HexCoordinates> adjencentTiles = new List<HexCoordinates>();

        int[,] vectors = { { 0, 1},
                           { 1, 0},
                           { 1, -1},
                           { 0, -1},
                           { -1, 0},
                           { -1, 1}};

        for (int i = 0; i < 6; i++)
        {
            adjencentTiles.Add(new HexCoordinates(thisTile.x + vectors[i, 0], thisTile.y + vectors[i, 1]));
        }

        return adjencentTiles;
    }

    public static List<Vector2Int> GetAdjencentTiles(Vector2Int thisTile)
    {
        OffsetCoordinates thisCoord = new OffsetCoordinates(thisTile.y, thisTile.x);
        HexCoordinates thisHex = OddqToAxial(thisCoord);

        List<HexCoordinates> adjecentHexes = GetAdjencentTiles(thisHex);
        List<Vector2Int> adjecentTiles = new List<Vector2Int>();

        foreach (var hex in adjecentHexes)
        {
            adjecentTiles.Add(new Vector2Int(AxialToOddq(hex).row, AxialToOddq(hex).col));
        }

        return adjecentTiles;
    }

    public static void FindDistances(Vector2Int startingTile, Dictionary<Vector2Int, Tile> tiles, Unit unit, GridManager gridManager)
    {
        List<Tile> openTiles = new List<Tile>();

        foreach (Tile tile in tiles.Values)
        {
            tile.distance = -1;
            tile.tileState = Tile.TileState.unseen;
        }

        tiles[startingTile].distance = 0;
        tiles[startingTile].tileState = Tile.TileState.open;
        openTiles.Add(tiles[startingTile]);

        while (openTiles.Count != 0)
        {
            openTiles.Sort(new TileComparer());
            Tile currentTile = openTiles[0];
            openTiles.RemoveAt(0);
            List<Vector2Int> adjecentTiles = GetAdjencentTiles(currentTile.coordinates);

            bool skip = false;

            // check if tile is controlled
            foreach (Vector2Int tileCoord in adjecentTiles)
            {
                if (currentTile.unit == unit)
                    break;

                Tile adjecentTile = gridManager.GetTileAtPosition(tileCoord);

                if (adjecentTile == null)
                    continue;

                if (adjecentTile.unit != null && adjecentTile.unit.Player != unit.Player)
                {
                    if (currentTile.distance < unit.CurrentMovement)
                        currentTile.distance = unit.CurrentMovement;
                    currentTile.tileState = Tile.TileState.closed;
                    skip = true;
                    break;
                }
            }

            if (skip)
                continue;

            // improve distance to adjecent tiles
            foreach (Vector2Int tileCoord in adjecentTiles)
            {
                Tile adjecentTile = gridManager.GetTileAtPosition(tileCoord);

                if (adjecentTile == null)
                    continue;

                int possibleDistance = currentTile.distance + unit.movementCost[adjecentTile.terrain];
                if (adjecentTile.tileState == Tile.TileState.unseen)
                {
                    adjecentTile.tileState = Tile.TileState.open;
                    openTiles.Add(adjecentTile);
                    adjecentTile.distance = possibleDistance;
                }
                else if (adjecentTile.tileState == Tile.TileState.open)
                {
                    if (possibleDistance < adjecentTile.distance)
                    {
                        adjecentTile.distance = possibleDistance;
                    }
                }
            }

            currentTile.tileState = Tile.TileState.closed;
        }

    }


}
