#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(TerrainSetSO))]
public class TerrainSetSOEditor : Editor
{
    SerializedObject _serializedTerrainSet;
    bool _showTerrainTypesArray = true;

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

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = CreateTexture(2, 2, new Color(0.35f, 0.35f, 0.35f, 0.35f)) },
                padding = new RectOffset(5, 5, 5, 5)
            };

            EditorGUILayout.BeginVertical(boxStyle);

            for (int i = 0; i < terrainTypesArray.arraySize; i++)
            {
                var element = terrainTypesArray.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Terrain Type {i}:", GUILayout.Width(100));

                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, element.isExpanded ? "Collapse" : "Expand");

                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    terrainTypesArray.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();

                if (element.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("name"), new GUIContent("Name"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("height"), new GUIContent("Height"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("tile"), new GUIContent("Tile"));
                    EditorGUI.indentLevel--;
                }
            }

            if (GUILayout.Button("Add New Terrain Type"))
                terrainTypesArray.arraySize++;

            EditorGUILayout.EndVertical();
        }

        _serializedTerrainSet.ApplyModifiedProperties();
        if (GUI.changed) EditorUtility.SetDirty(terrainSet);
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