using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class APPlanner
{
    public APGameState _gameState;
    protected ActionPointPanel _actionPointPanel;
    protected int _prevScore;
    public APPlanner(APGameState gameState, int prevScore, ActionPointPanel actionPointPanel)
    {
        _gameState = gameState.Clone();
        _prevScore = prevScore;
        _actionPointPanel = actionPointPanel;
    }
    public abstract void Simulate(MonoBehaviour coroutineOwner, Action OnSimulationCompleted, out List<APActionNode> actionNodes);
    public abstract bool IsAvailable(APActionNode prevNode);
}

public class MovePlanner : APPlanner
{
    public MovePlanner(APGameState gameState, int prevScore, ActionPointPanel actionPointPanel)
        : base(gameState, prevScore, actionPointPanel)
    {
        _actionPointPanel = actionPointPanel;
    }

    public override bool IsAvailable(APActionNode prevNode)
    {
        if (!_gameState.self.owner.HasAction(ActionType.Attack))
            return false;

        if (prevNode.GetType() == typeof(ActionNode_Move))
            return false;

        if (_gameState.self.actionPoint < _gameState.self.owner.GetActionSlot(ActionType.Move).cost)
            return false;

        return true;
    }

    public override void Simulate(MonoBehaviour coroutineOwner, Action OnSimulationCompleted, out List<APActionNode> moveNodes)
    {
        moveNodes = new List<APActionNode>();
        coroutineOwner.StartCoroutine(Simulate_Coroutine(OnSimulationCompleted, moveNodes));
    }

    private IEnumerator Simulate_Coroutine(Action OnSimulationCompleted, List<APActionNode> moveNodes)
    {
        // pathfind
        bool pathServed = false;
        List<PFPath> paths = new List<PFPath>();
        Pathfinder.Instance.RequestAsync(_gameState, (p) => { paths = p; pathServed = true; });

        // pathfind가 끝날때까지 대기
        while (!pathServed) yield return null;

        // 가능한 곳으로 이동하는 모든 경우의 수는 List로 생성
        foreach (var path in paths)
        {
            // 움직이지 않는 Move Action은 예외처리
            if (path.destination == path.start) continue;

            // path에 따라 이동하고 actionPoint를 소모하는 MoveActionNode
            moveNodes.Add(ActionNode_Move.Create(_gameState, _prevScore, _actionPointPanel, path));

            yield return null;
        }

        OnSimulationCompleted();
    }
}


public class AttackPlanner : APPlanner
{
    public AttackPlanner(APGameState gameState, int prevScore, ActionPointPanel actionPointPanel)
         : base(gameState, prevScore, actionPointPanel)
    {
        _gameState = gameState.Clone();
        _prevScore = prevScore;
        _actionPointPanel = actionPointPanel;
    }

    public override bool IsAvailable(APActionNode prevNode)
    {
        if (!_gameState.self.owner.HasAction(ActionType.Attack))
            return false;

        if (_gameState.self.actionPoint < _gameState.self.owner.GetActionSlot(ActionType.Attack).cost)
            return false;
        else
            return true;
    }

    public override void Simulate(MonoBehaviour coroutineOwner, Action OnSimulationCompleted, out List<APActionNode> attackNodes)
    {
        attackNodes = new List<APActionNode>();
        coroutineOwner.StartCoroutine(Simulate_Coroutine(OnSimulationCompleted, attackNodes));
    }

