#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnvironmentObjectSetSO))]
public class EnvironmentObjectSetSOEditor : Editor
{
    SerializedObject _serializedEnvironmentObjectSet;
    bool _showEnvironmentObjectTypesArray = true;

    void OnEnable()
    {
        _serializedEnvironmentObjectSet = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        _serializedEnvironmentObjectSet.Update();

        var config = (EnvironmentObjectSetSO)target;

        _showEnvironmentObjectTypesArray = EditorGUILayout.Foldout(_showEnvironmentObjectTypesArray, "Environment Object Types");

        if (_showEnvironmentObjectTypesArray)
        {
            var environmentObjectTypesArray = _serializedEnvironmentObjectSet.FindProperty("environmentObjectTypes");
            EditorGUILayout.PropertyField(environmentObjectTypesArray.FindPropertyRelative("Array.size"), new GUIContent("Size"));

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = CreateTexture(2, 2, new Color(0.35f, 0.35f, 0.35f, 0.35f)) },
                padding = new RectOffset(5, 5, 5, 5)
            };

            EditorGUILayout.BeginVertical(boxStyle);

            for (int i = 0; i < environmentObjectTypesArray.arraySize; i++)
            {
                var element = environmentObjectTypesArray.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Object Type {i}:", GUILayout.Width(100));

                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, element.isExpanded ? "Collapse" : "Expand");

                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    environmentObjectTypesArray.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();

                if (element.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("name"), new GUIContent("Name"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("terrainTypeID"), new GUIContent("Terrain Type ID"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("spawnAmount"), new GUIContent("Spawn Amount"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("width"), new GUIContent("Width"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("height"), new GUIContent("Height"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("objectPrefab"), new GUIContent("Object Prefab"));
                    EditorGUI.indentLevel--;
                }
            }

            if (GUILayout.Button("Add New Environment Object"))
                environmentObjectTypesArray.arraySize++;

            EditorGUILayout.EndVertical();
        }

        _serializedEnvironmentObjectSet.ApplyModifiedProperties();
        if (GUI.changed) EditorUtility.SetDirty(config);
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