/*
    Most code in this file was written out by Nathan Granger based on the free tutorial 
    videos posted by youtube user ZeveonHD, found at 
    https://www.youtube.com/playlist?list=PL5AKnriDHZs5a8De2wK_qqrwBUqjZo0hN. Many
    function and variable names may have been changed and some parts of the code may
    have been modified to fit our game scheme, these sections will be marked with 
    comments. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager main;

    public ShopManager shopManager;

    public GameObject basicTowerObject;

    private GameObject currTowerPlacing;

    private GameObject dummyPlacement;

    private GameObject hoverTile;

    public Camera cam;

    public LayerMask mask;
    public LayerMask towerMask;

    public bool isBuilding;

    public void Start()
    {
        if (main == null) main = this;
    }

    public Vector2 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void GetCurrentHoverTile()
    {
        Vector2 mousePosition = GetMousePosition();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0,0), 0.1f, mask, -100, 100);

        if (hit.collider != null)
        {
            if (MapGenerator.mapTiles.Contains(hit.collider.gameObject))        //Check if obj is mapTile
            {
                if (!MapGenerator.pathTiles.Contains(hit.collider.gameObject))  //Check that mapTile is not pathTile
                {
                    hoverTile = hit.collider.gameObject;
                }
            }
        }
    }

    public bool checkForTower()
    {
        bool towerOnSlot = false;

        Vector2 mousePosition = GetMousePosition();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0,0), 0.1f, towerMask, -100, 100);
    
        if (hit.collider != null)
        {
            towerOnSlot = true;
        }

        return towerOnSlot;
    }

    public void PlaceBuilding()
    {
        if (hoverTile != null)
        {
            if (checkForTower() == false)
            {
                if (shopManager.canBuyTower(currTowerPlacing) == true)
                {
                    GameObject newTowerObj = Instantiate(currTowerPlacing);
                    newTowerObj.layer = LayerMask.NameToLayer("Tower");
                    newTowerObj.transform.position= hoverTile.transform.position;

                    EndBuilding();
                    shopManager.buyTower(currTowerPlacing);
                }
                else
                {
                    Debug.Log("Not enough money for Tower.. \n");
                }
            }
        }
    }

    public void StartBuilding(GameObject towerToBuild)
    {
        isBuilding = true;

        currTowerPlacing = towerToBuild;

        dummyPlacement = Instantiate(currTowerPlacing);

        if (dummyPlacement.GetComponent<Towers>() != null)
        {
            Destroy(dummyPlacement.GetComponent<Towers>());
        }

        if (dummyPlacement.GetComponent<BarrelRotation>() != null)
        {
            Destroy(dummyPlacement.GetComponent<BarrelRotation>());
        }
    }

    public void EndBuilding()
    {
        isBuilding = false;

        if (dummyPlacement != null)
        {
            Destroy(dummyPlacement);
        }
    }

    public void Update()
    {
        if (isBuilding == true)
        {
            if (dummyPlacement != null)
            {
                GetCurrentHoverTile();

                if (hoverTile != null)
                {
                    dummyPlacement.transform.position = hoverTile.transform.position;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                PlaceBuilding();
            }
        }
    }


}