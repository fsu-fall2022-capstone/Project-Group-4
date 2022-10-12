using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetGenerator : MonoBehaviour
{
    struct Tile {
        (int x, int y) position; // position info
        int type;
    }

    struct TileSet {
        List<Tile> tiles = new List<Tile>();
        List<Tile> pathTiles = new List<Tile>();
        Tile startTile, endTile;
        (int start, int end) DirCardinals;
        int height, width;
    } 

    TileSet tileSet = new TileSet();
    int tileSetHeight, tilesetWidth;
    Tile currentTile;

    int currIndex, nextIndex;

    public TileSetGenerator(int tilesHeight, int tilesWidth) {
        tileSetHeight = tilesHeight;
        tilesetWidth = tilesWidth;
        tileSet.height = tileSetHeight;
        tileSet.width = tilesetWidth;
    }

    public tileSet getTileSet() { return tileSet; }
    
    private List<Tile> getTopEdgeTiles()
    {
        List<Tile> edgeTiles = new List<Tile>();

        for (int i = tileSetWidth * (tileSetHeight-1);
        i < tileSetWidth * tileSetHeight; i++){
            edgeTiles.Add(tileSet.tiles[i]);
        }

        return edgeTiles;
    }

    //This function was modified to allow any sized map based on tileSetWidth variable
    private List<Tile> getRightEdgeTiles()
    {
        List<Tile> edgeTiles = new List<Tile>();

        for (int i = 0; i < tileSetWidth * tileSetHeight; i++)
        {
            if ((i % tileSetWidth) == 0)            //Changed to allow any size map
                edgeTiles.Add(tileSet.tiles[i]);
        }

        return edgeTiles;
    }

    //This function was modified to allow any sized map based on tileSetWidth variable
    private List<Tile> getLeftEdgeTiles()
    {
        List<Tile> edgeTiles = new List<Tile>();

        for (int i = tileSetWidth-1; i < tileSetWidth * tileSetHeight; i++)
        {
            edgeTiles.Add(tileSet.tiles[i]);
            i += (tileSetWidth - 1);            //Changed to allow any size map
        }

        return edgeTiles;
    }


    private List<Tile> getBottomEdgeTiles()
    {
        List<Tile> edgeTiles = new List<Tile>();

        for (int i = 0; i < tileSetWidth; i++) {
            edgeTiles.Add(tileSet.tiles[i]);
        }

        return edgeTiles;
    }

    private Tile getMiddleTile()
    {
        return tileSet.tiles[((int)(tileSetHeight / 2))*
                    (((int)(tileSetWidth / 2)) - 1) + ((int)(tileSetWidth / 2))];
    }

    private void generateStartEnd()
    {
        // edge tiles for tile selection randomness
        List<Tile> startEdgeTiles = new List<Tile>();
        List<Tile> endEdgeTiles = new List<Tile>();

        DirCardinals.start = UnityEngine.Random.Range(0,4); // get the cardinals
        // 0 for bottom, 1 for left, 2 for top, 3 for right
        
        switch (DirCardinals.start)
        {
            case 0:
                startEdgeTiles = getBottomEdgeTiles();
                break;
            case 1:
                startEdgeTiles = getLeftEdgeTiles();
                break;
            case 2:
                startEdgeTiles = getTopEdgeTiles();
                break;
            case 3:
                startEdgeTiles = getRightEdgeTiles();
                break;            
        }

        Debug.Log($"{tileSet.DirCardinal.start}");

        DirCardinals.end = UnityEngine.Random.Range(0,4);
        if(DirCardinals.end == DirCardinals.start){
            while(DirCardinals.end == DirCardinals.start) {
                DirCardinals.end = UnityEngine.Random.Range(0,4);
            }
        }

        switch (DirCardinals.end) {
            case 0:
                endEdgeTiles = getBottomEdgeTiles();
                break;
            case 1:
                endEdgeTiles = getLeftEdgeTiles();
                break;
            case 2:
                endEdgeTiles = getTopEdgeTiles();
                break;
            case 3:
                endEdgeTiles = getRightEdgeTiles();
                break;    
        }

        int rand1 = UnityEngine.Random.Range(0, startEdgeTiles.Count);
        int rand2 = UnityEngine.Random.Range(0, endEdgeTiles.Count);
    
        tileSet.startTile = startEdgeTiles[rand1];
        tileSet.endTile = endEdgeTiles[rand2];
    }



    private void moveDown()
    {
        tileSet.pathTiles.Add(currentTile);
        currIndex = tileSet.tiles.IndexOf(currentTile);
        nextIndex = currIndex - mapSize.width;
        currentTile = tileSet.tiles[nextIndex];
    }

    private void moveUp()
    {
        tileSet.pathTiles.Add(currentTile);
        currIndex = tileSet.tiles.IndexOf(currentTile);
        nextIndex = currIndex + mapSize.width;
        currentTile = tileSet.tiles[nextIndex];
    }

    private void moveRight()
    {
        tileSet.pathTiles.Add(currentTile);
        currIndex = tileSet.tiles.IndexOf(currentTile);
        nextIndex = ++currIndex;
        currentTile = tileSet.tiles[nextIndex];
    }

    private void moveLeft() 
    {
        tileSet.pathTiles.Add(currentTile);
        currIndex = tileSet.tiles.IndexOf(currentTile);
        nextIndex = --currIndex;
        currentTile = tileSet.tiles[nextIndex];
    }

    private void generatePath() 
    {
        int counter = 0;
        currentTile = startTile;

        Debug.Log("Generating Path");
        reached = (false, false);
        bool moving = true;
        while(moving) {
            counter++;
            Debug.Log($"CurrentTile: {currentTile.position}");
            if(counter == 1)
                switch(DirStart) {
                    // 0 for bottom, 1 for left, 2 for top, 3 for right
                    case 0:
                        moveUp();
                        break;
                    case 1:
                        moveLeft();
                        break;
                    case 2:
                        moveDown();
                        break;
                    case 3:
                        moveRight();
                        break;
                }
            
            if (counter > 100)
            {
                Debug.Log("Loop ran too long, broke out of it.");
                break;
            }
            else if(!reached.y) {
                if (currentTile.position.y > tileSet.endTile.position.y)
                {
                    moveDown();
                }
                else if (currentTile.position.y < tileSet.endTile.position.y)
                {
                    moveUp();    
                }
                else
                {
                    reached.y = true;
                }
            } else if(!reached.x) {
                if(currentTile.position.x > tileSet.endTile.position.x) 
                {
                    moveLeft();
                } 
                else if (currentTile.position.x < tileSet.endTile.position.x) 
                {
                    moveRight();
                }
                else
                {
                    reached.x = true;
                }
            } else if(reached.x && reached.y) {
                moving = false;
            }
    }

    public generateTileset() {
        for(int y = 0; y < tileSetHeight; y++) {
            for(int x = 0; x < tileSetWidth; x++) {
                Tile newTile = new Tile();
                newTile.position.x = x;
                newTile.position.y = y;
                newTile.type = 0;
                tileSet.Add(newTile);
            }
        }

        generateStartEnd();
        generatePath();
    }

    public void printTileSet() {
        Debug.Log($"{tileSet.tiles}");
    }
}
