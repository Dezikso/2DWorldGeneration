using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTerrainSet", menuName = "MapGeneration/TerrainSet")]
public class TerrainSetSO : ScriptableObject
{
    public List<TerrainType> terrainTypes;

}
