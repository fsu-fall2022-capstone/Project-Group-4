using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this function is more like a sorting order renderer
// most layers are dependent on the tile location

public class MapRenderer : MonoBehaviour {
    public static MapRenderer main;
    private int previousMapCount = 0;
    public static bool activeRenderer = true;

    protected void Start() {
        if (main == null) main = this;
    }

    protected void Update() {
        if (previousMapCount != MapGenerator.mapTiles.Count && activeRenderer) {
            Debug.Log("MapRenderer: Map count changed from " + previousMapCount + " to " + MapGenerator.mapTiles.Count);
            previousMapCount = MapGenerator.mapTiles.Count;
            updateSortingLayerValue();
        }
    }
    
    public static void triggerRenderer() {
        activeRenderer = !activeRenderer;
    }

    public void UpdateSortingOrder() { // should only be utilized by the inspector
        updateSortingLayerValue();
    }

    private void updateSortingLayerValue() {
        int layerCount = 32767;
        for(int i = 0; i < previousMapCount && layerCount > -32767; i++) {
            GameObject mapTile = MapGenerator.mapTiles[i];
            
            // if there was a previous tile, give it the same layer
            if (i > 0) {
                GameObject previousMapTile = MapGenerator.mapTiles[i - 1];
                Debug.Log("MapRenderer: Comparing " + mapTile.transform.position + " to " + previousMapTile.transform.position);
                if (mapTile.transform.position.y == previousMapTile.transform.position.y) {
                    mapTile.GetComponent<SpriteRenderer>().sortingOrder = previousMapTile.GetComponent<SpriteRenderer>().sortingOrder;
                } else
                    mapTile.GetComponent<SpriteRenderer>().sortingOrder = --layerCount;
            } else
                mapTile.GetComponent<SpriteRenderer>().sortingOrder = layerCount;
        }
    }
}