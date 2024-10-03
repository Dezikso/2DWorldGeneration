#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Data.ScriptableObjects;

[CustomEditor(typeof(TerrainSetSO))]
public class TerrainSetSOEditor : Editor
{
    private SerializedObject _serializedTerrainSet;
    private bool _showTerrainTypesArray = true;

    void OnEnable()
    {
        _serializedTerrainSet = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        _serializedTerrainSet.Update();

        var terrainSet = (TerrainSetSO)target;

        _showTerrainTypesArray = EditorGUILayout.Foldout(_showTerrainTypesArray, "Terrain Types");

        if (_showTerrainTypesArray)
        {
            var terrainTypesArray = _serializedTerrainSet.FindProperty("terrainTypes");
            EditorGUILayout.PropertyField(terrainTypesArray.FindPropertyRelative("Array.size"), new GUIContent("Size"));

            EditorGUILayout.BeginVertical(CreateGUIStyle(new Color(0.35f, 0.35f, 0.35f, 0.35f), 5));

            for (int i = 0; i < terrainTypesArray.arraySize; i++)
            {
                var element = terrainTypesArray.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal(CreateGUIStyle(new Color(0.1f, 0.1f, 0.1f, 0.1f), 1));
                EditorGUILayout.LabelField($"{element.FindPropertyRelative("name").stringValue}", GUILayout.Width(65));

                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, element.isExpanded ? "Collapse" : "Expand");

                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    terrainTypesArray.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();

                if (element.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField($"ID: {i}", EditorStyles.boldLabel, GUILayout.Width(100));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("name"), new GUIContent("Name"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("height"), new GUIContent("Height"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("tile"), new GUIContent("Tile"));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(12.5f);
                }
            }

            if (GUILayout.Button("Add New Terrain Type"))
                terrainTypesArray.arraySize++;

            EditorGUILayout.EndVertical();
        }

        _serializedTerrainSet.ApplyModifiedProperties();
        if (GUI.changed) EditorUtility.SetDirty(terrainSet);
    }

    private GUIStyle CreateGUIStyle(Color color, int padding)
    {
        GUIStyle style = new GUIStyle(GUI.skin.box)
        {
            normal = { background = CreateTexture(2, 2, color) },
            padding = new RectOffset(padding, padding, padding, padding)
        };

        return style;
    }

    private Texture2D CreateTexture(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}
#endif