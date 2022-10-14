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

    public GameObject pathTile; // straight

    public GameObject portalTile; // enemy spawn tile
    public GameObject homeTile; // player home point tile

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

    // may be needed
    //private (bool x, bool y) reached = (false, false);
    //private GameObject currentTile;
    //private int currIndex;
    //private int nextIndex;

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
        Tile genStichTile = new Tile(tileSets[tileSets.Count-1].startTile.position, 3);
        TileSetGenerator newTileSet = new TileSetGenerator(tilesetWidth, tilesetHeight, genStichTile);
    }

    private void generateMap()
    {   // generates the initial map
        TileSetGenerator newTileSet = new TileSetGenerator(tilesetWidth, tilesetHeight);
        Debug.Log($"{newTileSet.ToString()}");
        tileSets.Add(newTileSet.getTileSet());
    }
}