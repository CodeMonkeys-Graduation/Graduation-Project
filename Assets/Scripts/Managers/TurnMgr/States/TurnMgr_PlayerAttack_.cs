using ObserverPattern;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnMgr_PlayerAttack_ : TurnMgr_State_
{
    List<Cube> cubesCanAttack;
    List<Cube> cubesAttackRange;
    private Cube cubeClicked;

    public TurnMgr_PlayerAttack_(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        // get all cubes in range
        cubesAttackRange = MapMgr.Instance.GetCubes(unit.basicAttackRange, unit.GetCube);

        // filter cubes
        cubesCanAttack = cubesAttackRange
            .Where(CubeCanAttackConditions)
            .ToList();

        CameraMgr.Instance.SetTarget(unit, true);

        MapMgr.Instance.BlinkCubes(cubesAttackRange, 0.1f);
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
                    List<Cube> cubesInAttackSplash = MapMgr.Instance.GetCubes(unit.basicAttackSplash, cubeClicked);

                    string popupContent = $"Are you Sure?";

                    owner.stateMachine.ChangeState(
                        new TurnMgr_Popup_(owner, unit, Input.mousePosition,
                        popupContent, AttackOnClickedCube, OnClickNo, () => cubesInAttackSplash.ForEach(c => c.SetBlink(0.7f)), null, () => MapMgr.Instance.StopBlinkAll()),
                        StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
                }

                else
                {
                    AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_NoAccept, AudioMgr.AudioType.UI);
                }
            }

            else if(RaycatWithEnemyUnitMask(out hit))
            {
                cubeClicked = hit.transform.GetComponent<Unit>().GetCube;
                if (cubesCanAttack.Contains(cubeClicked))
                {
                    List<Cube> cubesInAttackSplash = MapMgr.Instance.GetCubes(unit.basicAttackSplash, cubeClicked);

                    string popupContent = $"Are you Sure?";

                    owner.stateMachine.ChangeState(
                        new TurnMgr_Popup_(owner, unit, Input.mousePosition,
                        popupContent, AttackOnClickedCube, OnClickNo, () => cubesInAttackSplash.ForEach(c => c.SetBlink(0.7f)), null, () => MapMgr.Instance.StopBlinkAll()),
                        StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
                }

                else
                {
                    AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_NoAccept, AudioMgr.AudioType.UI);
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
    {
        List<Cube> splashCubes = MapMgr.Instance.GetCubes(unit.basicAttackSplash, cube);
        // 스플래쉬 범위에 살아있는 적군이 하나라도 존재하면 공격 가능한 큐브
        if (splashCubes.Where(cube => cube.GetUnit() != null && cube.GetUnit().currHealth >= 0 && unit.team.enemyTeams.Contains(cube.GetUnit().team)).Count() > 0)
            return true;

        return false;
    }


    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }

    private bool RaycatWithEnemyUnitMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Unit"));
        return hit.transform != null && 
            unit.team.enemyTeams.Contains(hit.transform.GetComponent<Unit>().team);
    }

    private void AttackOnClickedCube()
    {
        TurnMgr_State_ nextState = new TurnMgr_PlayerBegin_(owner, unit);
        owner.stateMachine.ChangeState(
            new TurnMgr_WaitSingleEvent_(owner, unit, EventMgr.Instance.onUnitIdleEnter, nextState, 
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
