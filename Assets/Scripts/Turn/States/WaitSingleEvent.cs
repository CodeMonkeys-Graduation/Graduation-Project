using System;
using UnityEngine.Events;

public class WaitSingleEvent : TurnState
{
    UnityEvent onEvent = new UnityEvent();
    EventListener el = new EventListener();
    Action onWaitEnter; Action onWaitExecute; Action onWaitExit;
    Event e;
    TurnState nextState;
    /// <summary>
    /// Event를 기다리는 State입니다.
    /// </summary>
    /// <param name="e">기다릴 Event</param>
    /// <param name="nextState">Event발생시 Transition할 다음 State</param>
    /// <param name="onEvent">state를 바꾸기 전에 호출할 함수가 잇다면 추가하세요.</param>
    public WaitSingleEvent(TurnMgr owner, Unit unit, Event e, 
        TurnState nextState, UnityAction onEvent = null, 
        Action onWaitEnter = null, Action onWaitExecute = null, Action onWaitExit = null) : base(owner, unit) 
    {
        this.e = e;
        this.onEvent.AddListener(onEvent);
        this.nextState = nextState;

        if(onEvent != null) el.OnNotify.AddListener(onEvent);

        if (onWaitEnter != null) this.onWaitEnter = onWaitEnter;
        if (onWaitExecute != null) this.onWaitExecute = onWaitExecute;
        if (onWaitExit != null) this.onWaitExit = onWaitExit;

        el.OnNotify.AddListener(OnEvent_TransitionToNextState);
    }

    public override void Enter()
    {
        e.Register(el);
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

    private void OnEvent_TransitionToNextState()
    {
        // path를 업데이트중인 큐브가 있으므로 큐브가 업데이트 될때까지 기다려야함.
        if (owner.isAnyCubePathUpdating)
        {
            owner.stateMachine.ChangeState(
                new WaitSingleEvent(owner, unit, owner.e_onPathfindRequesterCountZero, nextState),
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
        }
        else
        {
            owner.stateMachine.ChangeState(nextState, StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);
        }
    }

}


