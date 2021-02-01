using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitEvent : TurnState
{
    UnityEvent onEvent = new UnityEvent();
    EventListener el = new EventListener();
    Event e;
    TurnState nextState;
    /// <summary>
    /// Event를 기다리는 State입니다.
    /// </summary>
    /// <param name="e">기다릴 Event</param>
    /// <param name="nextState">Event발생시 Transition할 다음 State</param>
    /// <param name="onEvent">state를 바꾸기 전에 호출할 함수가 잇다면 추가하세요.</param>
    public WaitEvent(TurnMgr owner, Unit unit, Event e, TurnState nextState, UnityAction onEvent = null) : base(owner, unit) 
    {
        this.e = e;
        this.onEvent.AddListener(onEvent);
        this.nextState = nextState;

        if(onEvent != null)
            el.OnNotify.AddListener(onEvent);

        el.OnNotify.AddListener(OnEvent_TransitionToNextState);
    }

    public override void Enter()
    {
        e.Register(el);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        e.Unregister(el);
    }

    private void OnEvent_TransitionToNextState() => owner.stateMachine.ChangeState(nextState, StateMachine<TurnMgr>.StateChangeMethod.JustPush);

}
