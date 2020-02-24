using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilesResourceLoader {

    private const string Water = "beach";
    private const string Grass = "grass";

    public static Tile GetWaterTile () {
        return GetTileByName(Water);
    }

    public static Tile GetGrassTile () {
        return GetTileByName(Grass);
    }

    private static Tile GetTileByName(string name) {
        return (Tile) Resources.Load(name, typeof(Tile));
    }




   
}
