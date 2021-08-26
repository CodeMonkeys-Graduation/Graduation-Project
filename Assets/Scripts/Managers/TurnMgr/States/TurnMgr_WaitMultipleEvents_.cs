using ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class TurnMgr_WaitMultipleEvents_ : TurnMgr_WaitSingleEvent_
{
    private List<ObserverEvent> _events;
    private List<EventListener> _elList;

    /// <summary>
    /// Event를 기다리는 State입니다.
    /// </summary>
    /// <param name="e">기다릴 Event</param>
    /// <param name="nextState">Event발생시 Transition할 다음 State</param>
    /// <param name="onEvent">state를 바꾸기 전에 호출할 함수가 있다면 추가하세요.</param>
    public TurnMgr_WaitMultipleEvents_(TurnMgr owner, Unit unit, List<ObserverEvent> events,
        TurnMgr_State_ nextState, Action onWaitEnter = null, Action onWaitExecute = null, Action onWaitExit = null)
        : base(owner, unit, null, nextState, null, onWaitEnter, onWaitExecute, onWaitExit)
    {
        _events = events;
    }

    public override void Enter()
    {
        foreach (var e in _events)
        {
            EventListener el = new EventListener();
            e.Register(el, OnEvent);
            _elList.Add(el);
        }

        if (_onWaitEnter != null) _onWaitEnter.Invoke();
    }

    public override void Exit()
    {
        if (_onWaitExit != null) _onWaitExit.Invoke();

        if(_elList.Count > 0)
            foreach (var (e, i) in _events.Select((ev, i) => (ev, i)))
                e.Unregister(_elList[i]);
    }

    protected override void OnEvent(EventParam param)
    {
        _elList.Remove(el); 
        _e.Unregister(el);

        if(_elList.Count == 0)
            owner.stateMachine.ChangeState(_nextState, StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);
    }
}
