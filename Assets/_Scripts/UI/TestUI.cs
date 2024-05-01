using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;


    public void GenerateMap()
    {
        mapGenerator.RandomizeSeed();
        mapGenerator.GenerateMap();
    }
}
