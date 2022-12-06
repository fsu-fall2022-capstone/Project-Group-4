using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    public Quaternion pivot { get; private set; }
    [SerializeField] private Transform barrel;
    [SerializeField] private Towers tower;
    [SerializeField] private string spriteName = null;

    private float oldAngle = 0f;

    private void FixedUpdate()
    {
        if (tower != null)
        {
            if (tower.currentTarget != null)
            {
                updateData();
            }
        }
    }

    private void updateData()
    {
        // first update the pivot data
        Vector2 relative = tower.currentTarget.transform.position - barrel.position;
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        Vector3 newRotation = new Vector3(0, 0, angle);
        pivot = Quaternion.Euler(newRotation);

        // then update the sprite if the angle has vastly changed
        if (Math.Abs(oldAngle - angle) > 15)
        {
            TurretSpriteRenderer.main.UpdateTurretUnitSprite(barrel, spriteName, angle);
            oldAngle = angle;
        }

        if (!tower.aimReady)
        {
            tower.triggerAim();
        }
    }
}