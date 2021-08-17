using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : MonoBehaviour
{
    public enum Scene
    { 
        Main, 
        Dialog,
        UnitSelection,
        Battle
    }

    [SerializeField] public Event _onSceneChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
