//Class made by Alex Martinez
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager main;
    public ShopManager shopManager;

    public GameObject menuUi;
    public static GameObject dummyUi;

    private static GameObject currentTower; 

    // Start is called before the first frame update
    private void Start()
    {
        if (main == null) main = this;
        currentTower = null;
        Debug.Log(menuUi);
        menuUi.SetActive(false);
        dummyUi = menuUi;//To make sure i dont lose track of the Tower Menu UI
    }

    //This trys to open the UI panel for upgrading and selling towers
    public void Open(GameObject T) {
        currentTower = T;
        Debug.Log("Tower " + currentTower.GetComponent<Towers>().getName() + " loaded");
        Debug.Log(dummyUi);
        dummyUi.SetActive(true);//makes UI appear
    }

    public void Upgrade() {
        
        this.Close();
    }
    
    //This sells a tower and gives money back to the player
    public void Sell() {
        Debug.Log(currentTower.GetComponent<Towers>().getName());
        shopManager.sellTower(currentTower);
        Counter.towers.Remove(currentTower);
        Destroy(currentTower);
        this.Close();
        Debug.Log("Sold tower");//makes UI disappear
    }

    public void Close() {
        dummyUi.SetActive(false);
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            this.Close();
            Debug.Log("Unclicked a tower");
        }
    }
}
