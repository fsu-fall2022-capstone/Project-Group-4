using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverBG GameOver;
    // Update is called once per frame
    void Update()
    {
        if (HealthBar.lives == 0f)
        {
            GameOver.Show();
        }
    }
}
