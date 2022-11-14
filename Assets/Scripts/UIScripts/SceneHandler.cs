using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static void loadNewGame()
    {
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public static void Quit()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}