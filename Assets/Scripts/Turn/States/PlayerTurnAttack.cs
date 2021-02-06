using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurnAttack : TurnState
{
    List<Cube> cubesCanAttack;
    Cube cubeClicked;

    public PlayerTurnAttack(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        cubesCanAttack = owner.mapMgr.GetCubes(
            unit.CubeOnPosition,
            unit.basicAttackRange);

        cubesCanAttack = cubesCanAttack
            .Where((cube) => 
                cube != unit.CubeOnPosition &&
                cube.GetUnit() != null && 
                unit.team.enemyTeams.Contains(cube.GetUnit().team))
            .ToList();
    }

    public override void Enter()
    {
        owner.mapMgr.BlinkCubes(cubesCanAttack, 0.5f);
        unit.StartBlink();
        owner.testEndTurnBtn.SetActive(true);
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
            {
                Cube cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubeClicked && cubesCanAttack.Contains(cubeClicked))
                {
                    this.cubeClicked = cubeClicked;

                    TurnState nextState = new PlayerTurnBegin(owner, unit);
                    owner.stateMachine.ChangeState(
                        new WaitSingleEvent(owner, unit, owner.e_onUnitAttackExit, nextState),
                        StateMachine<TurnMgr>.StateChangeMethod.JustPush);

                    unit.StopBlink();

                    unit.Attack(cubeClicked);
                }
            }
        }
    }

    public override void Exit()
    {
        unit.StopBlink();
        owner.mapMgr.StopBlinkAll();
    }

}
