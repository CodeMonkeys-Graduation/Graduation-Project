using System.Linq;

public class AITurnPlan : TurnState
{
    public AITurnPlan(TurnMgr owner, Unit unit) : base(owner, unit)
    {
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

        ActionPlanner.Instance.Plan(
            unit, 
            actions => owner.stateMachine.ChangeState(new AITurnAction(owner, unit, actions), StateMachine<TurnMgr>.StateTransitionMethod.JustPush));
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        unit.StopBlink();
    }
}