    private IEnumerator Simulate_Coroutine(Action OnSimulationCompleted, List<APActionNode> attackNodes)
    {
        // 액션포인트가 충분한지부터 체크
        if(_gameState.self.actionPoint < _gameState.self.owner.GetActionSlot(ActionType.Attack).cost)
        {
            OnSimulationCompleted();
            yield break;
        }

        // 현실 큐브로 기본공격 범위를 먼저 Get
        Cube centerCube = _gameState._unitPos[_gameState.self];
        List<Cube> cubesInAttackRange = MapMgr.Instance.GetCubes(
            _gameState.self.owner.basicAttackRange,
            centerCube
            );

        // 공격 가능한 모든 곳으로 공격하는 모든 경우의 수는 List로 생성
        foreach (Cube cube in cubesInAttackRange)
        {
            List<Cube> splashCubes = MapMgr.Instance.GetCubes(_gameState.self.owner.basicAttackSplash, cube);
            List<APUnit> splashUnits = _gameState._unitPos.Where(unitPos => splashCubes.Contains(unitPos.Value)).Select(unitPos => unitPos.Key).ToList();
            // 자기 자신의 위치를 공격하는 것은 예외처리
            if (cube == centerCube) 
                continue;
            // 스플래쉬 범위에 자기 자신이 있는 것도 제외
            else if (splashCubes.Contains(_gameState._unitPos[_gameState.self]))
                continue;

            // cubesInAttackRange안의 적유닛이 하나라도 있는 큐브를 공격하는 AttackActionNode (Splash 포함)
            if(splashUnits.Any(units => _gameState.self.owner.team.enemyTeams.Contains(units.owner.team)))
            {
                attackNodes.Add(ActionNode_Attack.Create(_gameState, _prevScore, _actionPointPanel, cube));
            }


            yield return null;
        }

        OnSimulationCompleted();
    }
}

public class SkillPlanner : APPlanner
{
    public SkillPlanner(APGameState gameState, int prevScore, ActionPointPanel actionPointPanel)
         : base(gameState, prevScore, actionPointPanel)
    {
        _gameState = gameState.Clone();
        _prevScore = prevScore;
        _actionPointPanel = actionPointPanel;
    }

    public override bool IsAvailable(APActionNode prevNode)
    {
        if (!_gameState.self.owner.HasAction(ActionType.Skill))
            return false;

        if (_gameState.self.actionPoint < _gameState.self.owner.GetActionSlot(ActionType.Skill).cost)
            return false;
        else
            return true;
    }

    public override void Simulate(MonoBehaviour coroutineOwner, Action OnSimulationCompleted, out List<APActionNode> skillNodes)
    {
        skillNodes = new List<APActionNode>();
        coroutineOwner.StartCoroutine(Simulate_Coroutine(OnSimulationCompleted, skillNodes));
    }

    private IEnumerator Simulate_Coroutine(Action OnSimulationCompleted, List<APActionNode> skillNodes)
    {
        // 액션포인트가 충분한지부터 체크
        if (_gameState.self.actionPoint < _gameState.self.owner.GetActionSlot(ActionType.Skill).cost)
        {
            OnSimulationCompleted();
            yield break;
        }

        // 현실 큐브로 스킬 범위를 먼저 Get
        Cube centerCube = _gameState._unitPos[_gameState.self];
        List<Cube> cubesInSkillRange = MapMgr.Instance.GetCubes(
            _gameState.self.owner.skill.skillRange,
            centerCube
            );

        // 스킬 가능한 모든 곳으로 캐스팅하는 모든 경우의 수를 순회
        foreach (Cube cube in cubesInSkillRange)
        {
            List<Cube> splashCubes = MapMgr.Instance.GetCubes(_gameState.self.owner.skill.skillSplash, cube);
            List<APUnit> splashUnits = _gameState._unitPos.Where(unitPos => splashCubes.Contains(unitPos.Value)).Select(unitPos => unitPos.Key).ToList();

            // cubesInSkillRange안의 스코어가 음수가 아닌 액션의 경우의 수를 전부 생성
            if (_gameState.self.owner.skill.GetScoreIfTheseUnitsSplashed(_gameState.self.owner.team, splashUnits) > 0)
            {
                skillNodes.Add(ActionNode_Skill.Create(_gameState, _prevScore, _actionPointPanel, cube));
            }

            yield return null;
        }

        OnSimulationCompleted();
    }
}

