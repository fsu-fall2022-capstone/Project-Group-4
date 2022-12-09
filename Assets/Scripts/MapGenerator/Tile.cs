using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public (byte x, byte y) position; // position info
    public byte type; // 0 initialized, 1 path, 2 start, 3 end

    public Tile() { }
    public Tile((byte x, byte y) newPosition, byte newType)
    {
        position = newPosition;
        type = newType;
    }

    public override string ToString()
    {
        return "position: " + position + " type: " + type;
    }
}
