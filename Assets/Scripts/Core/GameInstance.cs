using ObserverPattern;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
public class GameInstance : SingletonBehaviour<GameInstance>
{
    [System.Serializable]
    public class ManagerDictionary : SerializableDictionaryBase<ManagerType, ManagerBehaviour> { }

    public enum ManagerType
    {
        NONE,

        ActionPlanner,
        BattleMgr,
        CameraMgr,
        EventMgr,
        MapMgr,
        Pathfinder,
        TurnMgr,
        UIMgr,
        SceneMgr,
    }

    [SerializeField] public ManagerDictionary ManagerPrefabs;

    [SerializeField] public ObserverEvent e_onSceneChanged;

    public EventListener el_onSceneChanged = new EventListener();

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var Mgr in ManagerPrefabs)
            Instantiate(Mgr.Value);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"OnSceneLoaded");
        Debug.Assert(SceneMgr.sceneMap.TryGetValue(scene.name, out _));

        List<ManagerBehaviour> managersToSpawn = new List<ManagerBehaviour>();

        foreach (var mgr in ManagerPrefabs)
        {
            // 이 Manager의 LifeCycle에 해당하는 Scene임.
            if (mgr.Value.LifeCycles.Contains(SceneMgr.sceneMap[scene.name]))
            {
                if (mgr.Value.GetInstance() == null)
                {
                    Instantiate(mgr.Value);
                    managersToSpawn.Add(mgr.Value);
                }
            }
            // 이 Manager의 LifeCycle에 해당하지 않는 Scene임.
            else
            {
                if (mgr.Value.GetInstance() != null)
                {
                    Destroy(mgr.Value.GetInstance().gameObject);
                }
            }
        }

    }


    private IEnumerator SpawnManagersInFrames(List<ManagerBehaviour> managersToSpawn)
    {
        foreach(var mgr in managersToSpawn)
        {
            yield return null;
            Instantiate(mgr);
        }
    }

}
