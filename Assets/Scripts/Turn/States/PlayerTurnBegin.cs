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
            unit.ResetActionPoint();

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

        SetUI();

        if(owner.cameraMove != null)
            owner.cameraMove.SetTarget(unit);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();

        UnsetUI();
    }

    private void SetUI()
    {
        owner.testPlayBtn.SetActive(false);
        owner.endTurnBtn.SetActive(true);
        owner.backBtn.SetActive(false);

        Dictionary<ActionType, UnityAction> btnEvents = new Dictionary<ActionType, UnityAction>();
        btnEvents.Add(ActionType.Move, OnClickMoveBtn);
        btnEvents.Add(ActionType.Attack, OnClickAttackBtn);
        btnEvents.Add(ActionType.Item, OnClickItemBtn);
        btnEvents.Add(ActionType.Skill, OnClickSkillBtn);

        owner.actionPanel.SetPanel(unit.actionSlots, unit.actionPointsRemain, btnEvents);

        owner.actionPointPanel.SetText(unit.actionPointsRemain);
        owner.turnPanel.gameObject.SetActive(true);

        if(owner.turnPanel.ShouldUpdateSlots(owner.turns.ToList()))
            owner.turnPanel.SetSlots(owner.turns.ToList(), owner.cameraMove);
    }

    private void UpdateCurrentUnitPaths()
    {
        owner.stateMachine.ChangeState(
                        new WaitSingleEvent(owner, unit, owner.e_onPathfindRequesterCountZero, this),
                        StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

        unit.GetCube.UpdatePaths(
            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
            (cube) => (cube as Cube).GetUnit() != null && (cube as Cube).GetUnit().Health > 0);
    }

    private void UnsetUI()
    {
        owner.actionPanel.UnsetPanel();
    }

    private void OnClickMoveBtn()
    => owner.stateMachine.ChangeState(new PlayerTurnMove(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickAttackBtn()
        => owner.stateMachine.ChangeState(new PlayerTurnAttack(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickItemBtn()
        => owner.stateMachine.ChangeState(new PlayerTurnItem(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickSkillBtn()
    => owner.stateMachine.ChangeState(new PlayerTurnSkill(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);


}
