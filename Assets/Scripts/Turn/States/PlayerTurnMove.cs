using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMove : TurnState
{
    List<Cube> cubesCanGo;
    Cube cubeClicked;
    public PlayerTurnMove(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        cubesCanGo = MapMgr.Instance.GetCubes(
            unit.GetCube,
            cube => cube != unit.GetCube && (cube.GetUnit() == null || cube.GetUnit().currHealth <= 0),
            path => path.path.Count <= unit.actionPointsRemain + 1);
    }

    public override void Enter()
    {
        CameraMove.Instance.SetTarget(unit);

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
        PFPath pathToDest = unit.GetCube.paths.Find((p) => p.destination == cubeClicked);

        MoveCommand moveCommand;
        if(MoveCommand.CreateCommand(unit, pathToDest, out moveCommand))
        {
            unit.EnqueueCommand(moveCommand);

            // update paths in the destination cube
            (cubeClicked as Cube).UpdatePaths(
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
        TurnState nextState = new PlayerTurnBegin(owner, unit);
        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, EventMgr.Instance.onUnitRunExit, nextState, 
            (param) => ((UnitStateEvent)param)._owner == unit, OnWaitEnter, OnWaitExecute, OnWaitExit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    private void WaitSingleEventParam_OnEvent(EventParam param)
    {
        if (param is UnitStateEvent && ((UnitStateEvent)param)._owner == unit)
            owner.stateMachine.ChangeState(
                new PlayerTurnBegin(owner, unit),
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    private void OnWaitEnter()
    {
        cubeClicked.SetBlink(0.5f);
        EventMgr.Instance.onTurnActionExit.Invoke();
    }

    private void OnWaitExecute()
    {
        EventMgr.Instance.onTurnMove.Invoke();
    }

    private void OnWaitExit()
    {
        cubeClicked.StopBlink();
    }

}
