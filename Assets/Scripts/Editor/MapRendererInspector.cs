using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapRenderer))]
public class MapRendererInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapRenderer ren = (MapRenderer)target;
        if (GUILayout.Button("Update Sorting Order"))
        {
            ren.UpdateSortingOrder();
        }
    }
}