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

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager main;
    
    private int currPlayerMoney;
    public int startMoney;

    public void Start()
    {
        if (main == null) main = this;
        currPlayerMoney = startMoney;
    }

    public int GetCurrMoney()
    {
        return currPlayerMoney;
    }

    public void addMoney(int amount)
    {
        currPlayerMoney += amount;
    }

    public void removeMoney(int amount)
    {
        currPlayerMoney -= amount;
        Debug.Log("Removed" + amount + " from player's money! The player now has " + currPlayerMoney);
    }
}