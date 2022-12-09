using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet
{
    public List<Tile> tiles = new List<Tile>();
    public List<List<Tile>> pathTiles = new List<List<Tile>>();
    public List<Tile> spawnTiles = new List<Tile>();
    public Tile endTile;
    public List<(byte start, byte end)> DirCardinals = new List<(byte start, byte end)>();
    public byte height, width;
}
