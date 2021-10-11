using System.Linq;

public class TurnMgr_AIPlan_ : TurnMgr_State_
{
    public TurnMgr_AIPlan_(TurnMgr owner, Unit unit) : base(owner, unit)
    {
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
        CameraMgr.Instance.SetTarget(unit, true);

        UIMgr.Instance.SetUIComponent<PlanningIndicator>(new PlanningIndicatorUIParam(unit), true);

        ActionPlanner.Instance.Plan(
            unit, 
            actions => owner.stateMachine.ChangeState(new TurnMgr_AIAction_(owner, unit, actions), StateMachine<TurnMgr>.StateTransitionMethod.JustPush));
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        UIMgr.Instance.SetUIComponent<PlanningIndicator>(null, false);
        unit.StopBlink();
    }
}
