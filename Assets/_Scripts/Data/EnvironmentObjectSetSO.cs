using UnityEngine;

[CreateAssetMenu(fileName = "NewEnvironmentObjectSet", menuName = "MapGeneration/EnvironmentObjectSet")]
public class EnvironmentObjectSetSO : ScriptableObject
{
    public EnvironmentObjectType[] environmentObjectTypes;
}

[System.Serializable]
public class EnvironmentObjectType
{
    public string name;
    public int terrainTypeID;
    public int spawnAmount;
    public int width;
    public int height;
    public GameObject objectPrefab;
}