using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelected : State<TurnMgr>
{
    List<Cube> cubesCanGo;
    Unit unit;
    public MoveSelected(TurnMgr owner, Unit unit) : base(owner)
    {
        this.unit = unit;
    }
    public override void Enter()
    {
        Debug.Log("MoveSelected Enter");

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
                if(hit.transform.GetComponent<Cube>())
                {
                    unit.MoveTo(hit.transform.GetComponent<Cube>());
                    owner.stateMachine.ChangeState(new PlayerTurnBegin(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.PopNPush);
                }
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("MoveSelected Exit");

        owner.mapMgr.StopBlinkAll();
    }
}
