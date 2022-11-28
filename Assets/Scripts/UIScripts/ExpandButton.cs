using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandButton : MonoBehaviour
{
    public MapLayout data { get; private set; }

    // Update is called once per frame
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject &&
                data != null)
            {
                UIExpandButtonController.trigger(data);
            }
        }
    }

    public void setData(MapLayout newData)
    {
        data = newData;
    }
}