using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilesResourceLoader {

    public static Tile GetBeachTile () {
        return GetTileByName("beach");
    }

    public static Tile GetBeachShallowCurveIn0Tile () {
        return GetTileByName("beach_shallow_curve_in_0");
    }

    public static Tile GetBeachShallowCurveIn1Tile () {
        return GetTileByName("beach_shallow_curve_in_1");
    }

    public static Tile GetBeachShallowCurveIn2Tile () {
        return GetTileByName("beach_shallow_curve_in_2");
    }

    public static Tile GetBeachShallowCurveIn3Tile () {
        return GetTileByName("beach_shallow_curve_in_3");
    }

    public static Tile GetBeachShallowCurveOut0Tile () {
        return GetTileByName("beach_shallow_curve_out_0");
    }

    public static Tile GetBeachShallowCurveOut1Tile () {
        return GetTileByName("beach_shallow_curve_out_1");
    }

    public static Tile GetBeachShallowCurveOut2Tile () {
        return GetTileByName("beach_shallow_curve_out_2");
    }

    public static Tile GetBeachShallowCurveOut3Tile () {
        return GetTileByName("beach_shallow_curve_out_3");
    }

    public static Tile GetBeachShallowHorizontal0Tile () 
    {
        return GetTileByName("beach_shallow_horizontal_0");
    }

    public static Tile GetBeachShallowHorizontal1Tile () 
    {
        return GetTileByName("beach_shallow_horizontal_1");
    }

    public static Tile GetBeachShallowHorizontal2Tile () 
    {
        return GetTileByName("beach_shallow_horizontal_2");
    }

    public static Tile GetBeachShallowHorizontal3Tile () 
    {
        return GetTileByName("beach_shallow_horizontal_3");
    }

    public static Tile GetBeachShallowStraight0Tile () {
        return GetTileByName("beach_shallow_straight_0");
    }

    public static Tile GetBeachShallowStraight1Tile () {
        return GetTileByName("beach_shallow_straight_1");
    }

    public static Tile GetBeachShallowStraight2Tile () {
        return GetTileByName("beach_shallow_straight_2");
    }

    public static Tile GetBeachShallowStraight3Tile () {
        return GetTileByName("beach_shallow_straight_3");
    }

    public static Tile GetDeepTile () {
        return GetTileByName("deep");
    }

    public static Tile GetGrassTile () {
        return GetTileByName("grass");
    }

    public static Tile GetGrassBeachCurveIn0Tile () {
        return GetTileByName("grass_beach_curve_in_0");
    }

    public static Tile GetGrassBeachCurveIn1Tile () {
        return GetTileByName("grass_beach_curve_in_1");
    }

    public static Tile GetGrassBeachCurveIn2Tile () {
        return GetTileByName("grass_beach_curve_in_2");
    }

    public static Tile GetGrassBeachCurveIn3Tile () {
        return GetTileByName("grass_beach_curve_in_3");
    }

    public static Tile GetGrassBeachCurveOut0Tile () {
        return GetTileByName("grass_beach_curve_out_0");
    }

    public static Tile GetGrassBeachCurveOut1Tile () {
        return GetTileByName("grass_beach_curve_out_1");
    }

    public static Tile GetGrassBeachCurveOut2Tile () {
        return GetTileByName("grass_beach_curve_out_2");
    }

    public static Tile GetGrassBeachCurveOut3Tile () {
        return GetTileByName("grass_beach_curve_out_3");
    }

    public static Tile GetGrassBeachHorizontal0Tile () 
    {
        return GetTileByName("grass_beach_horizontal_0");
    }

    public static Tile GetGrassBeachHorizontal1Tile () 
    {
        return GetTileByName("grass_beach_horizontal_1");
    }

    public static Tile GetGrassBeachHorizontal2Tile () 
    {
        return GetTileByName("grass_beach_horizontal_2");
    }

    public static Tile GetGrassBeachHorizontal3Tile () 
    {
        return GetTileByName("grass_beach_horizontal_3");
    }

    public static Tile GetGrassBeachStraight0Tile () {
        return GetTileByName("grass_beach_straight_0");
    }

    public static Tile GetGrassBeachStraight1Tile () {
        return GetTileByName("grass_beach_straight_1");
    }

    public static Tile GetGrassBeachStraight2Tile () {
        return GetTileByName("grass_beach_straight_2");
    }

    public static Tile GetGrassBeachStraight3Tile () {
        return GetTileByName("grass_beach_straight_3");
    }

    public static Tile GetShallowTile () {
        return GetTileByName("shallow");
    }

    public static Tile GetShallowDeepCurveIn0Tile () {
        return GetTileByName("shallow_deep_curve_in_0");
    }

    public static Tile GetShallowDeepCurveIn1Tile () {
        return GetTileByName("shallow_deep_curve_in_1");
    }

    public static Tile GetShallowDeepCurveIn2Tile () {
        return GetTileByName("shallow_deep_curve_in_2");
    }

    public static Tile GetShallowDeepCurveIn3Tile () {
        return GetTileByName("shallow_deep_curve_in_3");
    }

    public static Tile GetShallowDeepCurveOut0Tile () {
        return GetTileByName("shallow_deep_curve_out_0");
    }

    public static Tile GetShallowDeepCurveOut1Tile () {
        return GetTileByName("shallow_deep_curve_out_1");
    }

    public static Tile GetShallowDeepCurveOut2Tile () {
        return GetTileByName("shallow_deep_curve_out_2");
    }

    public static Tile GetShallowDeepCurveOut3Tile () {
        return GetTileByName("shallow_deep_curve_out_3");
    }

    public static Tile GetShallowDeepHorizontal0Tile () 
    {
        return GetTileByName("shallow_deep_horizontal_0");
    }

    public static Tile GetShallowDeepHorizontal1Tile () 
    {
        return GetTileByName("shallow_deep_horizontal_1");
    }

    public static Tile GetShallowDeepHorizontal2Tile () 
    {
        return GetTileByName("shallow_deep_horizontal_2");
    }

    public static Tile GetShallowDeepHorizontal3Tile () 
    {
        return GetTileByName("shallow_deep_horizontal_3");
    }

    public static Tile GetShallowDeepStraight0Tile () {
        return GetTileByName("shallow_deep_straight_0");
    }

    public static Tile GetShallowDeepStraight1Tile () {
        return GetTileByName("shallow_deep_straight_1");
    }

    public static Tile GetShallowDeepStraight2Tile () {
        return GetTileByName("shallow_deep_straight_2");
    }

    public static Tile GetShallowDeepStraight3Tile () {
        return GetTileByName("shallow_deep_straight_3");
    }

    private static Tile GetTileByName(string name) {
        return (Tile) Resources.Load(name, typeof(Tile));
    }




   
}
