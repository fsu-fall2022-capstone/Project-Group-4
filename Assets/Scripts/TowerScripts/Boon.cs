using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boon : MonoBehaviour
{
    public BoonType boonType; //Boon Effect
    public List<GameObject> towersInRange;
    public List<GameObject> enemiesInRange;
    public float duration; //Length in seconds of boon effects
    public float range; //Range of effect to nearby towers and/or enemies
    private float timeCheck;

    private void Start()
    {
        towersInRange = new List<GameObject>();
        enemiesInRange = new List<GameObject>();
        timeCheck = Time.time;
    }

    private void FixedUpdate()
    {
        if (boonType == BoonType.Fortune)
            updateEnemiesInRange();
        else
            updateTowersInRange();
        updateDuration();
        if (duration <= 0)
        {
            destroyBoon();
        }
    }

    //Decrement duration until 0, and then at 0, remove all boons from towers and/or enemies and destroy boon gameObject
    private void updateDuration()
    {
        duration -= (Time.time - timeCheck);
        timeCheck = Time.time;
    }

    //Determine towers in range and add spell effect, and then determines towers out of range and removes spell effect
    private void updateTowersInRange()
    {
        float distance;

        //Find towers in range and add spell effect, if not already added
        foreach (GameObject tower in Counter.towers)
        {
            if (tower != null)
            {
                if (!towersInRange.Contains(tower))
                {
                    distance = (transform.position - tower.transform.position).magnitude;

                    if (distance < range)
                    {
                        towersInRange.Add(tower);
                        tower.GetComponent<Towers>().addBoon(boonType);
                    }
                }
            }
        }

        //Parse towersInRange list and determine if any towers are out of range
        foreach (GameObject tower in towersInRange)
        {
            if (tower != null)
            {
                distance = (transform.position - tower.transform.position).magnitude;

                if (distance > range)
                {
                    towersInRange.Remove(tower);
                    tower.GetComponent<Towers>().removeBoon(boonType);
                }
            }
        }
    }

    //Determines enemies in range and add boon effect, and then determines enemies out of range and removes boon effect
    private void updateEnemiesInRange()
    {
        float distance;

        //Find enemies in range and add boon effect, if not already added
        foreach (GameObject enemy in Counter.enemies)
        {
            if (enemy != null)
            {
                if (!enemiesInRange.Contains(enemy))
                {
                    distance = (transform.position - enemy.transform.position).magnitude;

                    if (distance < range)
                    {
                        enemiesInRange.Add(enemy);
                        enemy.GetComponent<Enemy>().addBoon(boonType);
                    }
                }
            }
        }

        //Parse enemiesInRange list and determine if any enemies are out of range
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                distance = (transform.position - enemy.transform.position).magnitude;

                if (distance > range || enemy == null)
                {
                    enemiesInRange.Remove(enemy);
                    enemy.GetComponent<Enemy>().removeBoon(boonType);
                }
            }
        }
    }

    //Remove boon effect from all objects and destroy this object
    private void destroyBoon()
    {
        if (boonType == BoonType.Fortune)
        {
            foreach (GameObject enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().removeBoon(boonType);
                }
            }
        }
        else
        {
            foreach (GameObject tower in towersInRange)
            {
                if (tower != null)
                {
                    tower.GetComponent<Towers>().removeBoon(boonType);
                }
            }
        }
        Destroy(gameObject);
    }
}