using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AITurnAction : TurnState
{
    private class UnitKey : Unit, IKey { }
    Queue<APActionNode> _actions;
    APActionNode _currAction;
    public AITurnAction(TurnMgr owner, Unit unit, List<APActionNode> actions) : base(owner, unit)
    {
        this._actions = new Queue<APActionNode>(actions);
    }

    public override void Enter()
    {
        // 죽는 중인(UnitDead) 유닛이 존재 => 사라지고 다시 이 State로 돌아오기
        if (owner.units.Any(unit => unit.stateMachine.IsStateType(typeof(UnitDead))))
        {
            owner.stateMachine.ChangeState(
                new WaitSingleEvent(owner, unit, EventMgr.Instance.onUnitDeadCountZero, this),
                StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

            return;
        }

        unit.StartBlink();
        CameraMove.Instance.SetTarget(unit);

        // 이전 액션한 결과, ShouldReplan이라면 AITurnPlan로 돌아가서 Replan
        // 첫 Enter는 _currAction == null
        if (_currAction != null && _currAction.ShouldReplan(owner.turns.ToList(), MapMgr.Instance.map.Cubes.ToList()))
        {
            owner.stateMachine.ChangeState(
            new AITurnPlan(owner, unit),
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

        yield return new WaitForSeconds(0.5f);

        owner.NextTurn();
    }

    private IEnumerator Action()
    {
        if (_actions.Count <= 0) yield break;

        float sec = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(sec);

        // 다음 액션
        _currAction = _actions.Dequeue();

        // 액션 실행
        if (_currAction.Command.Perform<UnitKey>(unit))
        {
            // 액션이 끝나는 이벤트를 wait
            owner.stateMachine.ChangeState(
                new WaitSingleEvent(owner, unit, EventMgr.Instance.onUnitIdleEnter, this, null,
                _currAction.OnWaitEnter, _currAction.OnWaitExecute, _currAction.OnWaitExit),
                StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);
        }
        // 액션 실행 실패
        else
        {
            // Replan
            owner.stateMachine.ChangeState(
            new AITurnPlan(owner, unit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
        }
    }

}
