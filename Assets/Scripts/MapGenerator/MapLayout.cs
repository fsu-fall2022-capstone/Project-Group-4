using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout
{
    public (int x, int y) position;

    public MapLayout() {}
    public MapLayout((int x, int y) newPosition) {
        position = newPosition;
    }
}
