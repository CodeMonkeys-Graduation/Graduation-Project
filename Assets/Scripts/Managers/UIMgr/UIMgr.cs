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
            stateMachine = new StateMachine<UIMgr>(new UIBattleState(this, (BattleCanvas)canvasPrefab_Dictionary[CanvasType.Battle]));
            canvasType = CanvasType.Battle;
        }
        else
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, (NormalCanvas)canvasPrefab_Dictionary[CanvasType.Normal]));
            canvasType = CanvasType.Normal;
        }     
    }

    public void Update()
    {
        stateMachine.Run();
    }

    public void SetUI(UIType uitype, bool isOn)
    {
        UIState uiState = (UIState)stateMachine.stateStack.Peek();

        switch(canvasType)
        {
            case CanvasType.Normal:
                UINormalState normalState = (UINormalState)uiState;
                if(isOn) normalState._canvas.TurnOnUIComponent(uitype);
                else normalState._canvas.TurnOffUIComponent(uitype);
                break;
            case CanvasType.Battle:
                UIBattleState battleState = (UIBattleState)uiState;
                if (isOn) battleState._canvas.TurnOnUIComponent(uitype);
                else battleState._canvas.TurnOffUIComponent(uitype);
                break;
        }
    }

    public UIComponent GetUIComponent(UIType ut)
    {
        UIState uiState = (UIState)stateMachine.stateStack.Peek();

        switch (canvasType)
        {
            case CanvasType.Normal:
                UINormalState normalState = (UINormalState)uiState;
                return normalState._canvas.GetUIComponent(ut);

            case CanvasType.Battle:
                UIBattleState battleState = (UIBattleState)uiState;
                return battleState._canvas.GetUIComponent(ut);

            default:
                return null;
        }
    }
}