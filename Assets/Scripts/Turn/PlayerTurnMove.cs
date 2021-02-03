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
                    unit.MoveTo(hit.transform.GetComponent<Cube>());
                    this.hitCube = hitCube;

                    TurnState nextState = new PlayerTurnBegin(owner, unit);
                    owner.stateMachine.ChangeState(
                        new WaitEvent(owner, unit, owner.e_onUnitRunExit, nextState, OnUnitRunExit, OnWaitEnter), 
                        StateMachine<TurnMgr>.StateChangeMethod.JustPush);
                }
            }
        }
    }

    private void OnUnitRunExit() => hitCube.StopBlink();
    private void OnWaitEnter() => owner.mapMgr.BlinkCube(hitCube, 0.5f);

    public override void Exit()
    {
        unit.StopBlink();
        owner.mapMgr.StopBlinkAll();
    }
}
