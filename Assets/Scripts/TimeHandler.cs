using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler main { get; private set; }

    private void Start()
    {
        if(main != null) main = this;
    }

    public static void StartGameTime() {
        Time.timeScale = 1.0f;
        Debug.Log("Started/Resumed Game Time...");
    }

    public static void PauseGameTime() {
        Time.timeScale = 0.0f;
        Debug.Log("Paused Game Time...");
    }
}