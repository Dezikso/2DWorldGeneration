using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGenerator.autoUpdate)
            {
                mapGenerator.ClearEnvironment();
                mapGenerator.GenerateTilemap();
            }
        }

        if (GUILayout.Button("Generate Tilemap"))
        {
            mapGenerator.GenerateTilemap();
        }
        if (GUILayout.Button("Spawn Environment Objects"))
        {
            mapGenerator.SpawnEnvironmentObjects();
        }
        if (GUILayout.Button("Clear Map"))
        {
            mapGenerator.ClearTilemap();
        }        if (GUILayout.Button("Clear Environment"))
        {
            mapGenerator.ClearEnvironment();
        }
    }
}
