using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// this function is more like a sorting order renderer
// most layers are dependent on the tile location

public class MapRenderer : MonoBehaviour
{
    public static MapRenderer main;
    private int previousMapCount = 0;
    public static bool activeRenderer = true;

    [SerializeField] private Sprite[] tileSprites;
    private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

    protected void Start()
    {
        if (main == null) main = this;
        LoadDictionary();
    }

    protected void Update()
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

    private void LoadDictionary()
    {
        Sprites = new Dictionary<string, Sprite>();

        for (int i = 0; i < tileSprites.Length; i++)
        {
            Debug.Log("MapRenderer: Loaded sprite " + tileSprites[i].name);
            Sprites.Add(tileSprites[i].name, tileSprites[i]);
        }
    }

    public int GetSpriteCount(string name_pattern)
    {
        // count the number of sprites that match the name pattern
        int count = 0;
        foreach (KeyValuePair<string, Sprite> sprite in Sprites)
        {
            if (sprite.Key.Contains(name_pattern))
            {
                count++;
            }
        }
        return count;
    }

    public Sprite GetSpriteByName(string name)
    {
        if (Sprites.ContainsKey(name))
            return Sprites[name];
        else
            return null;
    }

    public void UpdateSortingOrder()
    { // should only be utilized by the inspector
        updateSortingLayerValue();
    }

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
                //Debug.Log("MapRenderer: Comparing " + mapTile.transform.position + " to " + previousMapTile.transform.position);
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
            pathTile.GetComponent<SpriteRenderer>().sprite = MapRenderer.main.GetSpriteByName("path_fourway");
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

        pathTile.GetComponent<SpriteRenderer>().sprite = MapRenderer.main.GetSpriteByName(spriteName);
        return;
    }

    public void GenerateRandomMapSprite(GameObject newTile)
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random >= 80 && random < 95)
        { // tree
            random = UnityEngine.Random.Range(1, GetSpriteCount("tree_cluster"));
            Sprite newSprite = GetSpriteByName($"tree_cluster_{random}");
            newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if (random >= 95 && random < 98)
        { // rock
            random = UnityEngine.Random.Range(1, GetSpriteCount("rock_cluster"));
            Sprite newSprite = GetSpriteByName($"rock_cluster_{random}");
            newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if (random >= 98 && random < 100)
        {
            random = UnityEngine.Random.Range(1, GetSpriteCount("gem_cluster"));
            Sprite newSprite = GetSpriteByName($"gem_cluster_{random}");
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
            pathTile.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_NW_NE");
            //Debug.Log($"NW_NE {pathTile.name}");
        }
        else if ((previous.position.y > current.position.y && current.position.y == next.position.y && current.position.x < next.position.x) ||
        (previous.position.x > current.position.x && current.position.x == next.position.x && current.position.y < next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_NE_SE");
            //Debug.Log($"NE_SE {pathTile.name}");
        }
        else if ((previous.position.y < current.position.y && current.position.y == next.position.y && current.position.x < next.position.x) ||
        (previous.position.x > current.position.x && current.position.x == next.position.x && current.position.y > next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_SW_SE");
            //Debug.Log($"SW_SE {pathTile.name}");
        }
        else if ((previous.position.y < current.position.y && current.position.y == next.position.y && current.position.x > next.position.x) ||
        (previous.position.x < current.position.x && current.position.x == next.position.x && current.position.y > next.position.y))
        {
            pathTile.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_NW_SW");
            //Debug.Log($"NW_SW {pathTile.name}");
        }
        else if ((next.position.x > current.position.x || next.position.x < current.position.x) && next.position.y == current.position.y)
        {
            Sprite newSprite = GetSpriteByName($"path_NW_SE");
            pathTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if ((next.position.y > current.position.y || next.position.y < current.position.y) && next.position.x == current.position.x)
        {
            Sprite newSprite = GetSpriteByName($"path_SW_NE");
            pathTile.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    // this is when we stitch paths and aren't sure about the lists, but have the actual GameObjects
    public void StitchPathSprite(GameObject newPath, GameObject stitch, Vector3 next)
    {
        //Debug.Log($"newPath pos: {newPath.transform.position} stitch pos: {stitch.transform.position} next pos: {next}");
        if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y > next.y) ||
        (stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        next.x < newPath.transform.position.x && next.y < newPath.transform.position.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_SW_SE");
            //Debug.Log($"Stitch SW_SE {newPath.name}");
        }
        else if ((stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y > next.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y < next.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_NE_SE");
            //Debug.Log($"Stitch NE_SE {newPath.name}");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x < next.x && newPath.transform.position.y < next.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x > next.x && newPath.transform.position.y < next.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_NW_NE");
            //Debug.Log($"Stitch NW_NE {newPath.name}");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y &&
        newPath.transform.position.x > next.x && newPath.transform.position.y < next.y) ||
        (stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y &&
        newPath.transform.position.x > next.x && newPath.transform.position.y > next.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_corner_NW_SW");
            //Debug.Log($"Stitch NW_SW {newPath.name}");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_NW_SE");
            //Debug.Log($"Stitch NW_SE {newPath.name}");
        }
        else if ((stitch.transform.position.x < newPath.transform.position.x && stitch.transform.position.y < newPath.transform.position.y) ||
        (stitch.transform.position.x > newPath.transform.position.x && stitch.transform.position.y > newPath.transform.position.y))
        {
            newPath.GetComponent<SpriteRenderer>().sprite = GetSpriteByName("path_SW_NE");
            //Debug.Log($"Stitch SW_NE {newPath.name}");
        }
    }
}