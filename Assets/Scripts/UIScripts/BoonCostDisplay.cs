/*
    Class made by Nathan Granger (modified Alex's CostDisplay script to work for Boons)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoonCostDisplay : MonoBehaviour
{
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text costTxt;
    [SerializeField] private GameObject Boon;

    // Start is called before the first frame update
    private void Start()
    {
        Boon x = Boon.GetComponent<Boon>();         //gets components data
        nameTxt.text = x.getName();                  //Pull components name then cost
        costTxt.text = "$" + x.getCost().ToString();

    }

    private void Update()
    {
        //leave empty no need to update the labels constantly
    }

}
