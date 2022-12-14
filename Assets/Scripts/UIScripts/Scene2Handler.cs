using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2Handler : MonoBehaviour
{
    public static void NewGame()
    {
        clearListsFromGameScene();
        TimeHandler.PauseGameTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset the Scene...");
        TimeHandler.StartGameTime();
    }

    public static void mainMenu()
    {
        clearListsFromGameScene();
        TimeHandler.PauseGameTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Enemy.resetForMainMenu();
        Debug.Log("Loading Menu...");
        TimeHandler.StartGameTime();
    }

    public static void clearListsFromGameScene()
    {
        MapGenerator.clearMapGenerator();
        Counter.clearCounter();
    }

    public static void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}