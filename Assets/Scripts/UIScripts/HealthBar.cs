/*
     Written by Nathan Granger to handle the updating of the health/lives bar.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private Text healthBarTxt;

    public static float lives;

    private void Start()
    {
        lives = 5f;
    }

    private void Update()
    {
        updatePlayerHealth(lives);
    }

    public void updatePlayerHealth(float livesLeft, float max = 5)
    {
        healthBarTxt.text = "Lives: ";
        health.fillAmount = livesLeft / max;
        healthBarTxt.text += livesLeft + "/" + max;
    }
}
