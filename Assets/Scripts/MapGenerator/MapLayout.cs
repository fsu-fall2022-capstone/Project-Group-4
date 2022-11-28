using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout
{
    public (int x, int y) position;
    public int tileSetNum;
    public int initPathID; // this is basically the path number of where the tileset starts from
    /*
              1 <---- 1
                      |
                      |
    0 ----> 0 ----> 0 |----->0
    */

    // each path will have the full length of the path up until it splits off
    // so the question would be to have a list of updatable expansion paths? lists of where the map generation can expand to?

    public List<(int id, int start)> relevantPaths = new List<(int, int)>();

    public MapLayout() { }
    public MapLayout((int x, int y) newPosition, int newTileSetNum, int newPathID)
    {
        position = newPosition;
        tileSetNum = newTileSetNum;
        initPathID = newPathID;
    }

    public override string ToString()
    {
        return "position: " + position + " tileSetNum: " + tileSetNum + " initPathID: " + initPathID + $" relevantPaths: {relevantPaths[0].id} {relevantPaths[0].start}";
    }
}
