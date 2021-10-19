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
        AudioMgr,
        CinematicDialogMgr
    }

    [SerializeField] public ManagerDictionary ManagerPrefabs;

    [SerializeField] public GameDB gameDB;

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

        gameDB.Init();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()    
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        // Coroutine으로 하는 이유는 모든 매니저를 파괴하고 
        // 라이프 사이클인 매니저만 다시 스폰해야 하는데,
        // 이를 한 프레임에 하면 모든 매니저를 파괴하는 것으로 마킹하면 Instaniate이 안먹힌다.
        // 그래서 모든 매니저를 파괴하고 한 프레임을 쉬어줘야하여 코루틴으로 한다.
        StartCoroutine(UpdateAllManagersInTwoFrames(scene));
    }

    private IEnumerator UpdateAllManagersInTwoFrames(Scene scene)
    {
        foreach (var mgr in ManagerPrefabs)
        {
            if (mgr.Value.GetInstance() != null)
            {
                Destroy(mgr.Value.GetInstance().gameObject);
            }
        }

        // 한 프레임을 쉬어줘야 매니저들이 파괴됩니다.
        yield return null;

        foreach (var mgr in ManagerPrefabs)
        {
            // 이 Manager의 LifeCycle에 해당하는 Scene임.
            if (mgr.Value.LifeCycles.Contains(SceneMgr.sceneMap[scene.name]))
            {
                if (mgr.Value.GetInstance() == null)
                {
                    Instantiate(mgr.Value);
                }
            }
        }
        EventMgr.Instance.OnSceneChanged.Invoke(new SceneChanged(SceneMgr.sceneMap[scene.name]));
    }

}
