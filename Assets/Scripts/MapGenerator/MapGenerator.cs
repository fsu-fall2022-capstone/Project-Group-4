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
    public static List<MapLayout> mapLayout = new List<MapLayout>();
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
        MapLayout locTileInfo = new MapLayout();
        Tile genStichTile = new Tile(tileSets[tileSets.Count-1].startTile.position, 3);
        TileSetGenerator tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, genStichTile);
        Debug.Log($"{tileSetGen.ToString()}");
        TileSet newTileSet = tileSetGen.getTileSet();

        (int start, int end) newDir = newTileSet.DirCardinals;
        (int x, int y) prevLayoutPos = mapLayout[mapLayout.Count-1].position;
    }

    private void generateMap()
    {   // generates the initial map
        MapLayout locTileInfo = new MapLayout();
        TileSetGenerator tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight);
        Debug.Log($"{tileSetGen.ToString()}");
        TileSet newTileSet = tileSetGen.getTileSet();
        tileSets.Add(newTileSet);
        
        locTileInfo.position = (0, 0);
        mapLayout.Add(locTileInfo);

        for(int i = 0; i < newTileSet.tiles.Count; i++) {
            Tile currTile = newTileSet.tiles[i];
            Vector3 tilePos = new Vector3(currTile.position.x, currTile.position.y, 0);
            if(currTile.type == 0) {
                // can randomize between mapTile1, mapTile2, mapTile3 here if needed
                GameObject newTile = Instantiate(mapTile1, tilePos, Quaternion.identity);
                mapTiles.Add(newTile);
            }
        }

        for(int i = 0; i < newTileSet.pathTiles.Count; i++) {
            Tile currTile = newTileSet.pathTiles[i];
            Vector3 tilePos = new Vector3(currTile.position.x, currTile.position.y, 0);
            switch(currTile.type) {
                case 1:
                    GameObject newPathTile = Instantiate(pathTile, tilePos, Quaternion.identity);
                    pathTiles.Add(newPathTile);
                    break;
                case 2:
                    // the sprite will be rotated to face the correct direction based on the cardinal direction
                    // though this is still a work in progress as we actually need an actual sprite to rotate
                    GameObject newStartTile = Instantiate(portalTile, tilePos, Quaternion.identity);
                    pathTiles.Add(newStartTile);
                    startTile = newStartTile;
                    break;
                case 3:
                    GameObject newEndTile = Instantiate(homeTile, tilePos, Quaternion.identity);
                    pathTiles.Add(newEndTile);
                    endTile = newEndTile;
                    break;
            }
        }
    }
}