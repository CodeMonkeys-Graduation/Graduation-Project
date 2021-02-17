using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMove : TurnState
{
    List<Cube> cubesCanGo;
    Cube cubeClicked;
    public PlayerTurnMove(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        cubesCanGo = owner.mapMgr.GetCubes(
            unit.GetCube,
            cube => cube != unit.GetCube && (cube.GetUnit() == null || cube.GetUnit().Health <= 0),
            path => path.path.Count <= unit.actionPointsRemain + 1);
    }

    public override void Enter()
    {
        owner.cameraMove.SetTarget(unit);

        owner.mapMgr.BlinkCubes(cubesCanGo, 0.5f);
        unit.StartBlink();

        owner.endTurnBtn.SetActive(true);
        owner.backBtn.SetActive(true);
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
        owner.mapMgr.StopBlinkAll();

        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
    }


    private void OnClickCubeCanGo(Cube cubeClicked)
    {
        // for wait state
        this.cubeClicked = cubeClicked;

        // chage state to wait state of unitRunExit, pathUpdateEnd Event
        ChangeStateToWaitState();

        // unit move
        PFPath pathToDest = unit.GetCube.paths.Find((p) => p.destination == cubeClicked);
        unit.MoveTo(pathToDest);

        // update paths in the destination cube
        (cubeClicked as Cube).UpdatePaths(
            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
            cube => (cube as Cube).GetUnit() != null && (cube as Cube).GetUnit() != unit);
    }

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }

    private void ChangeStateToWaitState()
    {
        TurnState nextState = new PlayerTurnBegin(owner, unit);
        List<Event> eList = new List<Event>() { owner.e_onUnitRunExit, owner.e_onPathfindRequesterCountZero };
        owner.stateMachine.ChangeState(
            new WaitMultipleEvents(owner, unit, eList, nextState, OnWaitEnter, OnWaitExecute, OnWaitExit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }
    private void OnWaitEnter()
    {
        cubeClicked.SetBlink(0.5f);
        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
    }

    private void OnWaitExecute()
    {
        owner.actionPointPanel.SetText(unit.actionPointsRemain);
    }

    private void OnWaitExit()
    {
        cubeClicked.StopBlink();
    }

}
