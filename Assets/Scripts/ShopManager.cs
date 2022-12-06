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

public class ShopManager : MonoBehaviour
{
    public static ShopManager main;

    public MoneyManager moneyManager;

    private void Start()
    {
        if (main == null) main = this;
    }

    //This function pulls its cost from BasicTowers function modified by Alex Martinez
    public int GetTowerCost(GameObject towerPrefab)
    {
        Towers tower = towerPrefab.GetComponent<Towers>();

        return tower.getCost();
    }

    public int GetBoonCost(GameObject boonPrefab)
    {
        Boon boon = boonPrefab.GetComponent<Boon>();

        return boon.getCost();
    }

    public void buyTower(GameObject towerPrefab)
    {
        moneyManager.removeMoney(GetTowerCost(towerPrefab));
    }

    public void buyBoon(GameObject boonPrefab)
    {
        moneyManager.removeMoney(GetBoonCost(boonPrefab));
    }

    public bool canBuyTower(GameObject towerPrefab)
    {
        int cost = GetTowerCost(towerPrefab);

        bool canBuy = false;

        if (moneyManager.GetCurrMoney() >= cost)
            canBuy = true;

        return canBuy;
    }

    public bool canBuyBoon(GameObject boonPrefab)
    {
        int cost = GetBoonCost(boonPrefab);

        bool canBuy = false;

        if (moneyManager.GetCurrMoney() >= cost)
            canBuy = true;

        return canBuy;
    }
}
