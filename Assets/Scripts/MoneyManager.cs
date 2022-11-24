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
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private Text playerMoneyTxt;

    public static MoneyManager main;
    
    private static int currPlayerMoney;
    private int counter;
    public int startMoney;

    private void Start()
    {
        if (main == null) main = this;
        currPlayerMoney = startMoney;
        counter = 0;
    }

    public int GetCurrMoney()
    {
        return currPlayerMoney;
    }

    //Modified to change and print out the players money when added to
    public void addMoney(int amount)
    {
        if (counter == 0)
            currPlayerMoney = 1000;        //If startMoney changes, change this to match
        currPlayerMoney = GetCurrMoney() + amount;
        playerMoneyTxt.text = $"Money: ${currPlayerMoney}";
        counter++;
    }

    //Modified to change and print out the players money when taken away
    public void removeMoney(int amount)
    {
        currPlayerMoney -= amount;
        playerMoneyTxt.text = $"Money: ${currPlayerMoney}";
        Debug.Log($"Removed {amount} from {currPlayerMoney+amount} to {currPlayerMoney}");
    }

    public void Hide()
    {
        playerMoneyTxt.text = "";
    }

    public void Show()
    {
        playerMoneyTxt.text = $"Money: ${currPlayerMoney}";
    }
}