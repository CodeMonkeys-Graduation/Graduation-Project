using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTurnBegin : TurnState
{
    public PlayerTurnBegin(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        // 턴의 첫 액션임
        if (owner.stateMachine.StackCount == 1)
        {
            // 유닛이 전 턴에 남긴 행동력이 존재한다면
            int remain;
            if (owner.actionPointRemains.TryGetValue(unit, out remain))
            {
                unit.actionPointsRemain += remain;
                owner.actionPointRemains.Remove(unit);
            }
            else
                remain = 0;

            unit.ResetActionPoint(remain);
        }

        // 액션포인트가 남아있지않음
        if (unit.actionPointsRemain <= 0)
        {
            owner.NextTurn();
            return;
        }

        // 큐브의 경로를 업데이트해야함
        if (unit.GetCube.pathUpdateDirty)
        {
            UpdateCurrentUnitPaths();
            return;
        }

        unit.StartBlink();
        
        EventMgr.Instance.onTurnBeginEnter.Invoke();

        if (owner.cameraMove != null)
            owner.cameraMove.SetTarget(unit);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();
        EventMgr.Instance.onTurnBeginExit.Invoke();
    }


    private void UpdateCurrentUnitPaths()
    {
        owner.stateMachine.ChangeState(
                new WaitSingleEvent(owner, unit, EventMgr.Instance.onPathfindRequesterCountZero, this),
                StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

        unit.GetCube.UpdatePaths(
            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
            (cube) => (cube as Cube).GetUnit() != null && (cube as Cube).GetUnit().Health > 0);
    }


}
