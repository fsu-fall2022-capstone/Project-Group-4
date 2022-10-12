using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator main; // for access by other systems

    // individual sprite visuals for map generation
    public GameObject mapTile1; // standard
    public GameObject mapTile2; // flora1
    public GameObject mapTile3; // flora2

    public GameObject pathTile1; // straight
    public GameObject pathTile2; // left turn
    public GameObject pathTile3; // right turn

    public GameObject portalTileNS; // enemy spawn point tile | N&S
    public GameObject portalTileWE; // W&E
    public GameObject homeTileNS; // end point tile | N&S
    public GameObject homeTileWE; // W&E

    [SerializeField] private int tilesetWidth;
    [SerializeField] private int tilesetHeight;
    private (int width, int height) mapSize;

    [SerializeField] private Color pathColor;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();
    public static List<TileSet> tileSets = new List<TileSet>();
    //public static List<GameObject> resourceTiles = new List<GameObject>();

    public static GameObject startTile { get; private set; } // starting position of enemy
    public static GameObject endTile { get; private set; } // home position of player

    // for path generation
    private (bool x, bool y) reached = (false, false);
    private GameObject currentTile;
    private int currIndex;
    private int nextIndex;

    private void Start()
    {
        if (main == null) main = this;
        mapSize.width = tilesetWidth;
        mapSize.height = tilesetHeight;
        generateMap();
    }

    private void Update()
    {

    }

    public void expandMap()
    {   // expands the map
        TileSetGenerator newTileSet = new TileSetGenerator(tilesetWidth, tilesetHeight);

    }

    private void generateMap()
    {   // generates the initial map
        TileSetGenerator newTileSet = new TileSetGenerator(tilesetWidth, tilesetHeight);
        newTileSet.generateTileset;
        (int x, int y) start = newTileSet.getStartCoord();
        (int x, int y) end = newTileSet.getEndCoord();
    }
}