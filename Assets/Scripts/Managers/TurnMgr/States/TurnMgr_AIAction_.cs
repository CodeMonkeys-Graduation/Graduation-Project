using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnMgr_AIAction_ : TurnMgr_State_
{
    Queue<APActionNode> _actions;
    APActionNode _currAction;
    public TurnMgr_AIAction_(TurnMgr owner, Unit unit, List<APActionNode> actions) : base(owner, unit)
    {
        this._actions = new Queue<APActionNode>(actions);
    }

    public override void Enter()
    {
        // 죽는 중인(UnitDead) 유닛이 존재 => 사라지고 다시 이 State로 돌아오기
        if (owner.units.Any(unit => unit.stateMachine.IsStateType(typeof(Unit_Dead_))))
        {
            owner.stateMachine.ChangeState(
                new TurnMgr_WaitSingleEvent_(owner, unit, EventMgr.Instance.onUnitDeadCountZero, this),
                StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

            return;
        }

        unit.StartBlink();
        CameraMgr.Instance.SetTarget(unit);

        // 이전 액션한 결과, ShouldReplan이라면 AITurnPlan로 돌아가서 Replan
        // 첫 Enter는 _currAction == null
        if (_currAction != null && _currAction.ShouldReplan(owner.turns.ToList(), MapMgr.Instance.map.Cubes.ToList()))
        {
            owner.stateMachine.ChangeState(
            new TurnMgr_AIPlan_(owner, unit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

            return;
        }

        // 액션 고갈
        if (_actions.Count == 0)
        {
            owner.StartCoroutine(NextTurn());
        }
        else
        {
            owner.StartCoroutine(Action());
        }

    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();
    }

    private IEnumerator NextTurn()
    {
        APNodePool.Instance.Reset();
        APGameStatePool.Instance.Reset();

        float sec = UnityEngine.Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(sec);

        owner.NextTurn();
    }

    private IEnumerator Action()
    {
        if (_actions.Count <= 0) yield break;

        float sec = UnityEngine.Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(sec);

        // 다음 액션
        _currAction = _actions.Dequeue();

        // 액션 실행
        unit.EnqueueCommand(_currAction.Command);

        // 커맨드의 성공여부이벤트를 wait
        List<KeyValuePair<Predicate<EventParam>, TurnMgr_State_>> branches 
            = new List<KeyValuePair<Predicate<EventParam>, TurnMgr_State_>>();

        // Command가 성공했을 경우 유닛의 Action의 끝을 Wait
        TurnMgr_State_ waitIdleEnterState = new TurnMgr_WaitSingleEvent_(
            owner, unit, EventMgr.Instance.onUnitIdleEnter, this, WaitUnitIdleEnterPredicate, 
            _currAction.OnWaitEnter, _currAction.OnWaitExecute, _currAction.OnWaitExit);
        var successBranch = new KeyValuePair<Predicate<EventParam>, TurnMgr_State_>((param) => CommandResultPredicate(param), waitIdleEnterState);
        branches.Add(successBranch);
        // Command가 실패했을 경우 Replan으로 State전환
        var failBranch = new KeyValuePair<Predicate<EventParam>, TurnMgr_State_>((param) => !CommandResultPredicate(param), new TurnMgr_AIPlan_(owner, unit));
        branches.Add(failBranch);

        
        // BranchSingleEvent
        owner.stateMachine.ChangeState(
            new TurnMgr_BranchSingleEvent_(owner, unit, EventMgr.Instance.onUnitCommandResult, branches, 
            (param) => ((CommandResultParam)param)._subject != unit),
            StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

    }

    private bool CommandResultPredicate(EventParam param)
    {
        CommandResultParam crParam = (CommandResultParam)param;
        if (crParam._success)
            return true;
        else 
            return false;
    }

    private bool WaitUnitIdleEnterPredicate(EventParam param)
    {
        UnitStateEvent usParam = (UnitStateEvent)param;
        if (usParam._owner == unit)
            return true;
        else
            return false;
    }

}
