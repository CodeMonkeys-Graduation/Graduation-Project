using System;
using UnityEngine.Events;

public class WaitSingleEvent : TurnState
{
    EventListener el = new EventListener();
    Action onWaitEnter; Action onWaitExecute; Action onWaitExit;
    Event e;
    TurnState nextState;
    Predicate<EventParam> paramCondition;


    /// <summary>
    /// Event를 기다리는 State입니다.
    /// </summary>
    /// <param name="e">기다릴 Event</param>
    /// <param name="nextState">이 State 다음 State</param>
    /// <param name="paramCondition">Event발생시 매개변수를 검사하여 nextState로 Tranisition 여부를 결정하는 Predicate</param>
    public WaitSingleEvent(
        TurnMgr owner, Unit unit, Event e, TurnState nextState, 
        Predicate<EventParam> paramCondition = null, Action onWaitEnter = null, 
        Action onWaitExecute = null, Action onWaitExit = null) : base(owner, unit) 
    {
        this.e = e;
        this.nextState = nextState;
        this.paramCondition = paramCondition;

        e.Register(el, OnEvent_TransitionToNextState);

        if (onWaitEnter != null) this.onWaitEnter = onWaitEnter;
        if (onWaitExecute != null) this.onWaitExecute = onWaitExecute;
        if (onWaitExit != null) this.onWaitExit = onWaitExit;
    }


    public override void Enter()
    {
        if (onWaitEnter != null) this.onWaitEnter.Invoke();
    }

    public override void Execute()
    {
        if (onWaitExecute != null) this.onWaitExecute.Invoke();
    }

    public override void Exit()
    {
        if (onWaitExit != null) this.onWaitExit.Invoke();
        e.Unregister(el);
    }

    private void OnEvent_TransitionToNextState(EventParam param)
    {
        if(paramCondition == null || paramCondition(param))
            owner.stateMachine.ChangeState(nextState, StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

    }

}


