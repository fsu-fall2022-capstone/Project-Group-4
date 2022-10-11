using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorInspector : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        MapGenerator gen = (MapGenerator)target;
        if(GUILayout.Button("Expand Map")) {
            gen.expandMap();
        }
    }
}
