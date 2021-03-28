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
        cubesAttackRange = MapMgr.Instance.GetCubes(unit.basicAttackRange, unit.GetCube);

        // filter cubes
        cubesCanAttack = cubesAttackRange
            .Where(CubeCanAttackConditions)
            .ToList();
    }

    public override void Enter()
    {
        CameraMove.Instance.SetTarget(unit);

        MapMgr.Instance.BlinkCubes(cubesAttackRange, 0.3f);
        MapMgr.Instance.BlinkCubes(cubesCanAttack, 0.7f);
        unit.StartBlink();

        EventMgr.Instance.onTurnActionEnter.Invoke();
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
                    List<Cube> cubesInAttackSplash = MapMgr.Instance.GetCubes(
                        unit.basicAttackSplash.range, unit.basicAttackSplash.centerX, unit.basicAttackSplash.centerZ, cubeClicked);

                    string popupContent = $"It is {cubeClicked.GetUnit().name} r u Attack?";

                    owner.stateMachine.ChangeState(
                        new PlayerTurnPopup(owner, unit, Input.mousePosition,
                        popupContent, AttackOnClickedCube, OnClickNo, () => cubesInAttackSplash.ForEach(c => c.SetBlink(0.7f)), null, () => MapMgr.Instance.StopBlinkAll()),
                        StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
                }
            }
        }
    }

    public override void Exit()
    {
        unit.StopBlink();
        MapMgr.Instance.StopBlinkAll();
        EventMgr.Instance.onTurnActionExit.Invoke();
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
            new WaitSingleEvent(owner, unit, EventMgr.Instance.onUnitIdleEnter, nextState, 
            (param) => ((UnitStateEvent)param)._owner == unit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        unit.StopBlink();

        List<Cube> cubesToAttack = MapMgr.Instance.GetCubes(unit.basicAttackSplash, cubeClicked);

        AttackCommand attackCommand;
        if(AttackCommand.CreateCommand(unit, cubeClicked, out attackCommand))
        {
            unit.EnqueueCommand(attackCommand);
        }
    }

    private void OnClickNo() => owner.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);


}
