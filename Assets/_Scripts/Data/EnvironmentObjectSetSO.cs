using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnvironmentObjectSet", menuName = "MapGeneration/EnvironmentObjectSet")]
public class EnvironmentObjectSetSO : ScriptableObject
{
    public List<EnvironmentObjectType> environmentObjectTypes;
}

[System.Serializable]
public class EnvironmentObjectType
{
    public int id;
    public string name;
    public int terrainTypeID;
    public int spawnAmount;
    public int width;
    public int height;
    public GameObject objectPrefab;

    public EnvironmentObjectType(int id, string name, int terrainTypeID, int spawnAmount, int width, int height, GameObject objectPrefab)
    {
        this.id = id;
        this.name = name;
        this.terrainTypeID = terrainTypeID;
        this.spawnAmount = spawnAmount;
        this.width = width;
        this.height = height;
        this.objectPrefab = objectPrefab;
    }
}