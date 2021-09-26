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

    public void Start()
    {
        if (SceneMgr.Instance._currScene.ToString().Contains("Battle"))
        {
            stateMachine = new StateMachine<UIMgr>(new UIBattleState(this, (BattleCanvas)canvasPrefab_Dictionary[CanvasType.Battle]));
        }
        else
        {
            stateMachine = new StateMachine<UIMgr>(new UINormalState(this, (NormalCanvas)canvasPrefab_Dictionary[CanvasType.Normal]));
        }     
    }

    public void Update()
    {
        stateMachine.Run();
    }

}