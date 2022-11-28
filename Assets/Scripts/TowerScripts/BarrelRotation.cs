/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names may have been changed and some parts of the code may
    have been modified to fit our game scheme, these sections will be marked with 
    comments. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    public Transform pivot;
    public Transform barrel;

    public Towers tower;

    private void FixedUpdate()
    {
        if (tower != null)
        {
            if (tower.currentTarget != null)
            {
                Vector2 relative = tower.currentTarget.transform.position - pivot.position;

                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

                Vector3 newRotation = new Vector3(0, 0, angle);

                pivot.localRotation = Quaternion.Euler(newRotation);
            }
        }
    }
}