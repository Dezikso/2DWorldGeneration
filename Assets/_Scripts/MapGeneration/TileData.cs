using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileData
{
    public int terrainTypeID;
    public bool isOccupied;

    public TileData(int terrainTypeID, bool isOccupied)
    {
        this.terrainTypeID = terrainTypeID;
        this.isOccupied = isOccupied;
    }
}
