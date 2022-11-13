using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetGenerator
{
    private TileSet tileSet = new TileSet();
    private int tileSetHeight, tileSetWidth;
    private Tile currentTile;
    private (bool x, bool y) reached = (false, false);
    private (int start, int end) DirCardinals = (-1,-1); // 0 for bottom, 1 for right, 2 for top, 3 for left
    private int numStartPoints;

    private int currIndex, nextIndex;

    public TileSetGenerator(int tilesHeight, int tilesWidth, int numStartPoints = 1) {
        tileSetHeight = tilesHeight;
        tileSetWidth = tilesWidth;
        tileSet.height = tileSetHeight;
        tileSet.width = tileSetWidth;
        initializeFromNumStartPoints(numStartPoints);
        generateTileset();
    }

    public TileSetGenerator(int tilesHeight, int tilesWidth, Tile previousTileSetStart, int numStartPoints = 1) {
        tileSetHeight = tilesHeight;
        tileSetWidth = tilesWidth;
        tileSet.height = tileSetHeight;
        tileSet.width = tileSetWidth;
        tileSet.endTile = previousTileSetStart;
        initializeFromNumStartPoints(numStartPoints);
        generateTileset();
    }

    public TileSetGenerator(int tilesHeight, int tilesWidth, Tile previousTileSetStart, int givenStartCardinal, int numStartPoints = 1) {
        tileSetHeight = tilesHeight;
        tileSetWidth = tilesWidth;
        tileSet.height = tileSetHeight;
        tileSet.width = tileSetWidth;
        tileSet.endTile = previousTileSetStart;
        DirCardinals.start = givenStartCardinal;
        this.numStartPoints = numStartPoints;
        initializeFromNumStartPoints(numStartPoints);
        generateTileset();
    }

    private void initializeFromNumStartPoints(int numStartPoints) {
        this.numStartPoints = (numStartPoints > 3) ? 3 : numStartPoints;
        for (int i = 0; i < numStartPoints; i++) {
            tileSet.pathTiles.Add(new List<Tile>());
        }
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

    private void coreStartGen() {
        List<Tile> startEdgeTiles = new List<Tile>();

        switch (DirCardinals.start)
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

        int rand, startTileIndex, endTileIndex, xDiff, yDiff;
        endTileIndex = tileSet.tiles.IndexOf(tileSet.endTile);

        do {
            rand = UnityEngine.Random.Range(1, startEdgeTiles.Count - 1);

            startTileIndex = tileSet.tiles.IndexOf(startEdgeTiles[rand]);

            xDiff = Mathf.Abs(tileSet.tiles[startTileIndex].position.x - tileSet.tiles[endTileIndex].position.x);
            yDiff = Mathf.Abs(tileSet.tiles[startTileIndex].position.y - tileSet.tiles[endTileIndex].position.y);
        } while ((xDiff + yDiff) < 7); //Make sure enemy path is at least 7 tiles long

        tileSet.tiles[startTileIndex].type = 2;

        tileSet.spawnTiles.Add(tileSet.tiles[startTileIndex]);

        //Debug.Log($"StartTile Pos: {tileSet.spawnTiles[tileSet.spawnTiles.Count - 1].position}");
        //Debug.Log($"EndTile Pos: {tileSet.endTile.position}");

        // add the cardinals to the list
        tileSet.DirCardinals.Add(DirCardinals);
    }

    private void adjustImportedEnd() 
    {
        // normal directions are this
        // 0 for bottom, 1 for right, 2 for top, 3 for left
        // but since we're stitching from the end tile, we need to reverse the directions
        // and adjust the position of the end tile to match the new edge
        if(tileSet.endTile.position.y == 0) {
            DirCardinals.end = 2;
            tileSet.endTile.position.y = tileSetHeight - 1;
        } else if(tileSet.endTile.position.y == (tileSetHeight-1)) {
            DirCardinals.end = 0;
            tileSet.endTile.position.y = 0;
        } else if(tileSet.endTile.position.x == 0) {
            DirCardinals.end = 1;
            tileSet.endTile.position.x = tileSetWidth - 1;
        } else if(tileSet.endTile.position.x == (tileSetWidth-1)) {
            DirCardinals.end = 3;
            tileSet.endTile.position.x = 0;
        }

        //Debug.Log($"{DirCardinals.end}");
        int index = tileSet.tiles.FindIndex(tile => tile.position == tileSet.endTile.position);
        tileSet.tiles[index].type = 3;
        tileSet.endTile = tileSet.tiles[index];
    }

    private void generateAdditionalStarts() {
        // generate additional start points
        int errorCounter = 0;
        for (int i = 1; i < numStartPoints && errorCounter < 100; i++, errorCounter++) {
            // check to see which start cardinals aren't used
            List<int> startCardinals = new List<int>();
            for (int j = 0; j < 4; j++) {
                bool flag = true;
                for(int x = 0; x < tileSet.DirCardinals.Count; x++) {
                    if(j == tileSet.DirCardinals[x].start || j == tileSet.DirCardinals[x].end) {
                        flag = false;
                        break;
                    }
                }
                if(flag)
                    startCardinals.Add(j);
            }

            // randomly select from the available start cardinals
            int newStart = startCardinals[UnityEngine.Random.Range(0, startCardinals.Count)];

            DirCardinals.start = newStart;

            //Debug.Log($"{DirCardinals.start}");

            coreStartGen();
        }
    }
    private void generateStartEnd() // usually for the initial start/end construction of the map
    {
        // edge tiles for tile selection randomness
        List<Tile> startEdgeTiles = new List<Tile>();
        List<Tile> endEdgeTiles = new List<Tile>();

        DirCardinals.start = UnityEngine.Random.Range(0,4); // get the cardinals
        // 0 for bottom, 1 for right, 2 for top, 3 for left

        //Debug.Log($"{DirCardinals.start}");
        
        switch (DirCardinals.start)
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

        DirCardinals.end = UnityEngine.Random.Range(0,4);
        while(DirCardinals.end == DirCardinals.start) {
            DirCardinals.end = UnityEngine.Random.Range(0,4);
        }

        //Debug.Log($"{DirCardinals.end}");

        switch (DirCardinals.end) {
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

        //randomize and exclude corner pieces
        int rand1, rand2, startTileIndex, endTileIndex, xDiff, yDiff;
        do
        {
            rand1 = UnityEngine.Random.Range(1, startEdgeTiles.Count - 1);
            rand2 = UnityEngine.Random.Range(1, endEdgeTiles.Count - 1);

            startTileIndex = tileSet.tiles.IndexOf(startEdgeTiles[rand1]);
            endTileIndex = tileSet.tiles.IndexOf(endEdgeTiles[rand2]);

            xDiff = Mathf.Abs(tileSet.tiles[startTileIndex].position.x - tileSet.tiles[endTileIndex].position.x);
            yDiff = Mathf.Abs(tileSet.tiles[startTileIndex].position.y - tileSet.tiles[endTileIndex].position.y);
        } while ((xDiff + yDiff) < 7); //Make sure enemy path is at least 7 tiles long

        tileSet.tiles[startTileIndex].type = 2;
        tileSet.tiles[endTileIndex].type = 3;

        tileSet.spawnTiles.Add(tileSet.tiles[startTileIndex]); // start tile
        tileSet.endTile = tileSet.tiles[endTileIndex];

        //Debug.Log($"StartTile Pos: {tileSet.spawnTiles[0].position}");
        //Debug.Log($"EndTile Pos: {tileSet.endTile.position}");

        // add the cardinals to the list
        tileSet.DirCardinals.Add(DirCardinals);
        if(numStartPoints > 1)
            generateAdditionalStarts();
    }

    private void generateStart() // imported end tile, generate start
    {
        adjustImportedEnd(); // fix the end tile to match the new edge

        // edge tiles for tile selection randomness
        List<Tile> startEdgeTiles = new List<Tile>();

        if(DirCardinals.start == -1) { // start cardinal can be set by MapGenerator
                                        // if random start is not desired
                                        // this is to keep the same capability as before
            DirCardinals.start = UnityEngine.Random.Range(0,4); // get the cardinals

            while(DirCardinals.start == DirCardinals.end) {
                DirCardinals.start = UnityEngine.Random.Range(0,4);
            }
        }

        //Debug.Log($"{DirCardinals.start}");
        
        coreStartGen();

        if(numStartPoints > 1)
            generateAdditionalStarts();
    }

    private void moveDown(int pathID = 0)
    {
        //Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        if(tileSet.tiles[currIndex].type == 0)
            tileSet.tiles[currIndex].type = 1;
        tileSet.pathTiles[pathID].Add(tileSet.tiles[currIndex]);
        nextIndex = currIndex - tileSetWidth;
        currentTile = tileSet.tiles[nextIndex];
        //Debug.Log("Moved Down.");
    }

    private void moveUp(int pathID = 0)
    {
        //Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        if(tileSet.tiles[currIndex].type == 0)
            tileSet.tiles[currIndex].type = 1;
        tileSet.pathTiles[pathID].Add(tileSet.tiles[currIndex]);
        nextIndex = currIndex + tileSetWidth;
        currentTile = tileSet.tiles[nextIndex];
        //Debug.Log("Moved Up.");
    }

    private void moveRight(int pathID = 0)
    {
        //Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        if(tileSet.tiles[currIndex].type == 0)
            tileSet.tiles[currIndex].type = 1;
        tileSet.pathTiles[pathID].Add(tileSet.tiles[currIndex]);
        nextIndex = ++currIndex;
        currentTile = tileSet.tiles[nextIndex];
        //Debug.Log("Moved Right.");
    }

    private void moveLeft(int pathID = 0) 
    {
        //Debug.Log($"Added {currentTile.position} as path.");
        currIndex = tileSet.tiles.IndexOf(currentTile);
        if(tileSet.tiles[currIndex].type == 0)
            tileSet.tiles[currIndex].type = 1;
        tileSet.pathTiles[pathID].Add(tileSet.tiles[currIndex]);
        nextIndex = --currIndex;
        currentTile = tileSet.tiles[nextIndex];
        //Debug.Log("Moved Left.");
    }

    // Getting the different quadrants will require
    // dividing the tileSetHeight and tileSetWidth by half
    // Ex: tileSetHeight = 8, tileSetWidth = 8
    //  1 2 3 4 5 6 7 8             0 1 2 3 4 5 6 7 
    // 1      |                    0      |
    // 2      |                    1      |
    // 3      |                    2      |  
    // 4______|________     ==>    3______|________
    // 5      |                    4      |   
    // 6      |                    5      |
    // 7      |                    6      |
    // 8      |                    7      |
    // from there we'll iterate through the tileSet.tiles list
    // and add the tiles to the appropriate quadrant list
    // then we'll randomly select a tile from each quadrant.

    // The borders will have to be excluded to prevent potential conflict
    // with the start and end tiles.
    // 1 | 2
    // -----
    // 3 | 4

    private List<Tile> generateQuadrantNodes() 
    {
        List<Tile> quadrantNodes = new List<Tile>();

        int tileSetHeightHalf = tileSetHeight / 2;
        int tileSetWidthHalf = tileSetWidth / 2;

        List<Tile> quadrant1 = new List<Tile>();
        List<Tile> quadrant2 = new List<Tile>();
        List<Tile> quadrant3 = new List<Tile>();
        List<Tile> quadrant4 = new List<Tile>();

        (int x, int y) tilePos;

        for(int i = 0; i < tileSet.tiles.Count; i++) {
            tilePos = tileSet.tiles[i].position;
            if(!((tilePos.x == 0 && tilePos.y <= tileSetHeight - 1) || 
                (tilePos.y == 0 && tilePos.x <= tileSetWidth - 1) ||
                (tilePos.x == tileSetWidth - 1 && tilePos.y <= tileSetHeight - 1) ||
                (tilePos.y == tileSetHeight - 1 && tilePos.x <= tileSetWidth - 1))) {
                if(tilePos.x < tileSetWidthHalf && tilePos.y < tileSetHeightHalf) {
                    quadrant1.Add(tileSet.tiles[i]);
                } else if(tilePos.x > tileSetWidthHalf && tilePos.y < tileSetHeightHalf) {
                    quadrant2.Add(tileSet.tiles[i]);
                } else if(tilePos.x < tileSetWidthHalf && tilePos.y > tileSetHeightHalf) {
                    quadrant3.Add(tileSet.tiles[i]);
                } else if(tilePos.x > tileSetWidthHalf && tilePos.y > tileSetHeightHalf) {
                    quadrant4.Add(tileSet.tiles[i]);
                }
            }
        }

        if(quadrant1.Count > 0) {
            quadrantNodes.Add(quadrant1[UnityEngine.Random.Range(0, quadrant1.Count)]);
        }

        if(quadrant2.Count > 0) {
            quadrantNodes.Add(quadrant2[UnityEngine.Random.Range(0, quadrant2.Count)]);
        }

        if(quadrant3.Count > 0) {
            quadrantNodes.Add(quadrant3[UnityEngine.Random.Range(0, quadrant3.Count)]);
        }

        if(quadrant4.Count > 0) {
            quadrantNodes.Add(quadrant4[UnityEngine.Random.Range(0, quadrant4.Count)]);
        }

        // time to cull through these random nodes to the lowered limit
        // only will want a max of 2 nodes total
        int randNodeCount = UnityEngine.Random.Range(1, quadrantNodes.Count - 1);

        for(int i = 0; i < randNodeCount; i++) {
            quadrantNodes.RemoveAt(UnityEngine.Random.Range(0, quadrantNodes.Count));
        }

        Tile closeStart = null, lonerChoice = null;

        if(quadrantNodes.Count > 1) {
            // need to reorganize the quadrantNodes to
            // point close to start - loner choice - point close to end
            // 
            float closestDistance = tileSetWidth * tileSetHeight;
            
            for(int i = 0; i < quadrantNodes.Count; i++) {
                float distance = (float) Vector2.Distance(new Vector2(quadrantNodes[i].position.x, quadrantNodes[i].position.y), 
                    new Vector2(tileSet.spawnTiles[0].position.x, tileSet.spawnTiles[0].position.y));
                if(distance < closestDistance) {
                    closestDistance = distance;
                    closeStart = quadrantNodes[i];
                }
            }

            quadrantNodes.Remove(closeStart);
            lonerChoice = quadrantNodes[0];
            quadrantNodes.Clear();
            quadrantNodes.Add(closeStart);
            quadrantNodes.Add(lonerChoice);
            //Debug.Log($"Node 1: {closeStart.position}");
            //Debug.Log($"Node 2: {lonerChoice.position}");
        }

        return quadrantNodes;
    }

    private void cullLoopPaths(int pathID = 0) {
        List<((int x, int y) position, int count)> positionList = new List<((int x, int y) position, int count)>();
        int loopCounter = 0;

        for(int i = 0; i < tileSet.pathTiles[pathID].Count; i++) {
            bool found = false;
            for(int j = 0; j < positionList.Count; j++) {
                if(positionList[j].position.x == tileSet.pathTiles[pathID][i].position.x && 
                    positionList[j].position.y == tileSet.pathTiles[pathID][i].position.y) {
                    found = true;
                    positionList[j] = (positionList[j].position, positionList[j].count + 1);
                    loopCounter++;
                }
            }

            if(!found) {
                positionList.Add((tileSet.pathTiles[pathID][i].position, 1));
            }
        }

        for(int i = 0; i < positionList.Count && loopCounter > 0; i++) {
            if(positionList[i].count > 1) {
                (int x, int y) cullLoopStartPosition = positionList[i].position;
                int index = tileSet.pathTiles[pathID].FindIndex(x => x.position.x == cullLoopStartPosition.x && x.position.y == cullLoopStartPosition.y);
                // delete all tiles from the pathTiles list that are in the loop
                // while preserving the start instance of the loop
                if(index != -1) {
                    int removedIndex = 0;
                    Tile removedTile = null;
                    for(int j = index+1; j < tileSet.pathTiles[pathID].Count; j++) {
                        if(tileSet.pathTiles[pathID][j].position.x == cullLoopStartPosition.x && 
                            tileSet.pathTiles[pathID][j].position.y == cullLoopStartPosition.y) { // should be second instance, don't change in tiles list
                            tileSet.pathTiles[pathID].RemoveAt(j);
                            positionList[i] = (positionList[i].position, positionList[i].count - 1);
                            loopCounter--;
                            //Debug.Log($"Removed tile at {removedTile.position}");
                            break;
                        } else {
                            removedTile = tileSet.pathTiles[pathID][j];
                            removedIndex = positionList.FindIndex(x => x.position.x == removedTile.position.x && x.position.y == removedTile.position.y);
                            if (removedIndex != -1) {
                                positionList[removedIndex] = (positionList[removedIndex].position, positionList[removedIndex].count - 1);
                                if (positionList[removedIndex].count == 0) {
                                    positionList.RemoveAt(removedIndex);
                                    tileSet.tiles[tileSet.tiles.IndexOf(removedTile)].type = 0;
                                }
                            }
                            tileSet.pathTiles[pathID].RemoveAt(j);
                            //Debug.Log($"Removed tile at {removedTile.position}");
                            j--;
                        }
                    }
                    i = 0;
                }
            }
        }
    }

    private void patchPath(int pathID = 0) {
        // need to fix up any gaps in the path
        Tile previousTile = tileSet.spawnTiles[pathID];

        int errorCounter = 0;
        for(int i = 1; i < tileSet.pathTiles[pathID].Count && errorCounter < 100; i++, errorCounter++) {
            float distance = (float) Vector2.Distance(new Vector2(tileSet.pathTiles[pathID][i].position.x, tileSet.pathTiles[pathID][i].position.y), 
                new Vector2(previousTile.position.x, previousTile.position.y));
            //Debug.Log($"Distance between {tileSet.pathTiles[pathID][i].position} and {previousTile.position} is {distance}");
            if(distance > 1) { // this means we'll have to patch the path
                List<Tile> segmentTiles = new List<Tile>();
                //Debug.Log("Need to patch path");
                for(int j = i; j < tileSet.pathTiles[pathID].Count; j++) {
                    //Debug.Log($"Added tile to needed patching: {tileSet.pathTiles[pathID][j].position}");
                    segmentTiles.Add(tileSet.pathTiles[pathID][j]);
                    //Debug.Log($"Attempting removal of index {j} of {tileSet.pathTiles[pathID].Count - 1}");
                    tileSet.pathTiles[pathID].RemoveAt(j);
                    j--;
                }

                // now we have a list of tiles that need to be patched to
                currentTile = previousTile;
                PathingLogic(segmentTiles, false, pathID);
            }
            previousTile = tileSet.pathTiles[pathID][i];
        }
    }

    private void PathingLogic(List<Tile> pathingNodes, bool start = false, int pathID = 0) {
        int counter = 0;
        reached = (false, false);
        bool moving = true;
        int index = 0;

        while(moving) {
            counter++;
            //Debug.Log($"CurrentTile: {currentTile.position}");
            if(counter == 1 && start) {
                switch(tileSet.DirCardinals[pathID].start) {
                    // 0 for bottom, 1 for left, 2 for top, 3 for right
                    case 0:
                        moveUp(pathID);
                        break;
                    case 1:
                        moveLeft(pathID);
                        break;
                    case 2:
                        moveDown(pathID);
                        break;
                    case 3:
                        moveRight(pathID);
                        break;
                }
            }
            
            if (counter > 100)
            {
                //Debug.Log("Loop ran too long, broke out of it.");
                break;
            }
            else if(!reached.y) {
                if (currentTile.position.y > pathingNodes[index].position.y)
                {
                    moveDown(pathID);
                }
                else if (currentTile.position.y < pathingNodes[index].position.y)
                {
                    moveUp(pathID);
                }
                else
                {
                    reached.y = true;
                }
            } else if(!reached.x) {
                if(currentTile.position.x > pathingNodes[index].position.x) 
                {
                    moveLeft(pathID);
                } 
                else if (currentTile.position.x < pathingNodes[index].position.x) 
                {
                    moveRight(pathID);
                }
                else
                {
                    reached.x = true;
                }
            } else if(reached.x && reached.y) {
                reached = (false, false);
                index++;
                if(index >= pathingNodes.Count) {
                    moving = false;
                }
            }
        }

        // always needed to be done at the end of the pathing logic
        tileSet.pathTiles[pathID].Add(pathingNodes[pathingNodes.Count - 1]);
    }

    private void findOffshootPaths() {
        // this function finds the point closest to any of the other spawn tiles
        // and then generates a path from that point to the spawn tile
        // while copying the paths before it to the new path
        
        int runCounter = 0;
        for(int i = 1; i < tileSet.spawnTiles.Count && runCounter < 100; i++, runCounter++) { 
            float shortestDistance = 1000;
            int closestTileIndex = 0;

            tileSet.pathTiles[i].Add(tileSet.spawnTiles[i]);

            for(int j = 0; j < tileSet.pathTiles[0].Count; j++) {
                float distance = (float) Vector2.Distance(new Vector2(tileSet.pathTiles[0][j].position.x, tileSet.pathTiles[0][j].position.y), 
                    new Vector2(tileSet.spawnTiles[i].position.x, tileSet.spawnTiles[i].position.y));
                if(distance < shortestDistance && tileSet.pathTiles[0][j].type == 1) {
                    shortestDistance = distance;
                    closestTileIndex = j;
                }
            }

            //Debug.Log($"{tileSet.spawnTiles[i].position}");

            for(int j = closestTileIndex; j < tileSet.pathTiles[0].Count; j++) {
                //Debug.Log($"{tileSet.pathTiles[0][j].position}");
                tileSet.pathTiles[i].Add(tileSet.pathTiles[0][j]);
            }

            //Debug.Log($"{tileSet.endTile.position} - {tileSet.pathTiles[i][tileSet.pathTiles[i].Count - 1].position}");

            // the path will be generated from the pathPatch function
            patchPath(i);
        }
    }

    private void generatePath() 
    {
        List<Tile> pathingNodes = generateQuadrantNodes();
        pathingNodes.Add(tileSet.endTile);
        currentTile = tileSet.spawnTiles[0]; // start at the first spawn tile

        //Debug.Log("Generating Path");
        PathingLogic(pathingNodes, true);
        cullLoopPaths(0);
        // a patch check is needed to see if the path is disconnected
        // checks the pathTiles list for any gaps in the path
        // tileSet.pathTiles.RemoveAt(tileSet.pathTiles.Count - 2);
        patchPath(0);

        if(tileSet.spawnTiles.Count > 1) {
            findOffshootPaths();
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
        if (tileSet.endTile == null)
            generateStartEnd();
        else
            generateStart();
        generatePath();
    }

    public override string ToString() {
        string output = "Grid View\n";

        for(int y = tileSetHeight - 1; y >= 0; y--) {
            for(int x = 0; x < tileSetWidth; x++) {
                int i = x + y * tileSetWidth;
                output += tileSet.tiles[i].type.ToString() + " ";
            }
            output+="\n";
        }
        output+=$"\ntileSetWidth: {tileSetWidth}\ntileSetHeight: {tileSetHeight}";

        output+="\n\nPath Tiles\n";
        for (int i = 0; i < tileSet.pathTiles.Count; i++)
        {
            output+=$"Path Number {i}\n";

            for(int y = tileSetHeight - 1; y >= 0; y--) {
                for(int x = 0; x < tileSetWidth; x++) {
                    if(tileSet.pathTiles[i].Any(tile => tile.position.x == x && tile.position.y == y))
                        output += tileSet.pathTiles[i].Find(tile => tile.position.x == x && tile.position.y == y)
                            .type.ToString() + " ";
                    else
                        output += "0 ";
                }
                output+="\n";
            }

            foreach (Tile tile in tileSet.pathTiles[i])
            {
                output += $"{tile.position}\n";
            }
        }

        output+="\n\nCardinals Driections\n";
        foreach((int start, int end) cardinal in tileSet.DirCardinals) {
            output += $"{cardinal}\n";
        }

        output+="\n\nSpawn Tiles\n";
        foreach(Tile tile in tileSet.spawnTiles) {
            output += $"{tile.position}\n";
        }
        return output;
    }
}
