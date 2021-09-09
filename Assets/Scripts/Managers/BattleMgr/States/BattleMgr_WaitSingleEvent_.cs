using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObserverPattern;

public class BattleMgr_WaitSingleEvent_ : BattleMgr_State_
{
    protected EventListener el = new EventListener();
    protected Action _onWaitEnter;
    protected Action _onWaitExecute;
    protected Action _onWaitExit;
    protected ObserverEvent _e;
    protected BattleMgr_State_ _nextState;

    public BattleMgr_WaitSingleEvent_(BattleMgr owner, ObserverEvent e, BattleMgr_State_ nextState,
       Action onWaitEnter = null, Action onWaitExecute = null, Action onWaitExit = null) : base(owner)
    {
        _e = e;
        _nextState = nextState;
        _onWaitEnter = onWaitEnter;
        _onWaitExecute = onWaitExecute;
        _onWaitExit = onWaitExit;
    }

    public override void Enter()
    {
        _e.Register(el, OnEvent);
    }

    public override void Execute()
    {


    }

    public override void Exit()
    {

    }

    protected virtual void OnEvent(EventParam param)
    {
        owner.stateMachine.ChangeState(_nextState, StateMachine<BattleMgr>.StateTransitionMethod.PopNPush);
    }
}
