using ObserverPattern;
using System.Collections.Generic;
using UnityEngine;

public class TurnMgr_PlayerMove_ : TurnMgr_State_
{
    List<Cube> cubesCanGo;
    Cube cubeClicked;
    public TurnMgr_PlayerMove_(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        cubesCanGo = MapMgr.Instance.GetCubes(
            unit.GetCube,
            cube => cube != unit.GetCube && (cube.GetUnit() == null || cube.GetUnit().currHealth <= 0),
            path => path.path.Count <= unit.actionPointsRemain + 1);
    }

    public override void Enter()
    {
        CameraMgr.Instance.SetTarget(unit, true);

        MapMgr.Instance.BlinkCubes(cubesCanGo, 0.5f);
        unit.StartBlink();

        EventMgr.Instance.onTurnActionEnter.Invoke();
    }

    public override void Execute()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (RaycastWithCubeMask(out hit))
            {
                Cube cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubesCanGo.Contains(cubeClicked))
                {
                    OnClickCubeCanGo(cubeClicked);
                }
                else
                {
                    AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_NoAccept, AudioMgr.AudioType.UI);
                }
            }
        }
    }

    public override void Exit()
    {
        unit.StopBlink();
        MapMgr.Instance.StopBlinkAll();

        EventMgr.Instance.onTurnActionExit.Invoke();
    }


    private void OnClickCubeCanGo(Cube cubeClicked)
    {
        // for wait state
        this.cubeClicked = cubeClicked;

        // chage state to wait state of unitRunExit, pathUpdateEnd Event
        ChangeStateToWaitState();

        // unit move
        PFPath pathToDest = unit.GetCube._paths.Find((p) => p.destination == cubeClicked);

        MoveCommand moveCommand;
        if(MoveCommand.CreateCommand(unit, pathToDest, out moveCommand))
        {
            unit.EnqueueCommand(moveCommand);

            // update paths in the destination cube
            cubeClicked.UpdatePaths(
                unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
                cube => (cube as Cube).GetUnit() != null && (cube as Cube).GetUnit() != unit);
        }
    }

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }

    private void ChangeStateToWaitState()
    {
        TurnMgr_State_ nextState = new TurnMgr_PlayerBegin_(owner, unit);
        owner.stateMachine.ChangeState(
            new TurnMgr_WaitSingleEvent_(owner, unit, EventMgr.Instance.onUnitRunExit, nextState, 
            (param) => ((UnitStateEvent)param)._owner == unit, OnWaitEnter, OnWaitExecute, OnWaitExit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    private void WaitSingleEventParam_OnEvent(EventParam param)
    {
        if (param is UnitStateEvent && ((UnitStateEvent)param)._owner == unit)
            owner.stateMachine.ChangeState(
                new TurnMgr_PlayerBegin_(owner, unit),
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    private void OnWaitEnter()
    {
        cubeClicked.SetBlink(0.5f);
        EventMgr.Instance.onTurnActionExit.Invoke();
        CameraMgr.Instance.SetTarget(unit);
    }

    private void OnWaitExecute()
    {
        EventMgr.Instance.onTurnMove.Invoke();
        UIMgr.Instance.SetUIComponent<ActionPointPanel>(new UIActionPointParam(owner.turns.Peek().actionPointsRemain), true);
    }

    private void OnWaitExit()
    {
        cubeClicked.StopBlink();
        CameraMgr.Instance.UnsetTarget();
    }

}
