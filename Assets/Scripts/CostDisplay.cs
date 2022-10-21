/*
Class made by Alex Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostDisplay : MonoBehaviour
{
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text costTxt;
    [SerializeField] private GameObject BasicTower;

    // Start is called before the first frame update
    private void Start()
    {
        Towers x = BasicTower.GetComponent<Towers>();//gets the data from the component
        nameTxt.text = x.getName();//So it can pull its name and cost
        costTxt.text = "$" + x.getCost().ToString();
        
    }

    private void Update(){
        //leave empty no need to update the labels constantly
    }

}
