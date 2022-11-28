/*
This code was written by Daniel Pijeira
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    public bool quadTime = false;
    public bool doubleTime = false;

    //SpeedUp checks val of doubleTime and handles timeScale
    public void SpeedUp()
    {
        if (!doubleTime )
        {
            Time.timeScale = 2.00f;
            doubleTime = true;
            Debug.Log("Game Time Doubled...");
        }
        else if (doubleTime && !quadTime)
        {
            TimeHandler.StartGameTime();
            doubleTime = false;
        }
    }

}
