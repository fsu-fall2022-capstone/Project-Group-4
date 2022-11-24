using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static void loadNewGame()
    {
        Scene2Handler.clearListsFromGameScene();
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public static void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}