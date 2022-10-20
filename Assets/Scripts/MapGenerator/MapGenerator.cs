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

    [SerializeField] private bool generateAsIsometric = true;
    [SerializeField] private float spriteSize = 1f;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();
    public static List<TileSet> tileSets = new List<TileSet>();
    public static List<MapLayout> mapLayout = new List<MapLayout>();
    //public static List<GameObject> resourceTiles = new List<GameObject>();

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

    public bool checkExpandability() 
    {
        // this'll be used to check if the map can be expanded based on the location
        // data of the recent tileset stored in mapLayout and 
        // the most recent direction data TileSet stored in tileSets.
        // this'll be used for any UI buttons that expand the map before calling it
        (int x, int y) lastTilePos = mapLayout[mapLayout.Count - 1].position;
        (int start, int end) lastDir = tileSets[tileSets.Count - 1].DirCardinals;
        // 0 for bottom, 1 for right, 2 for top, 3 for left

        (int x, int y) checkPos = (0,0);
        switch(lastDir.start)
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

    private List<int> checkAvailableExpansionDirections() {
        // now at the valid point of expansion, time to check which start directions are available
        (int x, int y) lastTilePos = mapLayout[mapLayout.Count - 1].position;
        (int start, int end) lastDir = tileSets[tileSets.Count - 1].DirCardinals;

        List<(int x, int y)> availableVectors = new List<(int x, int y)>();
        List<int> availableDirections = new List<int>();
        List<(int x, int y)> checkVectors = new List<(int x, int y)>();
        (int x, int y) validPos = (0,0);
        (int x, int y) checkPos = (0,0);
        (int x, int y) findPosDir = (0,0);

        switch(lastDir.start)
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

    private void drawTileSet(TileSet newTileSet, (int x, int y) displacement = (0,0), bool attachStitch = false) {
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
                GameObject newTile = Instantiate(mapTile1, tilePos, Quaternion.identity);
                mapTiles.Add(newTile);
            }
        }

        if(attachStitch) {
            // old end tile info
            Vector3 oldPos = startTile.transform.position;
            pathTiles.Remove(startTile);
            Destroy(startTile);

            // converting the end tile sprite to a path tile sprite
            GameObject replacedTile = Instantiate(pathTile, oldPos, Quaternion.identity);
            pathTiles.Add(replacedTile);
        }

        for(int i = newTileSet.pathTiles.Count - 1; i >= 0; i--) {
            Tile currTile = newTileSet.pathTiles[i];
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
                    if (!attachStitch) {
                        GameObject newEndTile = Instantiate(homeTile, tilePos, Quaternion.identity);
                        pathTiles.Add(newEndTile);
                        endTile = newEndTile;
                    } else {
                        GameObject newPathTile = Instantiate(pathTile, tilePos, Quaternion.identity);
                        pathTiles.Add(newPathTile);
                    }
                    break;
            }
        }
    }

    public void expandMap()
    {   // expands the map
        if(!checkExpandability())
        {
            Debug.Log("Map cannot be expanded.");
            return;
        }

        Debug.Log("Map expansion!");

        List<int> availableDirections = checkAvailableExpansionDirections();
        (int start, int end) lastDir = tileSets[tileSets.Count - 1].DirCardinals;

        MapLayout locTileInfo = new MapLayout();
        Tile genStitchTile = new Tile(tileSets[tileSets.Count-1].startTile.position, 3);
        TileSetGenerator tileSetGen;
        if(availableDirections.Count == 0 || availableDirections.Count == 3) { 
            // direction won't matter, just pick a random one
            Debug.Log("Any direction is available.");
            tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, genStitchTile);
        } else {
            int rand = UnityEngine.Random.Range(0, availableDirections.Count);
            Debug.Log($"Random number: {rand}");
            int randDir = availableDirections[rand];
            Debug.Log($"Random direction: {randDir}");
            tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight, genStitchTile, randDir);
        }

        Debug.Log($"{tileSetGen.ToString()}");
        TileSet newTileSet = tileSetGen.getTileSet();
        tileSets.Add(newTileSet);
        
        if(newTileSet.DirCardinals.start == lastDir.start) {
            straightLineCounter++;
        } else {
            straightLineCounter = 0;
        }
        
        (int x, int y) lastPos = mapLayout[mapLayout.Count - 1].position;

        switch(lastDir.start)
        {
            case 0: // bottom
                locTileInfo.position = (lastPos.x, lastPos.y - 1);
                break;
            case 1: // right
                locTileInfo.position = (lastPos.x + 1, lastPos.y);
                break;
            case 2: // top
                locTileInfo.position = (lastPos.x, lastPos.y + 1);
                break;
            case 3: // left
                locTileInfo.position = (lastPos.x - 1, lastPos.y);
                break;
        }

        mapLayout.Add(locTileInfo);

        Debug.Log($"Location tile info: {locTileInfo.position}");

        drawTileSet(newTileSet, (locTileInfo.position.x * tilesetWidth, locTileInfo.position.y * tilesetHeight), true);
    }

    private void generateMap()
    {   // generates the initial map
        MapLayout locTileInfo = new MapLayout();
        TileSetGenerator tileSetGen = new TileSetGenerator(tilesetWidth, tilesetHeight);

        Debug.Log($"{tileSetGen.ToString()}");
        tileSets.Add(tileSetGen.getTileSet());
        
        locTileInfo.position = (0, 0);
        mapLayout.Add(locTileInfo);
        (float x, float y) pos;

        drawTileSet(tileSets[0], (0, 0));
    }
}