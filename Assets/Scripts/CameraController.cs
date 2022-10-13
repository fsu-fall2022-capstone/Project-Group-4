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
    
    [SerializeField] private int BorderSize = 15;

    // different speed values, serialized for editor adjustment to save in the code
    [SerializeField] private float MoveSpeed = 20f;
    [SerializeField] private float EdgeScrollSpeed = 1f;
    [SerializeField] private float ZoomSpeed = 80f;

    // Start is called before the first frame update
    void Start()
    {
        if (main == null) main = this;
        mainCamera = GetComponent<Camera>();   
        //gen = GetComponent<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!HandleKeyInput()){
            HandleMouseInput();
        }
        HandleWheelScroll();
//        RestrictToBoundaryLimits();
    }

/*
    public Rect GetBoundaryLimits() // sets the maximum boundary the camera can roam
    {   // this way people don't lost in the void
        (int w, int h) size = MapGenerator.main.getMapSize();
        GameObject cornerTile = MapGenerator.main.getCornerTile();
        return new Rect(new Vector2(cornerTile.transform.position.x,
                        cornerTile.transform.position.y/(size.h/2)), new Vector2(size.w, size.h));
    }


    private void RestrictToBoundaryLimits()
    {
        Rect boundaries = GetBoundaryLimits(); // limits are based on the rectangle generated
        // around the map size
        if (boundaries.xMin > mainCamera.transform.position.x){
            mainCamera.transform.position = new Vector3(boundaries.xMin, 
                mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        if (boundaries.xMax < mainCamera.transform.position.x){
            mainCamera.transform.position = new Vector3(boundaries.xMax, 
                mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        if (boundaries.yMin > mainCamera.transform.position.y){
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, 
                boundaries.yMin, mainCamera.transform.position.z);
        }
        if (boundaries.yMax < mainCamera.transform.position.y){
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, 
                boundaries.yMax, mainCamera.transform.position.z);
        }
    }
*/
    private bool HandleKeyInput() 
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey("w") || Input.GetKey("up")) {
            movement = new Vector3(0,MoveSpeed * Time.deltaTime,0);
            mainCamera.transform.position += movement;
        }
        if(Input.GetKey("s") || Input.GetKey("down")) {
            movement = new Vector3(0,-MoveSpeed * Time.deltaTime,0);
            mainCamera.transform.position += movement;
        }
        if(Input.GetKey("a") || Input.GetKey("left")) {
            movement = new Vector3(-MoveSpeed * Time.deltaTime,0,0);
            mainCamera.transform.position += movement;
        }
        if(Input.GetKey("d") || Input.GetKey("right")) {
            movement = new Vector3(MoveSpeed * Time.deltaTime,0,0);
            mainCamera.transform.position += movement;
        }

        if(movement != Vector3.zero) {
            return true;
        }

        return false;
    }

    private bool HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
        {
            MouseScrollStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(2)){
            Vector3 movement = mainCamera.ScreenToWorldPoint(Input.mousePosition) - MouseScrollStartPos;
            mainCamera.transform.position -= movement;
            return true;
        }
        return false;
    }

    private bool HandleWheelScroll() 
    {
        if(Input.mouseScrollDelta.y != 0) {
            mainCamera.orthographicSize += Input.mouseScrollDelta.y * Time.deltaTime * ZoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2, 8);
            return true;
        } 
        return false;
    }
}