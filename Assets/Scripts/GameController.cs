using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverBG GameOver;
    private bool isGameOver = false; // resets every time scene is reloaded

    private void Update()
    {
        if (HealthBar.lives == 0f && !isGameOver)
        {
            GameOver.Show();
            isGameOver = true; // prevents the game over screen
                               // from being called multiple times
        }
    }
}
