#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private MethodInfo generateMapMethod;
    private MethodInfo generateTilemapMethod;
    private MethodInfo spawnEnvironmentObjectsMethod;
    private MethodInfo clearTilemapMethod;
    private MethodInfo clearEnvironmentMethod;

    private MapGenerator _mapGenerator;

    private void OnEnable()
    {
        _mapGenerator = (MapGenerator)target;

        generateMapMethod = _mapGenerator.GetType().GetMethod("GenerateMap", BindingFlags.NonPublic | BindingFlags.Instance);
        generateTilemapMethod = _mapGenerator.GetType().GetMethod("GenerateTilemap", BindingFlags.NonPublic | BindingFlags.Instance);
        spawnEnvironmentObjectsMethod = _mapGenerator.GetType().GetMethod("SpawnEnvironmentObjects", BindingFlags.NonPublic | BindingFlags.Instance);
        clearTilemapMethod = _mapGenerator.GetType().GetMethod("ClearTilemap", BindingFlags.NonPublic | BindingFlags.Instance);
        clearEnvironmentMethod = _mapGenerator.GetType().GetMethod("ClearEnvironment", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            if (_mapGenerator.AutoUpdate && generateMapMethod != null)
                generateMapMethod.Invoke(_mapGenerator, null);
        }

        if (GUILayout.Button("Generate Tilemap") && generateTilemapMethod != null)
            generateTilemapMethod.Invoke(_mapGenerator, null);

        if (GUILayout.Button("Spawn Environment Objects") && spawnEnvironmentObjectsMethod != null)
            spawnEnvironmentObjectsMethod.Invoke(_mapGenerator, null);

        if (GUILayout.Button("Clear Map") && clearTilemapMethod != null)
            clearTilemapMethod.Invoke(_mapGenerator, null);

        if (GUILayout.Button("Clear Environment") && clearEnvironmentMethod != null)
            clearEnvironmentMethod.Invoke(_mapGenerator, null);
    }
}
#endif