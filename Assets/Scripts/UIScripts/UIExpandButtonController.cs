using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExpandButtonController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public static List<GameObject> buttons = new List<GameObject>();
    private static bool triggered;

    private void Update()
    {
        if (triggered)
        {
            triggered = false;
            generateButtons();
        }
    }

    public static void trigger(MapLayout data)
    {
        MapGenerator.main.expandMap(data);
        triggered = true;
    }

    private void generateButtons()
    {
        foreach (GameObject child in buttons)
        {
            Destroy(child);
        }

        buttons.Clear();

        foreach (MapLayout layout in MapGenerator.expandableTiles)
        {
            Transform position = MapGenerator.spawnTiles[layout.initPathID].transform;
            GameObject button = Instantiate(buttonPrefab, position);
            button.GetComponent<ExpandButton>().setData(layout);
            buttons.Add(button);
        }

    }
}