/*
    Developing the camera boundary limits, scroll wheel and middle mouse
    is based on this tutorial: https://www.youtube.com/watch?v=IfbMKe6p9nM
    More work needs to be done with this.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController main;

    private Vector3 MouseScrollStartPos;
    private Camera mainCamera;


    // different speed values, serialized for editor adjustment to save in the code
    [SerializeField] private float MoveSpeed = 20f;
    [SerializeField] private float ZoomSpeed = 80f;

    private float spriteSize = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        if (main == null) main = this;
        mainCamera = GetComponent<Camera>();
        spriteSize = MapGenerator.main.getSpriteSize();
        mainCamera.orthographicSize = spriteSize * 5;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!HandleKeyInput())
        {
            HandleMouseInput();
        }
        HandleWheelScroll();
    }

    private bool HandleKeyInput()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            movement = new Vector3(0, MoveSpeed * Time.deltaTime, 0);
            mainCamera.transform.position += movement;
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            movement = new Vector3(0, -MoveSpeed * Time.deltaTime, 0);
            mainCamera.transform.position += movement;
        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            movement = new Vector3(-MoveSpeed * Time.deltaTime, 0, 0);
            mainCamera.transform.position += movement;
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            movement = new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
            mainCamera.transform.position += movement;
        }

        if (movement != Vector3.zero)
            return true;

        return false;
    }

    private bool HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
        {
            MouseScrollStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(2))
        {
            Vector3 movement = mainCamera.ScreenToWorldPoint(Input.mousePosition) - MouseScrollStartPos;
            mainCamera.transform.position -= movement;
            return true;
        }
        return false;
    }

    private bool HandleWheelScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            mainCamera.orthographicSize += Input.mouseScrollDelta.y * Time.deltaTime * ZoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, spriteSize * 2, spriteSize * 8);
            return true;
        }
        return false;
    }
}