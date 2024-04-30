using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnvironmentObjectSet", menuName = "MapGeneration/EnvironmentObjectSet")]
public class EnvironmentObjectSetSO : ScriptableObject
{
    public List<EnvironmentObjectType> environmentObjectTypes;
}
