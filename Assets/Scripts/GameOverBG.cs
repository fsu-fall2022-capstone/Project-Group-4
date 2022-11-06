using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverBG : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void gameTime()
    {
        Time.timeScale = 1.0f;
        Debug.Log("Reset the Game Time...");
        gameObject.SetActive(false);
    }
}
