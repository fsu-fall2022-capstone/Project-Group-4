using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetGenerator
{
    TileSet tileSet = new TileSet();
    int tileSetHeight, tileSetWidth;
    Tile currentTile;
    (bool x, bool y) reached = (false, false);

    int currIndex, nextIndex;

    public TileSetGenerator(int tilesHeight, int tilesWidth) {
        tileSetHeight = tilesHeight;
        tileSetWidth = tilesWidth;
        tileSet.height = tileSetHeight;
        tileSet.width = tileSetWidth;
        generateTileset();
    }

    public TileSetGenerator(int tilesHeight, int tilesWidth, Tile previousTileSetStart) {
        tileSetHeight = tilesHeight;
        tileSetWidth = tilesWidth;
        tileSet.height = tileSetHeight;
        tileSet.width = tileSetWidth;
        tileSet.endTile = previousTileSetStart;
        generateTileset();
    }


    public TileSet getTileSet() { return tileSet; }
    
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
    private List<Tile> getLeftEdgeTiles()
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
    private List<Tile> getRightEdgeTiles()
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

        tileSet.DirCardinals.start = UnityEngine.Random.Range(0,4); // get the cardinals
        // 0 for bottom, 1 for right, 2 for top, 3 for left

        Debug.Log($"{tileSet.DirCardinals.start}");
        
        switch (tileSet.DirCardinals.start)
        {
            case 0:
                startEdgeTiles = getBottomEdgeTiles();
                break;
            case 1:
                startEdgeTiles = getRightEdgeTiles();
                break;
            case 2:
                startEdgeTiles = getTopEdgeTiles();
                break;
            case 3:
                startEdgeTiles = getLeftEdgeTiles();
                break;                    
        }

        tileSet.DirCardinals.end = UnityEngine.Random.Range(0,4);
        if(tileSet.DirCardinals.end == tileSet.DirCardinals.start){
            while(tileSet.DirCardinals.end == tileSet.DirCardinals.start) {
                tileSet.DirCardinals.end = UnityEngine.Random.Range(0,4);
            }
        }

        Debug.Log($"{tileSet.DirCardinals.end}");

        switch (tileSet.DirCardinals.end) {
            case 0:
                endEdgeTiles = getBottomEdgeTiles();
                break;
            case 1:
                endEdgeTiles = getRightEdgeTiles();
                break;
            case 2:
                endEdgeTiles = getTopEdgeTiles();
                break;
            case 3:
                endEdgeTiles = getLeftEdgeTiles();
                break;            
        }

        int rand1 = UnityEngine.Random.Range(1, startEdgeTiles.Count);
        int rand2 = UnityEngine.Random.Range(1, endEdgeTiles.Count);
    
        int startTileIndex = tileSet.tiles.IndexOf(startEdgeTiles[rand1]);
        int endTileIndex = tileSet.tiles.IndexOf(endEdgeTiles[rand2]);

        tileSet.tiles[startTileIndex].type = 1;
        tileSet.tiles[endTileIndex].type = 1;

        tileSet.startTile = tileSet.tiles[startTileIndex];
        tileSet.endTile = tileSet.tiles[endTileIndex];

        Debug.Log($"StartTile Pos: {tileSet.startTile.position}");
        Debug.Log($"EndTile Pos: {tileSet.endTile.position}");
    }

    private void generateStart() 
    {
        // 0 for bottom, 1 for right, 2 for top, 3 for left
        if(tileSet.endTile.position.y == 0) {
            tileSet.DirCardinals.end = 0;
        } else if(tileSet.endTile.position.y == (tileSetHeight-1)) {
            tileSet.DirCardinals.end = 2;
        } else if(tileSet.endTile.position.x == 0) {
            tileSet.DirCardinals.end = 3;
        } else if(tileSet.endTile.position.x == (tileSetWidth-1)) {
            tileSet.DirCardinals.end = 1;
        }

        Debug.Log($"{tileSet.DirCardinals.end}");

        // edge tiles for tile selection randomness
        List<Tile> startEdgeTiles = new List<Tile>();

        tileSet.DirCardinals.start = UnityEngine.Random.Range(0,4); // get the cardinals

        if(tileSet.DirCardinals.end == tileSet.DirCardinals.start){
            while(tileSet.DirCardinals.end == tileSet.DirCardinals.start) {
                tileSet.DirCardinals.start = UnityEngine.Random.Range(0,4);
            }
        }

        Debug.Log($"{tileSet.DirCardinals.start}");
        
        switch (tileSet.DirCardinals.start)
        {
            case 0:
                startEdgeTiles = getBottomEdgeTiles();
                break;
            case 1:
                startEdgeTiles = getRightEdgeTiles();
                break;
            case 2:
                startEdgeTiles = getTopEdgeTiles();
                break;
            case 3:
                startEdgeTiles = getLeftEdgeTiles();
                break;                 
        }

        int rand = UnityEngine.Random.Range(1, startEdgeTiles.Count);

        int startTileIndex = tileSet.tiles.IndexOf(startEdgeTiles[rand]);

        tileSet.tiles[startTileIndex].type = 1;

        tileSet.startTile = tileSet.tiles[startTileIndex];

        Debug.Log($"StartTile Pos: {tileSet.startTile.position}");
        Debug.Log($"EndTile Pos: {tileSet.endTile.position}");
    }

    private void moveDown()
    {
        Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        tileSet.tiles[currIndex].type = 1;
        nextIndex = currIndex - tileSetWidth;
        currentTile = tileSet.tiles[nextIndex];
        Debug.Log("Moved Down.");
    }

    private void moveUp()
    {
        Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        tileSet.tiles[currIndex].type = 1;
        nextIndex = currIndex + tileSetWidth;
        currentTile = tileSet.tiles[nextIndex];
        Debug.Log("Moved Up.");
    }

    private void moveRight()
    {
        Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        tileSet.tiles[currIndex].type = 1;
        nextIndex = ++currIndex;
        currentTile = tileSet.tiles[nextIndex];
        Debug.Log("Moved Right.");
    }

    private void moveLeft() 
    {
        Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        tileSet.tiles[currIndex].type = 1;
        nextIndex = --currIndex;
        currentTile = tileSet.tiles[nextIndex];
        Debug.Log("Moved Left.");
    }

    private void generatePath() 
    {
        int counter = 0;
        currentTile = tileSet.startTile;

        Debug.Log("Generating Path");
        reached = (false, false);
        bool moving = true;
        while(moving) {
            counter++;
            Debug.Log($"CurrentTile: {currentTile.position}");
            if(counter == 1)
                switch(tileSet.DirCardinals.start) {
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
    }

    private void generateTileset() {
        for(int y = 0; y < tileSetHeight; y++) {
            for(int x = 0; x < tileSetWidth; x++) {
                Tile newTile = new Tile();
                newTile.position.x = x;
                newTile.position.y = y;
                newTile.type = 0;
                tileSet.tiles.Add(newTile);
            }
        }
        if(tileSet.startTile == null)
            generateStartEnd();
        else
            generateStart();
        generatePath();
    }

    public override string ToString() {
        string output = "\n\n";
        for(int i = tileSet.tiles.Count-1; i >= 0; i--) {
            output += tileSet.tiles[i].type.ToString();
            if(i % (tileSetWidth-1) == 0) {
                output+="\n";
            }
        }

        return output;
    }
}
