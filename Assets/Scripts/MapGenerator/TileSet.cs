using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet
{
    public List<Tile> tiles = new List<Tile>();
    public Tile startTile, endTile;
    public (int start, int end) DirCardinals;   // 0 for bottom, 1 for right, 2 for top, 3 for left
    public int height, width;
}
