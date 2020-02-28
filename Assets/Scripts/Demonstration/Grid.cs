/* Largely copied from Code Monkey */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        this.Width = width;
        this.Height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < Width && y < Height) {
            return gridArray[x, y];
        } else {
            return default(TGridObject);
        }
    }
}
