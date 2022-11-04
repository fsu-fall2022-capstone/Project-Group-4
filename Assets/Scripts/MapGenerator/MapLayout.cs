using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout
{
    public (int x, int y) position;
    public int tileSetNum;

    public MapLayout() {}
    public MapLayout((int x, int y) newPosition, int newTileSetNum) {
        position = newPosition;
        tileSetNum = newTileSetNum;
    }
}
