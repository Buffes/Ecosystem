using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Ecosystem.Grid
{
    public class TilesAssetsToTilemap
    {
        private List<Vector3Int> tilePositions;
        private int [,] tiles;

        public TilesAssetsToTilemap ()
        {
            this.tilePositions = GameZone.tilePositions;
            this.tiles = GameZone.tiles;

            SetupTiles(tilePositions);
        }

        private void SetupTiles(List<Vector3Int> positions) 
        {
            for(int i = 0; i < tiles.GetLength(0); i++) 
            {
                for(int j = 0; j < tiles.GetLength(1); j++) 
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    int currentpos = tiles[i,j];
                    SetTileToTilemap(pos, currentpos);
                }
            }
        }

        private void SetTileToTilemap (Vector3Int pos, int currentpos)
        {
            var deepTile = TilesResourceLoader.GetDeepTile();
            var shallowDeepCurvedOut0 = TilesResourceLoader.GetShallowDeepCurveOut0Tile();
            var shallowDeepCurvedOut1 = TilesResourceLoader.GetShallowDeepCurveOut1Tile();
            var shallowDeepCurvedOut2 = TilesResourceLoader.GetShallowDeepCurveOut2Tile();
            var shallowDeepCurvedOut3 = TilesResourceLoader.GetShallowDeepCurveOut3Tile();
            var shallowDeepCurvedIn0 = TilesResourceLoader.GetShallowDeepCurveIn0Tile();
            var shallowDeepCurvedIn1 = TilesResourceLoader.GetShallowDeepCurveIn1Tile();
            var shallowDeepCurvedIn2 = TilesResourceLoader.GetShallowDeepCurveIn2Tile();
            var shallowDeepCurvedIn3 = TilesResourceLoader.GetShallowDeepCurveIn3Tile();
            var shallowDeepHorizontal0 = TilesResourceLoader.GetShallowDeepHorizontal0Tile();
            var shallowDeepHorizontal1 = TilesResourceLoader.GetShallowDeepHorizontal1Tile();
            var shallowDeepHorizontal2 = TilesResourceLoader.GetShallowDeepHorizontal2Tile();
            var shallowDeepHorizontal3 = TilesResourceLoader.GetShallowDeepHorizontal3Tile();
            var shallowDeepStraight0 = TilesResourceLoader.GetShallowDeepStraight0Tile();
            var shallowDeepStraight1 = TilesResourceLoader.GetShallowDeepStraight1Tile();
            var shallowDeepStraight2 = TilesResourceLoader.GetShallowDeepStraight2Tile();
            var shallowDeepStraight3 = TilesResourceLoader.GetShallowDeepStraight3Tile();
            var shallowTile = TilesResourceLoader.GetShallowTile();
            var beachShallowCurvedOut0 = TilesResourceLoader.GetBeachShallowCurveOut0Tile();
            var beachShallowCurvedOut1 = TilesResourceLoader.GetBeachShallowCurveOut1Tile();
            var beachShallowCurvedOut2 = TilesResourceLoader.GetBeachShallowCurveOut2Tile();
            var beachShallowCurvedOut3 = TilesResourceLoader.GetBeachShallowCurveOut3Tile();
            var beachShallowCurvedIn0 = TilesResourceLoader.GetBeachShallowCurveIn0Tile();
            var beachShallowCurvedIn1 = TilesResourceLoader.GetBeachShallowCurveIn1Tile();
            var beachShallowCurvedIn2 = TilesResourceLoader.GetBeachShallowCurveIn2Tile();
            var beachShallowCurvedIn3 = TilesResourceLoader.GetBeachShallowCurveIn3Tile();
            var beachShallowHorizontal0 = TilesResourceLoader.GetBeachShallowHorizontal0Tile();
            var beachShallowHorizontal1 = TilesResourceLoader.GetBeachShallowHorizontal1Tile();
            var beachShallowHorizontal2 = TilesResourceLoader.GetBeachShallowHorizontal2Tile();
            var beachShallowHorizontal3 = TilesResourceLoader.GetBeachShallowHorizontal3Tile();
            var beachShallowStraight0 = TilesResourceLoader.GetBeachShallowStraight0Tile();
            var beachShallowStraight1 = TilesResourceLoader.GetBeachShallowStraight1Tile();
            var beachShallowStraight2 = TilesResourceLoader.GetBeachShallowStraight2Tile();
            var beachShallowStraight3 = TilesResourceLoader.GetBeachShallowStraight3Tile();
            var beachTile = TilesResourceLoader.GetBeachTile();
            var grassBeachCurvedOut0 = TilesResourceLoader.GetGrassBeachCurveOut0Tile();
            var grassBeachCurvedOut1 = TilesResourceLoader.GetGrassBeachCurveOut1Tile();
            var grassBeachCurvedOut2 = TilesResourceLoader.GetGrassBeachCurveOut2Tile();
            var grassBeachCurvedOut3 = TilesResourceLoader.GetGrassBeachCurveOut3Tile();
            var grassBeachCurvedIn0 = TilesResourceLoader.GetGrassBeachCurveIn0Tile();
            var grassBeachCurvedIn1 = TilesResourceLoader.GetGrassBeachCurveIn1Tile();
            var grassBeachCurvedIn2 = TilesResourceLoader.GetGrassBeachCurveIn2Tile();
            var grassBeachCurvedIn3 = TilesResourceLoader.GetGrassBeachCurveIn3Tile();
            var grassBeachHorizontal0 = TilesResourceLoader.GetGrassBeachHorizontal0Tile();
            var grassBeachHorizontal1 = TilesResourceLoader.GetGrassBeachHorizontal1Tile();
            var grassBeachHorizontal2 = TilesResourceLoader.GetGrassBeachHorizontal2Tile();
            var grassBeachHorizontal3 = TilesResourceLoader.GetGrassBeachHorizontal3Tile();
            var grassBeachStraight0 = TilesResourceLoader.GetGrassBeachStraight0Tile();
            var grassBeachStraight1 = TilesResourceLoader.GetGrassBeachStraight1Tile();
            var grassBeachStraight2 = TilesResourceLoader.GetGrassBeachStraight2Tile();
            var grassBeachStraight3 = TilesResourceLoader.GetGrassBeachStraight3Tile();
            var grassTile = TilesResourceLoader.GetGrassTile();

            switch (currentpos) 
            {
                case 0:
                    GameZone.tilemap.SetTile(pos, deepTile);
                    break;
                case 1:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedOut0);
                    break;
                case 2:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedOut1);
                    break;
                case 3:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedOut2);
                    break;
                case 4:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedOut3);
                    break;
                case 5:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedIn0);
                    break;
                case 6:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedIn1);
                    break;
                case 7:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedIn2);
                    break;
                case 8:
                    GameZone.tilemap.SetTile(pos, shallowDeepCurvedIn3);
                    break;
                case 9:
                    GameZone.tilemap.SetTile(pos, shallowDeepStraight0);
                    break;
                case 10:
                    GameZone.tilemap.SetTile(pos, shallowDeepStraight1);
                    break;
                case 11:
                    GameZone.tilemap.SetTile(pos, shallowDeepStraight2);
                    break;
                case 12:
                    GameZone.tilemap.SetTile(pos, shallowDeepStraight3);
                    break;
                case 13:
                    GameZone.tilemap.SetTile(pos, shallowDeepHorizontal0);
                    break;
                case 14:
                    GameZone.tilemap.SetTile(pos, shallowDeepHorizontal1);
                    break;
                case 15:
                    GameZone.tilemap.SetTile(pos, shallowDeepHorizontal2);
                    break;
                case 16:
                    GameZone.tilemap.SetTile(pos, shallowDeepHorizontal3);
                    break;
                case 17:
                    GameZone.tilemap.SetTile(pos,shallowTile);
                    break;
                case 18:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedOut0);
                    break;
                case 19:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedOut1);
                    break;
                case 20:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedOut2);
                    break;
                case 21:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedOut3);
                    break;
                case 22:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedIn0);
                    break;
                case 23:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedIn1);
                    break;
                case 24:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedIn2);
                    break;
                case 25:
                    GameZone.tilemap.SetTile(pos, beachShallowCurvedIn3);
                    break;
                case 26:
                    GameZone.tilemap.SetTile(pos, beachShallowStraight0);
                    break;
                case 27:
                    GameZone.tilemap.SetTile(pos, beachShallowStraight1);
                    break;
                case 28:
                    GameZone.tilemap.SetTile(pos, beachShallowStraight2);
                    break;
                case 29:
                    GameZone.tilemap.SetTile(pos, beachShallowStraight3);
                    break;
                case 30:
                    GameZone.tilemap.SetTile(pos, beachShallowHorizontal0);
                    break;
                case 31:
                    GameZone.tilemap.SetTile(pos, beachShallowHorizontal1);
                    break;
                case 32:
                    GameZone.tilemap.SetTile(pos, beachShallowHorizontal2);
                    break;
                case 33:
                    GameZone.tilemap.SetTile(pos, beachShallowHorizontal3);
                    break;
                case 34:
                    GameZone.tilemap.SetTile(pos, beachTile);
                    break;
                case 35:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedOut0);
                    break;
                case 36:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedOut1);
                    break;
                case 37:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedOut2);
                    break;
                case 38:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedOut3);
                    break;
                case 39:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedIn0);
                    break;
                case 40:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedIn1);
                    break;
                case 41:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedIn2);
                    break;
                case 42:
                    GameZone.tilemap.SetTile(pos, grassBeachCurvedIn3);
                    break;
                case 43:
                    GameZone.tilemap.SetTile(pos, grassBeachStraight0);
                    break;
                case 44:
                    GameZone.tilemap.SetTile(pos, grassBeachStraight1);
                    break;
                case 45:
                    GameZone.tilemap.SetTile(pos, grassBeachStraight2);
                    break;
                case 46:
                    GameZone.tilemap.SetTile(pos, grassBeachStraight3);
                    break;
                case 47:
                    GameZone.tilemap.SetTile(pos, grassBeachHorizontal0);
                    break;
                case 48:
                    GameZone.tilemap.SetTile(pos, grassBeachHorizontal1);
                    break;
                case 49:
                    GameZone.tilemap.SetTile(pos, grassBeachHorizontal2);
                    break;
                case 50:
                    GameZone.tilemap.SetTile(pos, grassBeachHorizontal3);
                    break;
                case 51:
                    GameZone.tilemap.SetTile(pos, grassTile);
                    break;
            }
        }
    }
}
