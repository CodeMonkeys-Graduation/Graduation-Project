using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;
using ObserverPattern;
using RotaryHeart.Lib.SerializableDictionary;

public enum CanvasType
{
    Normal,
    Battle
}
public class UIMgr : SingletonBehaviour<UIMgr>
{
    public StateMachine<UIMgr> stateMachine;

    [System.Serializable]
    public class CanvasDictionary : SerializableDictionaryBase<CanvasType, Canvas> { }

    [SerializeField] CanvasDictionary canvasPrefab_Dictionary = new CanvasDictionary();
    [SerializeField] List<Battle_UI> battleUIPrefabs = new List<Battle_UI>(); // 배틀에 사용될 UI 프리팹
    [SerializeField] List<Normal_UI> normalUIPrefabs = new List<Normal_UI>(); // 일단 UI 프리팹

    [HideInInspector] public EventListener sceneListener = new EventListener(); // 씬이 변경되면 이벤트 발생
    CanvasType currCanvasType; // 디버깅용

    [SerializeField] public Dictionary<string, List<NormalUIType>> normalUIDictionaryByScene = new Dictionary<string, List<NormalUIType>>(); // 씬마다 들어갈 UI 리스트를 만들어야 함

    public void Start()
    {
        RegisterEvent();
        SetNormalUIDictionary(); // 방법없나?
        stateMachine = new StateMachine<UIMgr>(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Normal], CanvasType.Normal, MakeActiveUIList("Main")));
        currCanvasType = CanvasType.Battle;
    }

    public void Update()
    {
        stateMachine.Run();
    }

    void SetNormalUIDictionary()
    {
        List<NormalUIType> activeUIList = new List<NormalUIType>();
        activeUIList.Add(NormalUIType.Start);
        normalUIDictionaryByScene.Add("Main", activeUIList);
    }

    void RegisterEvent() => EventMgr.Instance.OnSceneChanged.Register(sceneListener, ChangedScene);

    List<Normal_UI> MakeActiveUIList(string scenename)
    {
        List<Normal_UI> activeNormalUIList = new List<Normal_UI>();
        List<NormalUIType> activeNormalUITypeList = normalUIDictionaryByScene?[scenename];

        if(activeNormalUITypeList == null) return null;

        foreach (NormalUIType t in activeNormalUITypeList)
        {
            int idx = (int)t;
            activeNormalUIList.Add(normalUIPrefabs[idx]); // SerializeField와 enum의 순서가 일치해야함
        }

        return activeNormalUIList;
    }

    void ChangedScene(EventParam param)
    {
        SceneChanged e = (SceneChanged)param;

        if (e == null) return;

        string curr_scenename = e._scene.ToString();

        if(curr_scenename.Contains("Battle")) // 배틀씬
        {
            stateMachine.ChangeState(new UIBattleState(this, canvasPrefab_Dictionary[CanvasType.Battle], CanvasType.Battle, battleUIPrefabs, BattleMgr.Instance, TurnMgr.Instance), StateMachine<UIMgr>.StateTransitionMethod.JustPush);
            currCanvasType = CanvasType.Battle;
        }
        else // 그 외
        {
            stateMachine.ChangeState(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Normal], CanvasType.Normal, MakeActiveUIList(curr_scenename)), StateMachine<UIMgr>.StateTransitionMethod.JustPush);
            currCanvasType = CanvasType.Normal;
        }
    }
}