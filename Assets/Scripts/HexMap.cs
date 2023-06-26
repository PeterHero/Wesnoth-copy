using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class HexMap
{
    public static OffsetCoordinates AxialToOddq(HexCoordinates hex)
    {
        int col = hex.x;
        int row = hex.y + (hex.x - (hex.x & 1)) / 2;
        return new OffsetCoordinates(col, row);
    }

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

}
