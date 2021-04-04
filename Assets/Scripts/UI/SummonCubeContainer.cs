using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SummonCubeContainer : MonoBehaviour
{
    public List<Cube> canSummonCubes;

    public void SetCanSummonCubeContainer(List<Cube> canSummonCubes)
    {
        this.canSummonCubes = canSummonCubes;
    }
}
