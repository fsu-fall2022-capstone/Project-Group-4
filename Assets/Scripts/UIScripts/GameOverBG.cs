using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverBG : MonoBehaviour
{
    public void Show()
    {
        CanvasManager.main.Hide();
        gameObject.SetActive(true);
        TimeHandler.PauseGameTime();
    }

    public void Hide()
    {
        CanvasManager.main.Show();
        TimeHandler.StartGameTime();
        gameObject.SetActive(false);
    }
}
