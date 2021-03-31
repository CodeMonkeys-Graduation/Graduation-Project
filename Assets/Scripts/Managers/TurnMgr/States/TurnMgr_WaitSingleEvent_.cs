using System;
using UnityEngine.Events;

public class TurnMgr_WaitSingleEvent_ : TurnMgr_State_
{
    protected EventListener el = new EventListener();
    protected Action _onWaitEnter;
    protected Action _onWaitExecute;
    protected Action _onWaitExit;
    protected Event _e;
    protected TurnMgr_State_ _nextState;
    private Predicate<EventParam> _paramCondition;


    /// <summary>
    /// Event를 기다리는 State입니다.
    /// </summary>
    /// <param name="e">기다릴 Event</param>
    /// <param name="nextState">이 State 다음 State</param>
    /// <param name="paramCondition">Event발생시 매개변수를 검사하여 nextState로 Tranisition 여부를 결정하는 Predicate</param>
    public TurnMgr_WaitSingleEvent_(
        TurnMgr owner, Unit unit, Event e, TurnMgr_State_ nextState, 
        Predicate<EventParam> paramCondition = null, Action onWaitEnter = null, 
        Action onWaitExecute = null, Action onWaitExit = null) : base(owner, unit) 
    {
        _e = e;
        _nextState = nextState;
        _paramCondition = paramCondition;

        _onWaitEnter = onWaitEnter;
        _onWaitExecute = onWaitExecute;
        _onWaitExit = onWaitExit;
    }


    public override void Enter()
    {
        _e.Register(el, OnEvent);

        if (_onWaitEnter != null) _onWaitEnter.Invoke();
    }

    public override void Execute()
    {
        if (_onWaitExecute != null) _onWaitExecute.Invoke();
    }

    public override void Exit()
    {
        if (_onWaitExit != null) _onWaitExit.Invoke();
        _e.Unregister(el);
    }

    protected virtual void OnEvent(EventParam param)
    {
        if(_paramCondition == null || _paramCondition(param))
            owner.stateMachine.ChangeState(_nextState, StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);
    }

}


