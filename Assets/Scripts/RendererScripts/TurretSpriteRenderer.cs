using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurretSpriteRenderer : SpriteLoader
{
    public static TurretSpriteRenderer main;

    protected void Start()
    {
        if (main == null) main = this;
        base.LoadDictionary();
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