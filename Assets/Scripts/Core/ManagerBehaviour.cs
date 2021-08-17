using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBehaviour : MonoBehaviour
{
    public static ManagerBehaviour ManagerInstance { get; protected set; }

    public List<SceneMgr.Scene> LifeCycles { get => _lifeCycles; }

    public List<SceneMgr.Scene> _lifeCycles;

    public abstract ManagerBehaviour GetInstance();

    public void DestroyInstance()
    {
        if (GetInstance())
            Object.Destroy(GetInstance().gameObject);
    }
}
