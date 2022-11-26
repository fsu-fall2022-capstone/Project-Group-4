using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boon : MonoBehaviour
{
    public BoonType boonType; //Boon Effect
    public List<GameObject> towersInRange = new List<GameObject>();
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public float duration; //Length in seconds of boon effects
    public float range; //Range of effect to nearby towers and/or enemies

    private void Start()
    {
 //       towersInRange = new List<GameObject>;
 //       enemiesInRange = new List<GameObject>;
    }

    private void FixedUpdate()
    {
        updateTowersInRange();
        updateEnemiesInRange();
        updateDuration();
    }

    //Decrement duration until 0, and then at 0, remove all boons from towers and/or enemies and destroy boon gameObject
    private void updateDuration()
    {
        
    }

    //Determine towers in range and add spell effect, and then determines towers out of range and removes spell effect
    private void updateTowersInRange()
    {
        //Find towers in range and add spell effect, if not already added
        foreach (GameObject tower in Counter.towers)
        {
            float distance = (transform.position - tower.transform.position).magnitude;

            if (distance < range)
            {
                //Add boon effect to tower
            }
        }

        //Parse towersInRange list and determine if any towers are out of range, and remove boon effect
    }

    //Determines enemies in range and add boon effect, and then determines enemies out of range and removes boon effect
    private void updateEnemiesInRange()
    {

    }
}