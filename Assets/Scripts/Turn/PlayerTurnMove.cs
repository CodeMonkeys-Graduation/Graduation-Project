using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMove : State<TurnMgr>
{
    Unit unit;
    List<Cube> cubesCanGo;
    Cube hitCube;
    EventListener el_onRunExit = new EventListener();
    public PlayerTurnMove(TurnMgr owner, Unit unit) : base(owner)
    {
        this.unit = unit;
    }
    public override void Enter()
    {
        cubesCanGo = owner.mapMgr.GetCubes(unit.cubeOnPosition, unit.actionPointsRemain);
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
                    owner.e_onUnitRunExit.Register(el_onRunExit, OnUnitRunExit);

                    unit.MoveTo(hit.transform.GetComponent<Cube>());
                    owner.mapMgr.StopBlinkAll();
                    owner.mapMgr.BlinkCube(hitCube, 0.5f);
                    this.hitCube = hitCube;
                }
            }
        }
    }

    private void OnUnitRunExit()
    {
        owner.stateMachine.ChangeState(new PlayerTurnBegin(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
        hitCube.StopBlink();
    }

    public override void Exit()
    {
        owner.e_onUnitRunExit.Unregister(el_onRunExit);
        owner.mapMgr.StopBlinkAll();
    }
}
