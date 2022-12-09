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

    public GameObject upgradeButton;
    public static GameObject dummyUpgradeButton;

    private static GameObject currentTower;

    // Start is called before the first frame update
    private void Start()
    {
        if (main == null) main = this;
        currentTower = null;
        Debug.Log(menuUi);
        menuUi.SetActive(false);
        upgradeButton.SetActive(false);
        dummyUpgradeButton = upgradeButton;
        dummyUi = menuUi;//To make sure i dont lose track of the Tower Menu UI
    }

    //This trys to open the UI panel for upgrading and selling towers
    public void Open(GameObject T)
    {
        currentTower = T;
        Debug.Log("Tower " + currentTower.GetComponent<Towers>().getName() + " loaded");
        Debug.Log(dummyUpgradeButton);
        dummyUi.SetActive(true);//makes UI appear
        if (!currentTower.GetComponent<Towers>().canUpgrade())
        {//makes upgrade disappear if its already upgraded
            dummyUpgradeButton.SetActive(false);
        }
        else
            dummyUpgradeButton.SetActive(true);
    }

    public void Upgrade()
    {
        if ((shopManager.canUpgradeTower(currentTower)) && (currentTower.GetComponent<Towers>().canUpgrade()))
        {
            shopManager.upgradeTower(currentTower);
            currentTower.GetComponent<Towers>().upgrade();
            Debug.Log("Upgraded tower");
        }
        this.Close();
    }

    //This sells a tower and gives money back to the player
    public void Sell()
    {
        Debug.Log(currentTower.GetComponent<Towers>().getName());
        shopManager.sellTower(currentTower);
        Counter.towers.Remove(currentTower);
        Destroy(currentTower);
        this.Close();
        Debug.Log("Sold tower");//makes UI disappear
    }

    public void Close()
    {
        dummyUpgradeButton.SetActive(false);
        dummyUi.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (currentTower != null)
        {
            if (!currentTower.GetComponent<Towers>().canUpgrade())
            {//makes upgrade disappear if its already upgraded
                dummyUpgradeButton.SetActive(false);
            }
            else
                dummyUpgradeButton.SetActive(true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            this.Close();
            Debug.Log("Unclicked a tower");
        }
    }
}
