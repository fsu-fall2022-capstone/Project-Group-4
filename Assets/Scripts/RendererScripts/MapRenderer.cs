using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapRenderer : SpriteLoader
{
    public static MapRenderer main;
    private int previousMapCount = 0;
    public static bool activeRenderer = true;

    protected void Start()
    {
        if (main == null) main = this;
        base.LoadDictionary();
    }

    protected void FixedUpdate()
    {
        if (previousMapCount != MapGenerator.mapTiles.Count && activeRenderer)
        {
            Debug.Log("MapRenderer: Map count changed from " + previousMapCount + " to " + MapGenerator.mapTiles.Count);
            previousMapCount = MapGenerator.mapTiles.Count;
            updateSortingLayerValue();
        }
    }

    public static void triggerRenderer()
    {
        activeRenderer = !activeRenderer;
    }

    public void UpdateSortingOrder()
    { // should only be utilized by the inspector
        updateSortingLayerValue();
    }

    // this function is more like a sorting order renderer
    // most layers are dependent on the tile location
    private void updateSortingLayerValue()
    {
        int layerCount = 32767;
        for (int i = 0; i < previousMapCount && layerCount > -32767; i++)
        {
            GameObject mapTile = MapGenerator.mapTiles[i];

            // if there was a previous tile, give it the same layer
            if (i > 0)
            {
                GameObject previousMapTile = MapGenerator.mapTiles[i - 1];
                if (mapTile.transform.position.y == previousMapTile.transform.position.y)
                {
                    mapTile.GetComponent<SpriteRenderer>().sortingOrder = previousMapTile.GetComponent<SpriteRenderer>().sortingOrder;
                }
                else
                    mapTile.GetComponent<SpriteRenderer>().sortingOrder = --layerCount;
            }
            else
                mapTile.GetComponent<SpriteRenderer>().sortingOrder = layerCount;
        }

        // get home tile and apply the same layer to the child gameobject
        GameObject homeTile = MapGenerator.endTile;
        homeTile.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = homeTile.GetComponent<SpriteRenderer>().sortingOrder;
    }

    public void UpdateSplitPathSprite(GameObject pathTile, Tile previous, Tile current)
    {
        string spriteName = pathTile.GetComponent<SpriteRenderer>().sprite.name;
        Debug.Log($"spriteName: {spriteName} previous pos: {previous.position} current pos: {current.position}");
        if (spriteName.Contains("fourway")) return;

        int dir_count = 0;
        bool NW = false, NE = false, SE = false, SW = false;

        if (spriteName.Contains("NE")) { NE = true; dir_count++; }
        if (spriteName.Contains("NW")) { NW = true; dir_count++; }
        if (spriteName.Contains("SE")) { SE = true; dir_count++; }
        if (spriteName.Contains("SW")) { SW = true; dir_count++; }

        if (dir_count == 3)
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_fourway");
            return;
        }

        spriteName = "path_tri";

        if (current.position.x > previous.position.x && current.position.y == previous.position.y)
            SE = true;
        else if (current.position.x < previous.position.x && current.position.y == previous.position.y)
            NW = true;
        else if (current.position.y > previous.position.y && current.position.x == previous.position.x)
            NE = true;
        else if (current.position.y < previous.position.y && current.position.x == previous.position.x)
            SW = true;


        if (NW) spriteName += "_NW";
        if (NE) spriteName += "_NE";
        if (SW) spriteName += "_SW";
        if (SE) spriteName += "_SE";

        Debug.Log($"new spriteName: {spriteName} pathTile Name: {pathTile.name}");

        pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName(spriteName);
        return;
    }

    public void GenerateRandomMapSprite(GameObject newTile)
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random >= 80 && random < 95)
        { // tree
            random = UnityEngine.Random.Range(1, GetSpriteCount("tree_cluster"));
            Sprite newSprite = base.GetSpriteByName($"tree_cluster_{random}");
            newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if (random >= 95 && random < 98)
        { // rock
            random = UnityEngine.Random.Range(1, GetSpriteCount("rock_cluster"));
            Sprite newSprite = base.GetSpriteByName($"rock_cluster_{random}");
            newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if (random >= 98 && random < 100)
        {
            random = UnityEngine.Random.Range(1, GetSpriteCount("gem_cluster"));
            Sprite newSprite = base.GetSpriteByName($"gem_cluster_{random}");
            newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    // this shit is pure disgusting and probably the definition of visual spaghetti code
    // but it works and i won't be touching it as often 

    // this function is for selecting the correct sprite for the path tile
    public void SelectPathSprite(GameObject pathTile, Tile previous, Tile current, Tile next)
    {
        if ((previous.position.y > current.position.y && current.position.y == next.position.y && current.position.x > next.position.x) ||
        (previous.position.x < current.position.x && current.position.x == next.position.x && current.position.y < next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_NW_NE");
        }
        else if ((previous.position.y > current.position.y && current.position.y == next.position.y && current.position.x < next.position.x) ||
        (previous.position.x > current.position.x && current.position.x == next.position.x && current.position.y < next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_NE_SE");
        }
        else if ((previous.position.y < current.position.y && current.position.y == next.position.y && current.position.x < next.position.x) ||
        (previous.position.x > current.position.x && current.position.x == next.position.x && current.position.y > next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_SW_SE");
        }
        else if ((previous.position.y < current.position.y && current.position.y == next.position.y && current.position.x > next.position.x) ||
        (previous.position.x < current.position.x && current.position.x == next.position.x && current.position.y > next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_NW_SW");
        }
        else if ((next.position.x > current.position.x || next.position.x < current.position.x) && next.position.y == current.position.y)
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName($"path_NW_SE");
        }
        else if ((next.position.y > current.position.y || next.position.y < current.position.y) && next.position.x == current.position.x)
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName($"path_SW_NE");
        }
    }

    // this is when we stitch paths and aren't sure about the lists, but have the actual GameObjects
    public void StitchPathSprite(GameObject newPath, GameObject stitch, Vector3 next)
    {
        if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y > next.y) ||
        (stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        next.x < newPath.transform.position.x && next.y < newPath.transform.position.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_SW_SE");
        }
        else if ((stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y > next.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y < next.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_NE_SE");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y < next.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x > next.x && newPath.transform.position.y < next.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_NW_NE");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y &&
        newPath.transform.position.x > next.x && newPath.transform.position.y < next.y) ||
        (stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x > next.x && newPath.transform.position.y > next.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_corner_NW_SW");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_NW_SE");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = base.GetSpriteByName("path_SW_NE");
        }
    }
}