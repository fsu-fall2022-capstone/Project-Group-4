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

    private static int straightLineCounter = 0; // counter for map path, to prevent too many straight lines
    private int prevcount = 0;

    // may be needed
    //private (bool x, bool y) reached = (false, false);
    //private GameObject currentTile;
    //private int currIndex;
    //private int nextIndex;

    private void Start()
    {
        if (main == null) main = this;
        gameObject.AddComponent<MapRenderer>();
        generateMap();
    }

    private void Update()
    {
        if(prevcount != pathTiles.Count) {
            prevcount = pathTiles.Count;
            for(int i = 0; i < pathTiles.Count; i++) {
                string print = $"Path {i}";
                foreach(var tile in pathTiles[i]) {
                    print += $"{tile.transform.position} \n";
                }
                Debug.Log(print);
            }
        }
    }

    public static void clearMapGenerator() // this function is under the assumption
    { // that it's only being called when leaving or reloading the scene
        MapRenderer.activeRenderer = false;

        startTile = null;
        endTile = null;
        straightLineCounter = 0;

        mapTiles.Clear();
        pathTiles.Clear();
        tileSets.Clear();
        mapLayout.Clear();

        MapRenderer.activeRenderer = true;
    }

    public bool checkExpandability((int x, int y) lastTilePos, int start) 
    {
        (int x, int y) checkPos = (0,0);
        switch(start)
        {
            case 0: // bottom
                checkPos = (lastTilePos.x, lastTilePos.y - 1);
                break;
            case 1: // right
                checkPos = (lastTilePos.x + 1, lastTilePos.y);
                break;
            case 2: // top
                checkPos = (lastTilePos.x, lastTilePos.y + 1);
                break;
            case 3: // left
                checkPos = (lastTilePos.x - 1, lastTilePos.y);
                break;
        }

        for(int i = 0; i < mapLayout.Count; i++)
        {
            if(mapLayout[i].position == checkPos)
            {
                return false;
            }
        }

        return true;
    }

    private List<int> checkAvailableExpansionDirections(MapLayout layoutInfo) {
        // now at the valid point of expansion, time to check which start directions are available
        (int x, int y) lastTilePos = layoutInfo.position;
        int start = layoutInfo.relevantPaths[0].start;

        List<(int x, int y)> availableVectors = new List<(int x, int y)>();
        List<int> availableDirections = new List<int>();
        List<(int x, int y)> checkVectors = new List<(int x, int y)>();
        (int x, int y) validPos = (0,0);
        (int x, int y) checkPos = (0,0);
        (int x, int y) findPosDir = (0,0);

        switch(start)
        {
            case 0: // bottom
                validPos = (lastTilePos.x, lastTilePos.y - 1);
                if(straightLineCounter <= maxDirectionalStraightness)
                {
                    checkPos = (validPos.x, validPos.y - 1); // down
                    checkVectors.Add(checkPos);
                }
                checkPos = (validPos.x + 1, validPos.y); // right
                checkVectors.Add(checkPos);
                checkPos = (validPos.x - 1, validPos.y); // left
                checkVectors.Add(checkPos);
                break;
            case 1: // right
                validPos = (lastTilePos.x + 1, lastTilePos.y);
                if(straightLineCounter <= maxDirectionalStraightness)
                {
                    checkPos = (validPos.x + 1, validPos.y); // right
                    checkVectors.Add(checkPos);
                }
                checkPos = (validPos.x, validPos.y - 1); // down
                checkVectors.Add(checkPos);
                checkPos = (validPos.x, validPos.y + 1); // up
                break;
            case 2: // top
                validPos = (lastTilePos.x, lastTilePos.y + 1);
                if(straightLineCounter <= maxDirectionalStraightness)
                {
                    checkPos = (validPos.x, validPos.y + 1); // up
                    checkVectors.Add(checkPos);
                }
                checkPos = (validPos.x + 1, validPos.y); // right
                checkVectors.Add(checkPos);
                checkPos = (validPos.x - 1, validPos.y); // left
                checkVectors.Add(checkPos);
                break;
            case 3: // left
                validPos = (lastTilePos.x - 1, lastTilePos.y);
                if(straightLineCounter <= maxDirectionalStraightness){
                    checkPos = (validPos.x - 1, validPos.y); // left
                    checkVectors.Add(checkPos);
                }
                checkPos = (validPos.x, validPos.y - 1); // down
                checkVectors.Add(checkPos);
                checkPos = (validPos.x, validPos.y + 1); // up
                checkVectors.Add(checkPos);
                break;
        }

        for(int i = 0; i < checkVectors.Count; i++) {
            checkPos = checkVectors[i];
            if(!mapLayout.Any(x => x.position == checkPos)) {
                availableVectors.Add(checkPos);
            }
        }

        if(availableVectors.Count != 0){
            for(int i = 0; i < availableVectors.Count; i++) {
                findPosDir = availableVectors[i];
                if(findPosDir.x == validPos.x) {
                    if(findPosDir.y > validPos.y) {
                        availableDirections.Add(2);
                    } else {
                        availableDirections.Add(0);
                    }
                } else if(findPosDir.y == validPos.y) {
                    if(findPosDir.x > validPos.x) {
                        availableDirections.Add(1);
                    } else {
                        availableDirections.Add(3);
                    }
                }
            }
        }

        return availableDirections;
    }

    private void updateAvailableExpansionVectors() {
        expandableTiles.Clear();
        
        for(int id = 0; id < spawnTiles.Count; id++) {
            var lastPath = mapLayout.LastOrDefault(x => x.relevantPaths.Any(y => y.id == id));
            Debug.Log("lastPath: " + lastPath);
            if(lastPath != null) {
                int index = lastPath.relevantPaths.FindIndex(x => x.id == id);
                Debug.Log("index: " + index);
                if(checkExpandability(lastPath.position, lastPath.relevantPaths[index].start)) {
                    (int x, int y) lastTilePos = lastPath.position;
                    (int x, int y) newPos = (0,0);

                    Debug.Log("lastTilePos: " + lastTilePos);
                    
                    switch(lastPath.relevantPaths[index].start)
                    {
                        case 0: // bottom
                            newPos = (lastTilePos.x, lastTilePos.y - 1);
                            break;
                        case 1: // right
                            newPos = (lastTilePos.x + 1, lastTilePos.y);
                            break;
                        case 2: // top
                            newPos = (lastTilePos.x, lastTilePos.y + 1);
                            break;
                        case 3: // left
                            newPos = (lastTilePos.x - 1, lastTilePos.y);
                            break;
                    }

                    Debug.Log($"newPos: {newPos}");
                    MapLayout newLayout = new MapLayout(newPos, lastPath.tileSetNum, id);
                    foreach(var DirCardinal in tileSets[newLayout.tileSetNum].DirCardinals) {
                        Debug.Log($"DirCardinal: {DirCardinal}");
                    }

                    newLayout.relevantPaths.Add((id, tileSets[newLayout.tileSetNum].DirCardinals[index].start));

                    expandableTiles.Add(newLayout);
                }
            }
        }
        Debug.Log($"expandableTiles.Count: {expandableTiles.Count}");

        foreach(var tile in expandableTiles) {
            Debug.Log($"tile: {tile.ToString()}");
            Debug.Log($"spawnTile location: {spawnTiles[tile.relevantPaths[0].id].transform.position}");
        }
    }

    private void cleanExcessSpawns() {
        if(spawnTiles.Count > pathTiles.Count) {
            for(int i = spawnTiles.Count - 1; i >= pathTiles.Count; i--) {
                spawnTiles.RemoveAt(i);
            }
        }
    }

    private void drawMapTiles(TileSet newTileSet, (int x, int y) displacement) {
        GameObject newTile;
        (float x, float y) newPos;
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

    private void drawPathTiles(TileSet newTileSet, (int x, int y) displacement, bool attachStitch = false, int pathID = 0, int localID = 0) {
        (float x, float y) newPos;
        GameObject newPathTile, newStartTile, newEndTile;

        for(int i = newTileSet.pathTiles[localID].Count - 1; i >= 0; i--) {
            Tile currTile = newTileSet.pathTiles[localID][i];
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
            //bool alreadyExists = false;
            //if(mapTiles.Any(x => x.transform.position == tilePos)) {
            //    alreadyExists = true;
            //    pathTiles[pathID].Add(mapTiles.Find(x => x.transform.position == tilePos));
            //}

            //Debug.Log($"alreadyExists: {alreadyExists}");

            //if(!alreadyExists) {
                switch(currTile.type) {
                    case 1: 
                        newPathTile = Instantiate(pathTile, tilePos, Quaternion.identity);
                        pathTiles[pathID].Add(newPathTile);
                        mapTiles.Add(newPathTile);
                        break;                
                    case 2:
                        // the sprite will be rotated to face the correct direction based on the cardinal direction
                        // though this is still a work in progress as we actually need an actual sprite to rotate
                        newStartTile = Instantiate(portalTile, tilePos, Quaternion.identity);
                        pathTiles[pathID].Add(newStartTile);
                        mapTiles.Add(newStartTile);
                        Debug.Log($"newStartTile: {newStartTile}");
                        Debug.Log($"newStartTile.transform.position: {newStartTile.transform.position}");
                        spawnTiles.Insert(pathID, newStartTile);
                        Debug.Log($"spawnTiles.Count: {spawnTiles.Count}");
                        break;
                    case 3:
                        if (!attachStitch) {
                            newEndTile = Instantiate(homeTile, tilePos, Quaternion.identity);
                            pathTiles[pathID].Add(newEndTile);
                            mapTiles.Add(newEndTile);
                            endTile = newEndTile;
                        } else {
                            newPathTile = Instantiate(pathTile, tilePos, Quaternion.identity);
                            pathTiles[pathID].Add(newPathTile);
                            mapTiles.Add(newPathTile);
                        }
                        break;
                }
            //}
        }
    
        // reorder the mapTiles based on their position from bottom to top        
        mapTiles.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));
    }

    public void randomExpand() {
        int randomIndex = UnityEngine.Random.Range(0, expandableTiles.Count);
        MapLayout randomTile = expandableTiles[randomIndex];
        Debug.Log($"Chosen mapLayout: {randomTile.ToString()}");
        expandMap(randomTile);
    }

    public bool expandMap(MapLayout locTileInfo)
    {   // expands the map
        if(expandableTiles.Count == 0) {
            Debug.Log("No expandable tilesets");
            return false;
        } else if(!expandableTiles.Any(x => x.position == locTileInfo.position)) {
            Debug.Log("MapLayout isn't expandable");
            return false;
        }

        int randomNum = UnityEngine.Random.Range(0, 100);
        int randomPathCount;
        if (randomNum >= 0 && randomNum < 30) {
            randomPathCount = 2;
        } else if(randomNum >= 30 && randomNum < 40) {
            randomPathCount = 3;
        } else
            randomPathCount = 1;

        int initPathID = locTileInfo.initPathID;

        List<int> availableDirections = checkAvailableExpansionDirections(locTileInfo);

        int l_index = locTileInfo.relevantPaths[0].start;
        int index = tileSets[locTileInfo.tileSetNum].DirCardinals.FindIndex(x => x.start == l_index);
        Tile genStitchTile = tileSets[locTileInfo.tileSetNum].spawnTiles[index];
        Debug.Log($"Stitch: {genStitchTile.position.x}, {genStitchTile.position.y}");
        foreach(var tile in tileSets[locTileInfo.tileSetNum].spawnTiles) {
            Debug.Log($"Tile: {tile.ToString()}");
        }
        Debug.Log($"End Tile: {tileSets[locTileInfo.tileSetNum].endTile.ToString()}");

        TileSetGenerator tileSetGen;
        if(availableDirections.Count == 0 || availableDirections.Count == 3) { 
            // direction won't matter, just pick a random one
            Debug.Log("Any direction is available.");
            tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, genStitchTile, numStartPoints: randomPathCount);
        } else {
            int rand = UnityEngine.Random.Range(0, availableDirections.Count);
            Debug.Log($"Random number: {rand}");
            int randDir = availableDirections[rand];
            Debug.Log($"Random direction: {randDir}");
            tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, genStitchTile, randDir, randomPathCount);
        }

        TileSet newTileSet = tileSetGen.getTileSet();
        Debug.Log($"{tileSetGen.ToString()}");
        tileSets.Add(newTileSet);
        locTileInfo.tileSetNum = tileSets.Count - 1;

        Debug.Log($"Spawn Tiles Count @1 {spawnTiles.Count}");

        // old end tile info
        Debug.Log($"Removing index: {locTileInfo.initPathID}");
        Vector3 oldPos = spawnTiles[initPathID].transform.position;
        GameObject oldTile = spawnTiles[initPathID];
        spawnTiles.Remove(oldTile);
        pathTiles[initPathID].Remove(oldTile);
        mapTiles.Remove(oldTile);
        Destroy(oldTile);

        // converting the end tile sprite to a path tile sprite
        GameObject replacedTile = Instantiate(pathTile, oldPos, Quaternion.identity);
        pathTiles[initPathID].Add(replacedTile);
        mapTiles.Add(replacedTile);
        locTileInfo.relevantPaths.Clear();

        Debug.Log($"Spawn Tiles Count @2 {spawnTiles.Count}");

        drawMapTiles(newTileSet, (locTileInfo.position.x * tilesetWidth, locTileInfo.position.y * tilesetHeight));

        for(int i = 0; i < randomPathCount; i++) {
            int count = (i == 0) ? initPathID : pathTiles.Count; 
            Debug.Log($"Count: {count}");
            locTileInfo.relevantPaths.Add((count,newTileSet.DirCardinals[i].start));
            if(i != 0) {
                pathTiles.Add(new List<GameObject>(pathTiles[initPathID]));
            }
        }

        Debug.Log($"Spawn Tiles Count @3 {spawnTiles.Count}");

        for(int i = 0; i < randomPathCount; i++) {
            int pathID = locTileInfo.relevantPaths[i].id;
            drawPathTiles(newTileSet, (locTileInfo.position.x * tilesetWidth, locTileInfo.position.y * tilesetHeight), true, pathID, i);
        }

        Debug.Log($"Spawn Tiles Count @4 {spawnTiles.Count}");

        mapTiles.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));

        // updates overall map layout info
        mapLayout.Add(locTileInfo);
        Debug.Log($"MapLayout: {locTileInfo.position}");
        updateAvailableExpansionVectors(); // updates the list for what's available to expand
        cleanExcessSpawns();
        return true;
    }

    private void generateMap()
    {   // generates the initial map
        
        TileSetGenerator tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, numStartPoints: 1);

        Debug.Log($"{tileSetGen.ToString()}");
        tileSets.Add(tileSetGen.getTileSet());
        
        MapLayout locTileInfo = new MapLayout((0, 0),0,0);
        locTileInfo.relevantPaths.Add((0, tileSets[0].DirCardinals[0].start));
        mapLayout.Add(locTileInfo);
        pathTiles.Add(new List<GameObject>());

        drawMapTiles(tileSets[0], (0, 0));
        drawPathTiles(tileSets[0], (0, 0));
        updateAvailableExpansionVectors();
    }
}