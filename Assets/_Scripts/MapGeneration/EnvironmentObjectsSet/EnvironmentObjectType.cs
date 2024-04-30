using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnvironmentObjectType
{
    public int id;
    public string name;
    public int terrainTypeID;
    public int density;
    public int width;
    public int height;
    public GameObject objectPrefab;

    public EnvironmentObjectType(int id, string name, int terrainTypeID, int density, int width, int height, GameObject objectPrefab)
    {
        this.id = id;
        this.name = name;
        this.terrainTypeID = terrainTypeID;
        this.density = density;
        this.width = width;
        this.height = height;
        this.objectPrefab = objectPrefab;
    }
}
