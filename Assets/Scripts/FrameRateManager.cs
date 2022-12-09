using UnityEngine;
using System.Collections;

public class FrameRateManager : MonoBehaviour
{

    [SerializeField]
    private byte frameRate = 60;

    private void Start()
    {
        StartCoroutine(changeFramerate());
    }

    private IEnumerator changeFramerate()
    {
        yield return new WaitForSeconds(1);
        Application.targetFrameRate = frameRate;
    }
}
