using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMove : TurnState
{
    List<Cube> cubesCanGo;
    Cube hitCube;
    public PlayerTurnMove(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        cubesCanGo = owner.mapMgr.GetCubes(
            unit.cubeOnPosition, 
            cube => cube != unit.cubeOnPosition && cube.GetUnit() == null, 
            path => path.path.Count <= unit.actionPointsRemain + 1);

        owner.mapMgr.BlinkCubes(cubesCanGo, 0.5f);
    }

    public override void Execute()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
            {
                Cube hitCube = hit.transform.GetComponent<Cube>();
                if (hitCube && cubesCanGo.Contains(hitCube))
                {
                    this.hitCube = hitCube;
                    TurnState nextState = new PlayerTurnBegin(owner, unit);
                    List<Event> eList = new List<Event>() { owner.e_onUnitRunExit, owner.e_onPathfindRequesterCountZero };
                    owner.stateMachine.ChangeState(
                        new WaitMultipleEvents(owner, unit, eList, nextState, OnWaitEnter, null, OnWaitExit),
                        StateMachine<TurnMgr>.StateChangeMethod.JustPush);

                    unit.MoveTo(hit.transform.GetComponent<Cube>());
                    owner.mapMgr.UpdateCubesPaths(
                        cube => cube == hitCube || (cube.GetUnit() != null && cube.GetUnit() != unit), // hitCube와 unit이 아닌 유닛이있는 큐브는 업데이트 필요
                        cube => cube == hitCube ? unit.actionPoints : cube.GetUnit().actionPoints, // destination이면 unit의 AP를, 아니라면 cube위의 유닛의 AP 리턴
                        unit, 
                        hitCube);
                }
            }
        }
    }

    private void OnWaitExit() => hitCube.StopBlink();
    private void OnWaitEnter() => hitCube.SetBlink(0.5f);

    public override void Exit()
    {
        unit.StopBlink();
        owner.mapMgr.StopBlinkAll();
    }
}
