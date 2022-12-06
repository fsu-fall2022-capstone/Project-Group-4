using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager main;
    [SerializeField] private GameObject[] canvasObjects;

    private void Start()
    {
        if (main == null) main = this;
    }

    public void Show()
    {
        foreach (GameObject canvasObject in canvasObjects)
        {
            canvasObject.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (GameObject canvasObject in canvasObjects)
        {
            canvasObject.SetActive(false);
        }
    }
}