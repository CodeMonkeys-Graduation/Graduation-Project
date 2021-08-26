using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMgr_BranchSingleEvent_ : TurnMgr_WaitSingleEvent_
{
    List<KeyValuePair<Predicate<EventParam>, TurnMgr_State_>> _branches;
    Predicate<EventParam> _ignoreCondition;

    /// <summary>
    /// Event를 기다리는 State입니다.
    /// branches List에 들어있는 순서대로 Predicate를 검사하고 True를 반환하는 Key의 Value인 State로 Transition합니다.
    /// </summary>
    /// <param name="e">기다릴 Event</param>
    public TurnMgr_BranchSingleEvent_(
        TurnMgr owner, Unit unit, ObserverEvent e, List<KeyValuePair<Predicate<EventParam>, TurnMgr_State_>> branches, 
        Predicate<EventParam> ignoreCondition = null, Action onWaitEnter = null, Action onWaitExecute = null, Action onWaitExit = null)
        : base(owner, unit, e, null, null, onWaitEnter, onWaitExecute, onWaitExit)
    {
        _branches = branches;
        _ignoreCondition = ignoreCondition;
    }

    protected override void OnEvent(EventParam param)
    {
        if (_ignoreCondition(param))
            return;

        foreach(var pair in _branches)
        {
            Predicate<EventParam> condition = pair.Key;
            TurnMgr_State_ nextState = pair.Value;

            if (condition(param))
            {
                owner.stateMachine.ChangeState(nextState, StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);
                return;
            }
        }
    }
}
