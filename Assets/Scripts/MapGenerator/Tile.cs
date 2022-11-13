using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public (int x, int y) position; // position info
    public int type; // 0 initialized, 1 path, 2 start, 3 end

    public Tile() {}
    public Tile((int x, int y) newPosition, int newType) {
        position = newPosition;
        type = newType;
    } 

    public override string ToString() {
        return "position: " + position + " type: " + type;
    }
}
