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
    Main,
    Battle,
    Select,
    Dialog,
    Ending,
    SceneLoad
}
public class UIMgr : SingletonBehaviour<UIMgr>
{
    public StateMachine<UIMgr> stateMachine;
    [System.Serializable] public class CanvasDictionary : SerializableDictionaryBase<CanvasType, BaseCanvas> { }
    [SerializeField] CanvasDictionary canvasPrefab_Dictionary = new CanvasDictionary();
    
    SceneLoadCanvas sceneLoadView = null;
    CanvasType currCanvasType;

    public void Start()
    {
        //TurnOffSceneLoadUI();
        ChangeUIState(SceneMgr.Instance._currScene.ToString());
    }

    public void Update()
    {
        stateMachine.Run();
    }
    public void UnsetUIComponentAll()
    {
        switch (currCanvasType)
        {
            case CanvasType.Main:
            case CanvasType.Select:
            case CanvasType.Battle:
                UINormalState normalState = (UINormalState)stateMachine.stateStack.Peek();
                normalState._canvas.TurnOffAllUI();
                break;
        }
    }

    public void SetUIComponent<T>(UIParam uiParam, bool isOn) where T : PanelUIComponent
    {
        switch(currCanvasType)
        {
            case CanvasType.Main:
            case CanvasType.Select:
            case CanvasType.Battle:
                UINormalState normalState = (UINormalState)stateMachine.stateStack.Peek();

                if(isOn) normalState._canvas.GetUIComponent<T>().SetPanel(uiParam);
                else normalState._canvas.GetUIComponent<T>().UnsetPanel();
                break;
        }
    }

    public T GetUIComponent<T>(bool evenInactive = false) where T : PanelUIComponent
    {
        UIState uiState = (UIState)stateMachine.stateStack.Peek();
            
        switch (currCanvasType)
        {
            case CanvasType.Main:
            case CanvasType.Select:
            case CanvasType.Battle:
                UINormalState normalState = (UINormalState)uiState;
                return normalState._canvas.GetUIComponent<T>(evenInactive);
            default:
                return null;
        }
    }

    public static UIType TypeToUITypeConverter(Type t)
    {
        return (UIType)Enum.Parse(typeof(UIType), t.ToString()); // 이렇게 변환해도 되긴 하는데, 이러면 Type명과 enum이 같아야 함
    }

    public void ChangeUIState(string scenename)
    {   
        if (scenename.Contains("Battle"))
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Battle]));
            currCanvasType = CanvasType.Battle;
        }
        else if (scenename.Contains("Main"))
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Main]));
            currCanvasType = CanvasType.Main;
        }
        else if (scenename.Contains("UnitSelection"))
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Select]));
            currCanvasType = CanvasType.Select;
        }
    }
    public void TurnOnSceneLoadUI()
    {
        sceneLoadView = (SceneLoadCanvas)canvasPrefab_Dictionary[CanvasType.SceneLoad];
        sceneLoadView = Instantiate<SceneLoadCanvas>(sceneLoadView);
        DontDestroyOnLoad(sceneLoadView);
        sceneLoadView.TurnOnCanvas();
    }

    public void TurnOffSceneLoadUI()
    {
        sceneLoadView = FindObjectOfType<SceneLoadCanvas>();

        if (sceneLoadView != null) // 현재 여기 안 들어옴
        {
            sceneLoadView.TurnOffCanvas();
        }
    }
}