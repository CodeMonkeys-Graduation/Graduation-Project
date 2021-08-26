using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ManagerBehaviour : MonoBehaviour
{
    public static ManagerBehaviour ManagerInstance { get; protected set; }

    [SerializeField] public GameInstance.ManagerType Type;

    static public GameInstance.ManagerType CachedType = GameInstance.ManagerType.NONE;

    public List<SceneMgr.Scene> LifeCycles { get => _lifeCycles; }

    [SerializeField] public List<SceneMgr.Scene> _lifeCycles;

    public abstract ManagerBehaviour GetInstance();

    protected virtual void Awake()
    {
        CachedType = Type;
    }

}
