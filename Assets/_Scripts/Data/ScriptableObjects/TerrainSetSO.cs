using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTerrainSet", menuName = "Data/MapGeneration/TerrainSet")]
    public class TerrainSetSO : ScriptableObject
    {
        public TerrainType[] terrainTypes;
    }

    [System.Serializable]
    public class TerrainType
    {
        public string name;
        public float height;
        public TileBase tile;
    }
}