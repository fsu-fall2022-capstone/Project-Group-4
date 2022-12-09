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


    private GameObject currObjPlacing;

    private GameObject dummyPlacement;

    private GameObject hoverTile;

    public Camera cam;

    public LayerMask mask;
    public LayerMask towerMask;

    private bool flag = false;

    public bool isPlacing;

    private void Start()
    {
        if (main == null) main = this;
    }

    private void Update()
    {
        if (isPlacing == true)
        {
            StartCoroutine(GetCurrentHoverTile());

            if (dummyPlacement != null)
            {
                if (hoverTile != null)
                    dummyPlacement.transform.position = hoverTile.transform.position;
            }

            if (Input.GetButtonDown("Fire1"))
                Placement();

            if (Input.GetButtonDown("Fire2"))
                EndPlacement();
        }
    }

    public Vector2 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public IEnumerator GetCurrentHoverTile()
    {
        Vector2 mousePosition = GetMousePosition();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 0), 0.1f, mask, -100, 100);

        if (hit.collider != null)
        {
            if (MapGenerator.mapTiles.Contains(hit.collider.gameObject) &&
                !Counter.towers.Contains(hit.collider.gameObject))        //Check if obj is mapTile
            {
                bool isPathTile = false;
                foreach (List<GameObject> path in MapGenerator.pathTiles)       //Check if obj is pathTile
                {
                    if (path.Contains(hit.collider.gameObject))  //Check that mapTile is not pathTile
                    {
                        isPathTile = true;
                        break;
                    }
                }

                if (!isPathTile) hoverTile = hit.collider.gameObject;
            }
        }

        yield return null;
    }

    public bool checkForTower()
    {
        Vector2 mousePosition = GetMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 0), 0.1f, towerMask, -100, 100);

        return (hit.collider != null) ? true : false;
    }

    public void Placement()
    {
        if (flag)
            PlaceBoon();
        else
            PlaceBuilding();
    }

    public void PlaceBoon()
    {
        if (hoverTile != null)
        {
            if (checkForTower() == false)
            {
                if (shopManager.canBuyBoon(currObjPlacing) == true)
                {
                    GameObject newTowerObj = Instantiate(currObjPlacing);
                    newTowerObj.layer = LayerMask.NameToLayer("Tower");
                    newTowerObj.GetComponent<SpriteRenderer>().sortingOrder = hoverTile.GetComponent<SpriteRenderer>().sortingOrder;
                    newTowerObj.transform.position = hoverTile.transform.position;

                    EndPlacement();
                    shopManager.buyBoon(currObjPlacing);
                }
                else
                {
                    Debug.Log("Not enough money for Boon.. \n");
                    EndPlacement();
                }
            }
        }
    }

    public void PlaceBuilding()
    {
        if (hoverTile != null)
        {
            if (checkForTower() == false)
            {
                if (shopManager.canBuyTower(currObjPlacing) == true)
                {
                    GameObject newTowerObj = Instantiate(currObjPlacing);
                    newTowerObj.layer = LayerMask.NameToLayer("Tower");
                    newTowerObj.GetComponent<SpriteRenderer>().sortingOrder = hoverTile.GetComponent<SpriteRenderer>().sortingOrder;
                    newTowerObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder =
                        hoverTile.GetComponent<SpriteRenderer>().sortingOrder + 1;
                    newTowerObj.transform.position = hoverTile.transform.position;

                    Counter.towers.Add(newTowerObj);

                    Counter.towers.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));

                    EndPlacement();
                    shopManager.buyTower(currObjPlacing);
                }
                else
                {
                    Debug.Log("Not enough money for Tower.. \n");
                    EndPlacement();
                }
            }
        }
    }

    public void StartPlacing(GameObject towerToBuild)
    {
        GetCurrentHoverTile();

        isPlacing = true;

        currObjPlacing = towerToBuild;

        dummyPlacement = Instantiate(currObjPlacing);

        if (dummyPlacement.GetComponent<Towers>() != null)
            Destroy(dummyPlacement.GetComponent<Towers>());


        if (dummyPlacement.GetComponent<BarrelRotation>() != null)
            Destroy(dummyPlacement.GetComponent<BarrelRotation>());


        if (dummyPlacement.GetComponent<Boon>() != null)
        {
            Destroy(dummyPlacement.GetComponent<Boon>());
            flag = true;
        }
    }

    public void EndPlacement()
    {
        isPlacing = false;

        if (dummyPlacement != null)
        {
            Destroy(dummyPlacement);
        }

        flag = false;
    }
}