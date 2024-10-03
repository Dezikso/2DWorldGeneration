using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainSetSO))]
public class TerrainSetSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainSetSO terrainSet = (TerrainSetSO)target;

        
        if (GUILayout.Button("Assign IDs Automatically"))
        {
            AssignTerrainTypeIDs(terrainSet);
        }
    }

    private void AssignTerrainTypeIDs(TerrainSetSO terrainSet)
    {
        List<TerrainType> modifiedTerrainTypes = new List<TerrainType>();

        for (int i = 0; i < terrainSet.terrainTypes.Count; i++)
        {
            TerrainType terrainType = terrainSet.terrainTypes[i];
            modifiedTerrainTypes.Add(new TerrainType(i, terrainType.name, terrainType.height, terrainType.tile));
        }

        terrainSet.terrainTypes = modifiedTerrainTypes;
        EditorUtility.SetDirty(terrainSet);
    }

}
