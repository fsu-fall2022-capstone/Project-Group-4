using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurretSpriteRenderer : SpriteLoader
{
    public static TurretSpriteRenderer main;
    private int previousMapCount = 0;
    public static bool activeRenderer = true;

    protected void Start()
    {
        if (main == null) main = this;
        base.LoadDictionary();
    }

    protected void FixedUpdate()
    {
        if (previousMapCount != Counter.towers.Count && activeRenderer)
        {
            Debug.Log("TurretSpriteRenderer: Map count changed from " + previousMapCount + " to " + Counter.towers.Count);
            previousMapCount = Counter.towers.Count;
            updateSortingLayerValue();
        }
    }

    public void UpdateSortingOrder()
    { // should only be utilized by the inspector
        updateSortingLayerValue();
    }

    // this function is more like a sorting order renderer
    // most layers are dependent on the tile location
    private void updateSortingLayerValue()
    {
        int layerCount = 32766; // max is 32767

        for (int i = 0; i < Counter.towers.Count && layerCount > -32767; i++)
        {
            GameObject tower = Counter.towers[i];

            // if there was a previous tile, give it the same layer
            if (i > 0)
            {
                GameObject previousTower = Counter.towers[i - 1];
                if (tower.transform.position.y == previousTower.transform.position.y)
                {
                    tower.GetComponent<SpriteRenderer>().sortingOrder = previousTower.GetComponent<SpriteRenderer>().sortingOrder;
                    tower.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder =
                        previousTower.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;
                }
                else
                {
                    tower.GetComponent<SpriteRenderer>().sortingOrder = --layerCount;
                    tower.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = layerCount + 1;
                }
            }
            else
            {
                tower.GetComponent<SpriteRenderer>().sortingOrder = layerCount;
                tower.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = layerCount + 1;
            }
        }
    }

    public void UpdateTurretUnitSprite(Transform barrel, string spriteName, float angle)
    {
        if (string.IsNullOrEmpty(spriteName)) return;

        string direction = "_";

        if (angle > 15 && angle <= 45)
            direction += "NE"; // NE
        else if (angle > 45 && angle <= 135)
            direction += "N"; // N
        else if (angle > 135 && angle <= 165)
            direction += "NW"; // NW
        else if (angle > 165 || angle <= -165)
            direction += "W"; // W
        else if (angle > -165 && angle <= -135)
            direction += "SW"; // SW
        else if (angle > -135 && angle <= -45)
            direction += "S"; // S
        else if (angle > -45 && angle <= -15)
            direction += "SE"; // SE
        else
            direction += "E"; // E


        barrel.GetComponent<SpriteRenderer>().sprite =
                base.GetSpriteByName(spriteName + direction);
    }
}