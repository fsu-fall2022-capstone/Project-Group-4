/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names have been changed and some parts of the code has been 
    modified to fit our game scheme, these sections will be marked with comments. 
*/
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject mapTile;

    [SerializeField] private int tilesetWidth;
    [SerializeField] private int tilesetHeight;
    private (int width, int height) mapSize;

    [SerializeField] private Color pathColor;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();

    public static GameObject startTile { get; private set; } // starting position of enemy
    public static GameObject endTile { get; private set; } // home position of player

    private (bool x, bool y) reached = (false, false);
    private GameObject currentTile;
    private int currIndex;
    private int nextIndex;

    private void Start()
    {
        mapSize.width = tilesetWidth;
        mapSize.height = tilesetHeight;
        generateMap();
    }

        //This function was modified to allow any sized map based on mapSize.width variable
    private List<GameObject> getFrontEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();

        for (int i = 0; i < mapSize.width * mapSize.height; i++)
        {
            if ((i % mapSize.width) == 0)            //Changed to allow any size map
                edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    //This function was modified to allow any sized map based on mapSize.width variable
    private List<GameObject> getBackEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();

        for (int i = mapSize.width-1; i < mapSize.width * mapSize.height; i++)
        {
            edgeTiles.Add(mapTiles[i]);
            i += (mapSize.width - 1);            //Changed to allow any size map
        }

        return edgeTiles;
    }


    //MoveDown, moveUp, moveRight all modified to select path tiles from left to right
    private void moveDown()
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex - mapSize.width;
        currentTile = mapTiles[nextIndex];
    }

    private void moveUp()
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex + mapSize.width;
        currentTile = mapTiles[nextIndex];
    }

    private void moveRight()
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex + 1;
        currentTile = mapTiles[nextIndex];
    }

    private void moveLeft() 
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex--;
        currentTile = mapTiles[nextIndex];
    }

    private void generatePath() {
        int counter = 0;

        while (!reached.y)
        {
            counter++;
            Debug.Log($"Current Tile: {currentTile.transform.position}");
            if (counter == 1)
                moveRight();

            if (counter > 100)
            {
                Debug.Log("Loop ran too long, broke out of it.");
                break;
            }
            if (currentTile.transform.position.y > endTile.transform.position.y)
            {
                moveDown();
                Debug.Log("Moved Down.\n");
            }
            else if (currentTile.transform.position.y < endTile.transform.position.y)
            {
                moveUp();
                Debug.Log("Moved Up.\n");
            }
            else
            {
                reached.y = true;
            }
        }

        while (!reached.x)
        {
            moveRight();
            if (currentTile.transform.position.x == endTile.transform.position.x)
            {
                reached.x = true;
                break;
            }
        }
    }

    public void expandMap() {
        reached = (false, false);
        currentTile = endTile;
        pathTiles.Remove(endTile);

        // gen new tileset based on end tile
        int newWidth = mapSize.width + tilesetWidth;
        for (int y = 0; y < mapSize.height; y++) 
        {
            for (int x = mapSize.width; x < newWidth; x++) 
            {
                GameObject newTile = Instantiate(mapTile);
                newTile.transform.position = new Vector2(x,y);

                mapTiles.Add(newTile);
            }
        }
        mapTiles = mapTiles.OrderBy(t => t.transform.position.y)
                    .ThenBy(t => t.transform.position.x).ToList();
        

        mapSize.width = newWidth;

        // get new front
        List<GameObject> backEdgeTiles = getBackEdgeTiles();

        int rand = UnityEngine.Random.Range(0, mapSize.height);

        endTile = backEdgeTiles[rand];

        Debug.Log($"End Tile: {endTile.transform.position}");

        generatePath();
        pathTiles.Add(endTile);

        foreach(GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = pathColor;
        }

        startTile.GetComponent<SpriteRenderer>().color = startColor;
        endTile.GetComponent<SpriteRenderer>().color = endColor;

    }

    private void generateMap()
    {
        for (int y = 0; y < mapSize.height; y++)
        {
            for (int x = 0; x < mapSize.width; x++)
            {
                GameObject newTile = Instantiate(mapTile);
                newTile.transform.position = new Vector2(x,y);

                mapTiles.Add(newTile);
            }
        }

        List<GameObject> frontEdgeTiles = getFrontEdgeTiles();
        List<GameObject> backEdgeTiles = getBackEdgeTiles();

        int rand1 = UnityEngine.Random.Range(0, mapSize.height);
        int rand2 = UnityEngine.Random.Range(0, mapSize.height);

        startTile = frontEdgeTiles[rand1];
        endTile = backEdgeTiles[rand2];

        Debug.Log($"Start Tile: {startTile.transform.position}");
        Debug.Log($"End Tile: {endTile.transform.position}");

        currentTile = startTile;

        generatePath();
        pathTiles.Add(endTile);

        foreach(GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = pathColor;
        }

        startTile.GetComponent<SpriteRenderer>().color = startColor;
        endTile.GetComponent<SpriteRenderer>().color = endColor;
    }
}