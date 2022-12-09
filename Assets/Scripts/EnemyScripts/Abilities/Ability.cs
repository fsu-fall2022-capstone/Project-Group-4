using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public AbilityType abilityType { get; private set; }
    public float cooldown { get; private set; }
    public float duration { get; private set; }
    protected float maxDuration;
    protected float maxCooldown;

    protected virtual void ConstructAbility(AbilityType abilityType, float duration, float cooldown)
    {
        this.abilityType = abilityType;
        this.maxDuration = duration;
        this.maxCooldown = cooldown;
        this.duration = 0;
        this.cooldown = 0;
    }

    public void startAbility()
    {
        duration = maxDuration;
        cooldown = maxCooldown;
    }

    public void updateAbility(float time)
    {
        if (duration > 0)
        {
            duration -= time;
        }
        else if (cooldown > 0)
        {
            cooldown -= time;
        }
    }

    public bool isRunning()
    {
        return duration > 0;
    }

    public bool isReady()
    {
        return cooldown <= 0;
    }

    // command functions to directly change the timers
    public void updateCooldown(float time)
    {
        cooldown -= time;
    }

    public void updateDuration(float time)
    {
        duration -= time;
    }

    protected List<GameObject> getEnemiesInRange(Vector3 position, float range)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        foreach (GameObject enemy in Counter.enemies)
        {
            if (enemy != null)
            {
                if (Vector3.Distance(position, enemy.transform.position) <= range)
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }
        return enemiesInRange;
    }
}