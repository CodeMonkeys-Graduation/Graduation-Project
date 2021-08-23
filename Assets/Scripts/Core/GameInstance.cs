using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanged : EventParam
{
    public SceneMgr.Scene _scene;

    public SceneChanged(SceneMgr.Scene scene)
    {
        _scene = scene;
    }
}

/// <summary>
/// 게임의 시작부터 끝까지 존재하는 게임오브젝트입니다.
/// Manager들의 Lifecycle을 관리합니다.
/// </summary>
public class GameInstance : MonoBehaviour
{
    [SerializeField] public List<ManagerBehaviour> ManagerPrefabs;

    [SerializeField] public Event e_onSceneChanged;

    public EventListener el_onSceneChanged = new EventListener();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        e_onSceneChanged.Register(el_onSceneChanged, OnSceneChanged);
        OnSceneChanged(new SceneChanged(SceneMgr.Scene.Main));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnSceneChanged(EventParam param)
    {
        SceneChanged sceneParam = param as SceneChanged;
        if (sceneParam == null) 
            return;

        foreach(ManagerBehaviour manager in ManagerPrefabs)
        {
            // 현재 Scene이 Manager의 LifeCycle에 해당함.
            if (manager.LifeCycles.Contains(sceneParam._scene))
            {
                // 이미 해당 Manager가 존재함.
                if (manager.GetInstance() != null)
                    continue;

                else 
                {
                    Object.Instantiate(manager);
                }
            }
            // 현재 Scene이 Manager의 LifeCycle에 해당하지 않음.
            else
            {
                manager.DestroyInstance();
            }
        }
    }
}
