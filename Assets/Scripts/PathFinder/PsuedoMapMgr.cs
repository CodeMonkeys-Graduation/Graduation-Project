using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsuedoMapMgr : MonoBehaviour
{
    [SerializeField] List<PsuedoCube> cubes;

    public List<List<PsuedoCube>> cubeSorted;

    public Map map;

    private void Start()
    {
        
    }
}
