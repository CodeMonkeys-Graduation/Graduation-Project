using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMove : TurnState
{
    List<Cube> cubesCanGo;
    Cube cubeClicked;
    public PlayerTurnMove(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        cubesCanGo = owner.mapMgr.GetCubes(
            unit.CubeOnPosition,
            cube => cube != unit.CubeOnPosition && (cube.GetUnit() == null || cube.GetUnit().health <= 0),
            path => path.path.Count <= unit.actionPointsRemain + 1);
    }

    public override void Enter()
    {
        owner.mapMgr.BlinkCubes(cubesCanGo, 0.5f);
        unit.StartBlink();
        owner.testEndTurnBtn.SetActive(true);
    }

    public override void Execute()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
            {
                Cube cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubeClicked && cubesCanGo.Contains(cubeClicked))
                {
                    this.cubeClicked = cubeClicked;

                    TurnState nextState = new PlayerTurnBegin(owner, unit);
                    List<Event> eList = new List<Event>() { owner.e_onUnitRunExit, owner.e_onPathfindRequesterCountZero };
                    owner.stateMachine.ChangeState(
                        new WaitMultipleEvents(owner, unit, eList, nextState, OnWaitEnter, null, OnWaitExit),
                        StateMachine<TurnMgr>.StateChangeMethod.JustPush);

                    unit.MoveTo(hit.transform.GetComponent<Cube>());

                    owner.mapMgr.UpdateCubesPaths(
                        // Predicate<Cube> shouldUpdate 
                        cube => cube == cubeClicked || (cube.GetUnit() != null && cube.GetUnit() != unit), // hitCube와 unit이 아닌 유닛이있는 큐브는 업데이트 필요
                        // Func<Cube, int> moveRangeGetter
                        cube => cube == cubeClicked ? 
                            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost : 
                            cube.GetUnit().actionPoints / cube.GetUnit().GetActionSlot(ActionType.Move).cost, // destination이면 unit의 AP에, 아니라면 cube위의 유닛의 AP에 Move cost를 나눈 값을 리턴
                        // Unit movingUnit
                        unit,
                        // Cube destination
                        cubeClicked);
                }
            }
        }
    }

    private void OnWaitExit()
    {
        cubeClicked.StopBlink();
    }
    private void OnWaitEnter()
    {
        cubeClicked.SetBlink(0.5f);
        owner.testEndTurnBtn.SetActive(false);
    }

    public override void Exit()
    {
        unit.StopBlink();
        owner.mapMgr.StopBlinkAll();
        owner.testEndTurnBtn.SetActive(false);
    }
}
