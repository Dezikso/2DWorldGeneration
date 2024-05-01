using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnvironmentObjectSetSO))]
public class EnvironmentObjectSetSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnvironmentObjectSetSO environmentObjectSet = (EnvironmentObjectSetSO)target;


        if (GUILayout.Button("Assign IDs Automatically"))
        {
            AssignTerrainTypeIDs(environmentObjectSet);
        }
    }

    private void AssignTerrainTypeIDs(EnvironmentObjectSetSO envObjectSet)
    {
        List<EnvironmentObjectType> modifiedEnvObjectTypes = new List<EnvironmentObjectType>();

        for (int i = 0; i < envObjectSet.environmentObjectTypes.Count; i++)
        {
            EnvironmentObjectType envObjectType = envObjectSet.environmentObjectTypes[i];
            modifiedEnvObjectTypes.Add(new EnvironmentObjectType(i, envObjectType.name, envObjectType.terrainTypeID, envObjectType.spawnAmount, envObjectType.width, envObjectType.height, envObjectType.objectPrefab));
        }

        envObjectSet.environmentObjectTypes = modifiedEnvObjectTypes;
        EditorUtility.SetDirty(envObjectSet);
    }
}
