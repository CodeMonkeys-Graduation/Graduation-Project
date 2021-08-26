using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonBehaviour<T> : ManagerBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance { get; protected set; }

    public override ManagerBehaviour GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            //throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            Instance = (T)this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public static bool IsSingletonExist() => Instance != null;

}