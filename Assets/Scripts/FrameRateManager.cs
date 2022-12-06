using UnityEngine;
using System.Collections;

public class FrameRateManager : MonoBehaviour
{

    public byte frameRate = 60;

    void Start()
    {
        StartCoroutine(changeFramerate());
    }
    IEnumerator changeFramerate()
    {
        yield return new WaitForSeconds(1);
        Application.targetFrameRate = frameRate;
    }
}