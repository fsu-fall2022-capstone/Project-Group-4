using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverBG : MonoBehaviour
{
    public void Show()
    {
        MoneyManager.main.Hide();
        gameObject.SetActive(true);
        TimeHandler.PauseGameTime();
    }

    public void Hide()
    {
        TimeHandler.StartGameTime();
        MoneyManager.main.Show();
        gameObject.SetActive(false);
    }
}
