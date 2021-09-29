using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLoadSceneBtn : UIComponent, ISceneLoadBtn
{
    [SerializeField] SceneMgr.Scene scene; 
    public void LoadScene()
    {
        SceneMgr.Instance.LoadScene(scene);
    }
}
