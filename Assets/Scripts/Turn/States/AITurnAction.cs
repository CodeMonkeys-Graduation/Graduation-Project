using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AITurnAction : TurnState
{
    Queue<APActionNode> _actions;
    APActionNode _currAction;
    public AITurnAction(TurnMgr owner, Unit unit, List<APActionNode> actions) : base(owner, unit)
    {
        this._actions = new Queue<APActionNode>(actions);
    }

    public override void Enter()
    {
        unit.StartBlink();
        owner.cameraMove.SetTarget(unit);

        // 이전 액션이 있고 
        // 이전 액션한 결과, ShouldReplan이라면 AITurnPlan로 돌아가서 Replan
        if (_currAction != null && _currAction.ShouldReplan(owner.turns.ToList(), owner.mapMgr.map.Cubes.ToList()))
        {
            owner.stateMachine.ChangeState(
            new AITurnPlan(owner, unit, owner.actionPlanner),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

            return;
        }

        // 다음 액션
        _currAction = _actions.Dequeue();
        if(_currAction == null || _currAction.GetType() == typeof(RootNode))
        {
            owner.NextTurn();
            return;
        }

        // 액션이 끝나는 이벤트를 wait
        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, _currAction.e_onUnitActionExit, this,
            null, _currAction.OnWaitEnter, _currAction.OnWaitExecute, _currAction.OnWaitExit
            ),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        // 액션 실행
        _currAction.Perform();
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();
    }

}
