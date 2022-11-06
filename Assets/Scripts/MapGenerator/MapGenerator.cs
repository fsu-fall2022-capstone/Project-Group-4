using System.Linq;
using System.Reflection;
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

    [SerializeField] private bool generateAsIsometric = true;
    [SerializeField] private float spriteSize = 1f;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<List<GameObject>> pathTiles = new List<List<GameObject>>();
    public static List<TileSet> tileSets = new List<TileSet>();
    public static List<MapLayout> mapLayout = new List<MapLayout>();
    public static List<MapLayout> expandableTiles = new List<MapLayout>(); // keeps the data of where the map can expand to
    public static List<GameObject> spawnTiles = new List<GameObject>();

    public static GameObject startTile { get; private set; } // starting position of enemy
    public static GameObject endTile { get; private set; } // home position of player

    [SerializeField] private int maxDirectionalStraightness = 3;
    private int straightLineCounter = 0; // counter for map path, to prevent too many straight lines

    // may be needed
    //private (bool x, bool y) reached = (false, false);
    //private GameObject currentTile;
    //private int currIndex;
    //private int nextIndex;

    private void Start()
    {
        if (main == null) main = this;
        generateMap();
    }

    private void Update()
    {

    }

    public bool checkExpandability((int x, int y) tilePos) 
    {
        for(int i = 0; i < mapLayout.Count; i++)
        {
            if(mapLayout[i].position == tilePos)
            {
                return false;
            }
        }

        return true;
    }

    private void updateAvailableExpansionVectors() {
        foreach(MapLayout layout in mapLayout)
        {

        }
    }

    private void drawTileSet(TileSet newTileSet, (int x, int y) displacement, bool attachStitch = false, bool preRendered = false, int pathID = 0) {
        (float x, float y) newPos;
        GameObject newPathTile, newStartTile, newEndTile, newTile;

        if(!preRendered) { // if not pre-rendered, then we need to generate the tiles
            for(int i = 0; i < newTileSet.tiles.Count; i++) {
                Tile currTile = newTileSet.tiles[i];
                if(generateAsIsometric){
                    newPos.x = (((displacement.x) + currTile.position.x)
                     * spriteSize + ((displacement.y) + currTile.position.y) * spriteSize) / 2f;
                    newPos.y = ((((displacement.x) + currTile.position.x)
                     * spriteSize - ((displacement.y) + currTile.position.y) * spriteSize) / 4f) * -1;
                } else {
                    newPos.x = (displacement.x) + currTile.position.x;
                    newPos.y = (displacement.y) + currTile.position.y;
                }
                //Debug.Log($"New pos: {newPos.x}, {newPos.y}");
                Vector3 tilePos = new Vector3(newPos.x, newPos.y, 0);
                if(currTile.type == 0) {
                    // can randomize between mapTile1, mapTile2, mapTile3 here if needed
                    newTile = Instantiate(mapTile1, tilePos, Quaternion.identity);
                    mapTiles.Add(newTile);
                }
            }
        }

        if(attachStitch) {
            // old end tile info
            Vector3 oldPos = spawnTiles[pathID].transform.position;
            pathTiles[pathID].Remove(spawnTiles[pathID]);
            Destroy(spawnTiles[pathID]);

            // converting the end tile sprite to a path tile sprite
            GameObject replacedTile = Instantiate(pathTile, oldPos, Quaternion.identity);
            pathTiles[pathID].Add(replacedTile);
        }

        int newPathID = -1;
        for(int index = 0; index < newTileSet.pathTiles.Count; index++) {
            if(newTileSet.pathTiles.Count > 1 && newPathID == -1) {
                newPathID = pathTiles.Count;
                for(int i = 1; i < newTileSet.pathTiles.Count; i++) { // need to ready the pathTiles list for the new path
                    pathTiles.Add(new List<GameObject>());
                }
            }
            for(int i = newTileSet.pathTiles[index].Count - 1; i >= 0; i--) {
                Tile currTile = newTileSet.pathTiles[index][i];
                if(generateAsIsometric) {
                    newPos.x = (((displacement.x) + currTile.position.x)
                     * spriteSize + ((displacement.y) + currTile.position.y) * spriteSize) / 2f;
                    newPos.y = ((((displacement.x) + currTile.position.x)
                     * spriteSize - ((displacement.y) + currTile.position.y) * spriteSize) / 4f) * -1;
                } else {
                    newPos.x = (displacement.x) + currTile.position.x;
                    newPos.y = (displacement.y) + currTile.position.y;
                }

                //Debug.Log($"New pos: {newPos.x}, {newPos.y}");
                Vector3 tilePos = new Vector3(newPos.x, newPos.y, 0);

                // check to see if it already exists in a different pathTiles list
                bool alreadyExists = false;
                if(mapTiles.Any(x => x.transform.position == tilePos)) {
                    alreadyExists = true;
                    pathTiles[pathID+index].Add(mapTiles.Find(x => x.transform.position == tilePos));
                }

                if(!alreadyExists) {
                    switch(currTile.type) {
                        case 1: 
                            newPathTile = Instantiate(pathTile, tilePos, Quaternion.identity);
                            pathTiles[pathID].Add(newPathTile);
                            break;                
                        case 2:
                            // the sprite will be rotated to face the correct direction based on the cardinal direction
                            // though this is still a work in progress as we actually need an actual sprite to rotate
                            newStartTile = Instantiate(portalTile, tilePos, Quaternion.identity);
                            pathTiles[pathID].Add(newStartTile);
                            spawnTiles.Insert(pathID, newStartTile);
                            break;
                        case 3:
                            if (!attachStitch) {
                                newEndTile = Instantiate(homeTile, tilePos, Quaternion.identity);
                                pathTiles[pathID].Add(newEndTile);
                                endTile = newEndTile;
                            } else {
                                newPathTile = Instantiate(pathTile, tilePos, Quaternion.identity);
                                pathTiles[pathID].Add(newPathTile);
                            }
                            break;
                    }
                }
            }
            if(!newPathID != -1) {
                pathID = newPathID;
                newPathID++;
            }
        }
    }

    // uncomment if debugging in editor
    //public void ClearLog()
    //{
    //    var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //    var type = assembly.GetType("UnityEditor.LogEntries");
    //    var method = type.GetMethod("Clear");
    //    method.Invoke(new object(), null);
    //}

    public void expandMap(MapLayout locTileInfo)
    {   // expands the map
        if(expandableTiles.Count == 0) {
            Debug.Log("No expandable tilesets");
            return;
        } else if(!expandableTiles.Any(x => x.position == locTileInfo.position)) {
            Debug.Log("MapLayout isn't expandable");
            return;
        }

        int randomNum = Random.Range(0, 100);
        int randomPathCount;
        if (randomNum >= 0 && randomNum < 30) {
            randomPathCount = 2;
        } else if(randomNum >= 30 && randomNum < 40) {
            randomPathCount = 3;
        } else
            randomPathCount = 1;

        int tileSetNum = locTileInfo.tileSetNum;
        int pathID = locTileInfo.pathID;
        

        bool preRendered = false;
        for(int i = 0; i < randomPathCount; i++, preRendered = true) {
            drawTileSet(newTileSet, (locTileInfo.position.x * tilesetWidth, locTileInfo.position.y * tilesetHeight), true, preRendered, i);
        }
        updateAvailableExpansionVectors(); // updates the list for what's available to expand
    }

    private void generateMap()
    {   // generates the initial map
        MapLayout locTileInfo = new MapLayout();
        TileSetGenerator tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, numStartPoints: 1);

        Debug.Log($"{tileSetGen.ToString()}");
        tileSets.Add(tileSetGen.getTileSet());
        
        locTileInfo.position = (0, 0);
        locTileInfo.tileSetNum = 0;
        locTileInfo.pathID = 0;
        mapLayout.Add(locTileInfo);
        pathTiles.Add(new List<GameObject>());

        drawTileSet(tileSets[0], (0, 0));
    }
}