/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names have been changed and some parts of the code has been 
    modified to fit our game scheme, these sections will be marked with comments. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject mapTile;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();

    public static GameObject startTile;
    public static GameObject endTile;

    private bool reachedY = false;
    private bool reachedX = false;
    private GameObject currentTile;
    private int currIndex;
    private int nextIndex;

    public Color pathColor;
    public Color startColor;
    public Color endColor;

    private void Start()
    {
        generateMap();
    }

    //This function was modified to allow any sized map based on mapWidth variable
    private List<GameObject> getFrontEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();

        for (int i = 0; i < mapWidth * mapHeight; i++)
        {
            if ((i % mapWidth) == 0)            //Changed to allow any size square map
                edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    //This function was modified to allow any sized map based on mapWidth variable
    private List<GameObject> getBackEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();

        for (int i = mapWidth-1; i < mapWidth * mapHeight; i++)
        {
            edgeTiles.Add(mapTiles[i]);
            i += (mapWidth - 1);            //Changed to allow any size square map
        }

        return edgeTiles;
    }

    //MoveDown, moveUp, moveRight all modified to select path tiles from left to right
    private void moveDown()
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex - mapWidth;
        currentTile = mapTiles[nextIndex];
    }

    private void moveUp()
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex + mapWidth;
        currentTile = mapTiles[nextIndex];
    }

    private void moveRight()
    {
        pathTiles.Add(currentTile);
        currIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currIndex + 1;
        currentTile = mapTiles[nextIndex];
    }

    private void generateMap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                GameObject newTile = Instantiate(mapTile);

                mapTiles.Add(newTile);

                newTile.transform.position = new Vector2(x,y);
            }
        }

        List<GameObject> frontEdgeTiles = getFrontEdgeTiles();
        List<GameObject> backEdgeTiles = getBackEdgeTiles();

        int rand1 = UnityEngine.Random.Range(0, mapHeight);
        int rand2 = UnityEngine.Random.Range(0, mapHeight);

        startTile = frontEdgeTiles[rand1];
        endTile = backEdgeTiles[rand2];

        currentTile = startTile;
        int counter = 0;

        while (!reachedY)
        {
            counter++;
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
                reachedY = true;
            }
        }

        while (!reachedX)
        {
            moveRight();
            if (currentTile.transform.position.x == endTile.transform.position.x)
            {
                reachedX = true;
                break;
            }
        }

        pathTiles.Add(endTile);

        foreach(GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = pathColor;
        }

        startTile.GetComponent<SpriteRenderer>().color = startColor;
        endTile.GetComponent<SpriteRenderer>().color = endColor;
    }
}
