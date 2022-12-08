/*
This code was written by Daniel Pijeira. Using the YouTube tutorial
https://www.youtube.com/watch?v=JivuXdrIHK0&ab_channel=Brackeys
This script implements the logic for the pause menu buttons.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        
        TimeHandler.StartGameTime();
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        
        TimeHandler.PauseGameTime();
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Scene2Handler.mainMenu();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
