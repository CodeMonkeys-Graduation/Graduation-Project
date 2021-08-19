using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : ManagerBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; protected set; }

    public override ManagerBehaviour GetInstance() => Instance;

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            Instance = (T)this;
        }
    }

    public static bool IsSingletonExist() => Instance != null;

}