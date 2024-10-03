using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnvironmentObjectSet", menuName = "Data/MapGeneration/EnvironmentObjectSet")]
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
}