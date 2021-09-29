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
    Ending
}
public class UIMgr : SingletonBehaviour<UIMgr>
{
    public StateMachine<UIMgr> stateMachine;
    [System.Serializable] public class CanvasDictionary : SerializableDictionaryBase<CanvasType, BaseCanvas> { }
    [SerializeField] CanvasDictionary canvasPrefab_Dictionary = new CanvasDictionary();

    CanvasType canvasType;

    public void Start()
    {
        if (SceneMgr.Instance._currScene.ToString().Contains("Battle"))
        {
            stateMachine = new StateMachine<UIMgr>(new UIBattleState(this, canvasPrefab_Dictionary[CanvasType.Battle]));
            canvasType = CanvasType.Battle;
        }
        else if(SceneMgr.Instance._currScene.ToString().Contains("Main"))
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Main]));
            canvasType = CanvasType.Main;
        }
        else if (SceneMgr.Instance._currScene.ToString().Contains("UnitSelection"))
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, canvasPrefab_Dictionary[CanvasType.Select]));
            canvasType = CanvasType.Select;
        }
    }

    public void Update()
    {
        stateMachine.Run();
    }

    public void SetUI(UIType uitype, bool isOn)
    {
        switch(canvasType)
        {
            case CanvasType.Main:
            case CanvasType.Select:
                UINormalState normalState = (UINormalState)stateMachine.stateStack.Peek();

                if(isOn) normalState._canvas.TurnOnUIComponent(uitype);
                else normalState._canvas.TurnOffUIComponent(uitype);
                break;
            case CanvasType.Battle:
                UIBattleState battleState = (UIBattleState)stateMachine.stateStack.Peek();

                if (isOn) battleState._canvas.TurnOnUIComponent(uitype);
                else battleState._canvas.TurnOffUIComponent(uitype);
                break;
        }
    }

    public T GetUIComponent<T>(bool evenInactive = false) where T : PanelUIComponent
    {
        UIState uiState = (UIState)stateMachine.stateStack.Peek();
            
        switch (canvasType)
        {
            case CanvasType.Main:
            case CanvasType.Select:
                UINormalState normalState = (UINormalState)uiState;
                return normalState._canvas.GetUIComponent<T>(evenInactive);

            case CanvasType.Battle:
                UIBattleState battleState = (UIBattleState)uiState;
                return battleState._canvas.GetUIComponent<T>(evenInactive);

            default:
                return null;
        }
    }

    public static UIType TypeToUITypeConverter(Type t)
    {
        return (UIType)Enum.Parse(typeof(UIType), t.ToString()); // 이렇게 변환해도 되긴 하는데, 이러면 Type명과 enum이 같아야 함
    }
}