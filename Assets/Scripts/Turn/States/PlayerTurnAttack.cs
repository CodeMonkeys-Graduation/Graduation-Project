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
                    string popupContent = "It is " + cubeClicked.GetUnit().name + " r u Attack?";

                    List<Cube> cubesInAttackSplash = owner.mapMgr.GetCubes(
                        unit.basicAttackSplash.range, unit.basicAttackSplash.centerX, unit.basicAttackSplash.centerZ, cubeClicked);

                    owner.stateMachine.ChangeState(
                        new PlayerTurnPopup(owner, unit, owner.Popup, Input.mousePosition, 
                        popupContent, AttackOnClickedCube, OnClickNo, () => cubesInAttackSplash.ForEach(c => c.SetBlink(0.7f))),
                        StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
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
        => cube != unit.GetCube && // 자신이 서있는 큐브 제외
            cube.GetUnit() != null && // 아무도 없는 큐브 제외
            unit.team.enemyTeams.Contains(cube.GetUnit().team); // 자신의 적이 서있는 큐브

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }

    private void AttackOnClickedCube()
    {
        TurnState nextState = new PlayerTurnBegin(owner, unit);
        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, owner.e_onUnitAttackExit, nextState),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        unit.StopBlink();

        List<Cube> cubesToAttack = owner.mapMgr.GetCubes(
            unit.basicAttackSplash.range,
            unit.basicAttackSplash.centerX,
            unit.basicAttackSplash.centerX,
            cubeClicked);

        unit.Attack(cubesToAttack, cubeClicked);
    }

    private void OnClickNo() => owner.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);


}
