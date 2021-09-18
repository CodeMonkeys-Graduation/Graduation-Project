using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "MapData", fileName = "MapData", order = 0)]
public class MapData : ScriptableObject
{
    [SerializeField] public List<Cube> _positionableCubes;
}
