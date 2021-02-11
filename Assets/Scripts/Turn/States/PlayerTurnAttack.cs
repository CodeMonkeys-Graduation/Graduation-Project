using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurnAttack : TurnState
{
    List<Cube> cubesCanAttack;
    Cube cubeClicked;

    public PlayerTurnAttack(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        // get all cubes in range
        cubesCanAttack = owner.mapMgr.GetCubes(
            unit.basicAttackRange.range,
            unit.basicAttackRange.centerX,
            unit.basicAttackRange.centerZ,
            unit.GetCube
            );

        // filter cubes
        cubesCanAttack = cubesCanAttack
            .Where(CubeCanAttackConditions)
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
            RaycastHit hit;
            if (RaycastWithCubeMask(out hit))
            {
                Cube cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubesCanAttack.Contains(cubeClicked))
                {
                    OnClickCubeCanAttack(cubeClicked);
                }
            }
        }
    }

    public override void Exit()
    {
        unit.StopBlink();
        owner.mapMgr.StopBlinkAll();
    }
    private bool CubeCanAttackConditions(Cube cube)
        => cube != unit.GetCube &&
            cube.GetUnit() != null &&
            unit.team.enemyTeams.Contains(cube.GetUnit().team);

    private void OnClickCubeCanAttack(Cube cubeClicked)
    {
        this.cubeClicked = cubeClicked;

        TurnState nextState = new PlayerTurnBegin(owner, unit);
        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, owner.e_onUnitAttackExit, nextState),
            StateMachine<TurnMgr>.StateChangeMethod.JustPush);

        unit.StopBlink();

        List<Cube> cubesToAttack = owner.mapMgr.GetCubes(
            unit.basicAttackSplash.range, 
            unit.basicAttackSplash.centerX, 
            unit.basicAttackSplash.centerX, 
            cubeClicked);

        unit.Attack(cubesToAttack, cubeClicked);
    }

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }


}
