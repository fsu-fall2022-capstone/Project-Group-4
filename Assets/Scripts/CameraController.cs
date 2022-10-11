/* built based on: https://www.youtube.com/watch?v=IfbMKe6p9nM
   this will be requiring some tweaking to perfect for this
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController main;

    private Vector3 MouseScrollStartPos;
    private Camera mainCamera;
    //private MapGenerator gen;
    private int BorderSize=30;
    private float MoveSpeed = 5f;
    private float EdgeScrollSpeed = 1f;
    private float ZoomSpeed = 10f;

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
        HandleBtnInput();
        HandleMiddleMouseBtn();
        HandleEdgeScroll();
        HandleWheelScroll();
        RestrictToBoundaryLimits();
    }

    public Rect GetBoundaryLimits()
    {
        (int w, int h) size = MapGenerator.main.getMapSize();
        GameObject cornerTile = MapGenerator.main.getCornerTile();
        return new Rect(new Vector2(cornerTile.transform.position.x,
                        cornerTile.transform.position.y/(size.h/2)), new Vector2(size.w, size.h));
    }

    private void RestrictToBoundaryLimits()
    {
        Rect boundaries = GetBoundaryLimits();
        if (boundaries.xMin > mainCamera.transform.position.x){
            mainCamera.transform.position = new Vector3(boundaries.xMin, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        if (boundaries.xMax < mainCamera.transform.position.x){
            mainCamera.transform.position = new Vector3(boundaries.xMax, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        if (boundaries.yMin > mainCamera.transform.position.y){
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, boundaries.yMin, mainCamera.transform.position.z);
        }
        if (boundaries.yMax < mainCamera.transform.position.y){
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, boundaries.yMax, mainCamera.transform.position.z);
        }
    }

    private void HandleBtnInput() 
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey("up")) {
            movement = new Vector3(0,MoveSpeed * Time.deltaTime,0);
            mainCamera.transform.position += movement;
        }
        if(Input.GetKey("down")) {
            movement = new Vector3(0,-MoveSpeed * Time.deltaTime,0);
            mainCamera.transform.position += movement;
        }
        if(Input.GetKey("left")) {
            movement = new Vector3(-MoveSpeed * Time.deltaTime,0,0);
            mainCamera.transform.position += movement;
        }
        if(Input.GetKey("right")) {
            movement = new Vector3(MoveSpeed * Time.deltaTime,0,0);
            mainCamera.transform.position += movement;
        }
    }

    private void HandleMiddleMouseBtn()
    {
        if (Input.GetMouseButtonDown(2))
        {
            MouseScrollStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2)){
            Vector3 movement = Vector3.zero;
            movement = mainCamera.ScreenToWorldPoint(Input.mousePosition) - MouseScrollStartPos;
            mainCamera.transform.position -= movement;
        }
    }

    private void HandleEdgeScroll() 
    {
        int distanceToTop = mainCamera.pixelHeight - (int)Input.mousePosition.y;
        int distanceToBottom = (int)Input.mousePosition.y;
        int distanceToRight = mainCamera.pixelWidth - (int)Input.mousePosition.x;
        int distanceToLeft = (int)Input.mousePosition.x;

        if(distanceToTop < BorderSize && distanceToTop > 0){
            mainCamera.transform.position += Vector3.up * Time.deltaTime * (BorderSize - distanceToTop) * EdgeScrollSpeed;
        } else if(distanceToBottom < BorderSize && distanceToBottom > 0){
             mainCamera.transform.position += Vector3.down * Time.deltaTime * (BorderSize - distanceToBottom) * EdgeScrollSpeed;
        }

        if(distanceToLeft < BorderSize && distanceToLeft > 0) {
            mainCamera.transform.position += Vector3.left * Time.deltaTime * (BorderSize - distanceToLeft) * EdgeScrollSpeed;
        } else if(distanceToRight < BorderSize && distanceToRight > 0) {
            mainCamera.transform.position += Vector3.right * Time.deltaTime * (BorderSize - distanceToRight) * EdgeScrollSpeed;
        }       
    }

    private void HandleWheelScroll() 
    {
        if(Input.mouseScrollDelta.y != 0) {
            mainCamera.orthographicSize += Input.mouseScrollDelta.y * Time.deltaTime * ZoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2, 10);
        }
    }
}
