using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BresenhamTest : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap = default;
    [SerializeField]
    private Tile goodTile = default;
    [SerializeField]
    private Tile badTile = default;

    [Header("Input")]
    [SerializeField]
    private Vector2Int start = default;
    [SerializeField]
    private Vector2Int end = default;
    [SerializeField]
    private Vector2Int[] blockedCells = default;

    public void Draw()
    {
        tilemap.ClearAllTiles();
        DrawBresenhamLine(start.x, start.y, end.x, end.y);
    }

    private bool DrawBresenhamLine(int x0, int y0, int x1, int y1)
    {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;
        int prevX = x0, prevY = y0;
        for (; ; )
        {
            Plot(x0, y0);
            if (!CheckCell(x0, y0) || !CheckDiagonal(prevX, prevY, x0, y0)) return false;
            if (x0 == x1 && y0 == y1) return true;
            prevX = x0;
            prevY = y0;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        }
    }

    private void Plot(int x, int y) => tilemap.SetTile(new Vector3Int(x, y, 0), goodTile);

    private void PlotBlock(int x, int y) => tilemap.SetTile(new Vector3Int(x, y, 0), badTile);

    private bool CheckDiagonal(int x0, int y0, int x1, int y1)
    {
        int dx = x1 - x0;
        int dy = y1 - y0;
        if (dx == 0 || dy == 0) return true;
        return CheckCell(x1 - dx, y1) || CheckCell(x1, y1 - dy);
    }

    private bool CheckCell(int x, int y)
    {
        foreach (Vector2Int blockedCell in blockedCells)
        {
            if (blockedCell.x == x && blockedCell.y == y)
            {
                PlotBlock(x, y);
                return false;
            }
        }
        return true;
    }
}
