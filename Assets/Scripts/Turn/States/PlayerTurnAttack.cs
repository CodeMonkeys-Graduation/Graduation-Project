using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurnAttack : TurnState
{
    List<Cube> cubesCanAttack;
    List<Cube> cubesAttackRange;
    protected Cube cubeClicked;

    public PlayerTurnAttack(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        // get all cubes in range
        cubesAttackRange = owner.mapMgr.GetCubes(
            unit.basicAttackRange.range,
            unit.basicAttackRange.centerX,
            unit.basicAttackRange.centerZ,
            unit.GetCube
            );

        // filter cubes
        cubesCanAttack = cubesAttackRange
            .Where(CubeCanAttackConditions)
            .ToList();
    }

    public override void Enter()
    {
        owner.cameraMove.SetTarget(unit);

        owner.mapMgr.BlinkCubes(cubesAttackRange, 0.3f);
        owner.mapMgr.BlinkCubes(cubesCanAttack, 0.7f);
        unit.StartBlink();
        owner.endTurnBtn.SetActive(true);
        owner.backBtn.SetActive(true);
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (RaycastWithCubeMask(out hit))
            {
                cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubesCanAttack.Contains(cubeClicked))
                {
                    //unit.attackTargetCube = cubeClicked;
                    //owner.stateMachine.ChangeState(new PlayerTurnPopup(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
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

    private bool CubeCanAttackConditions(Cube cube)
        => cube != unit.GetCube &&
            cube.WhoAccupied() != null &&
            unit.team.enemyTeams.Contains(cube.WhoAccupied().team);

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }


}
