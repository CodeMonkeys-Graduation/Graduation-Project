using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionPlanner : MonoBehaviour
{
    [SerializeField] MapMgr mapMgr;
    [SerializeField] TurnMgr turnMgr;
    [SerializeField] Pathfinder pathfinder;
    [SerializeField] ActionPointPanel actionPointPanel;

    [SerializeField] public Event e_onUnitMoveExit;
    [SerializeField] public Event e_onUnitAttackExit;
    [SerializeField] public Event e_onUnitItemExit;
    [SerializeField] public Event e_onUnitSkillExit;

    private void Start()
    {
        mapMgr = FindObjectOfType<MapMgr>();
        turnMgr = FindObjectOfType<TurnMgr>();
        pathfinder = FindObjectOfType<Pathfinder>();
        actionPointPanel = FindObjectOfType<ActionPointPanel>();
    }

    public void Plan(Unit requester, Action<List<APActionNode>> OnPlanCompleted)
    {
        StartCoroutine(Plan_Coroutine(requester, OnPlanCompleted));
    }

    public IEnumerator Plan_Coroutine(Unit requester, Action<List<APActionNode>> OnPlanCompleted)
    {
        APGameState initialGameState = APGameState.Create(requester, turnMgr.turns.ToList(), mapMgr.map.Cubes.ToList());

        List<APActionNode> leafNodes = new List<APActionNode>();

        // BFS Tree Construction
        Queue<APActionNode> queue = new Queue<APActionNode>();
        queue.Enqueue(new RootNode(initialGameState));

        while(queue.Count > 0)
        {
            APActionNode parentNode = queue.Dequeue();
            int childCount = 0;

            //************** MOVE NODES **************// 
            MovePlanner movePlanner = new MovePlanner(parentNode._gameState, parentNode._score, actionPointPanel, pathfinder);
            if (movePlanner.IsAvailable(parentNode))
            {
                List<APActionNode> moveNodes;
                bool simulCompleted = false;
                movePlanner.Simulate(this, () => simulCompleted = true, out moveNodes);

                while (!simulCompleted) yield return null;

                // 부모노드 세팅 및 인큐
                foreach (var node in moveNodes)
                {
                    node._parent = parentNode;
                    childCount++;
                    queue.Enqueue(node);
                }
            }

            //************** ATTACK NODES **************// 
            AttackPlanner attackPlanner = new AttackPlanner(parentNode._gameState, parentNode._score, actionPointPanel, mapMgr);
            if (attackPlanner.IsAvailable(parentNode))
            {
                List<APActionNode> attackNodes;
                bool simulCompleted = false;
                attackPlanner.Simulate(this, () => simulCompleted = true, out attackNodes);

                while (!simulCompleted) yield return null;

                // 부모노드 세팅 및 인큐
                foreach (var node in attackNodes)
                {
                    node._parent = parentNode;
                    childCount++;
                    queue.Enqueue(node);
                }
            }


            //************** ITEM NODES **************// 




            //************** SKILL NODES **************// 





            //*** Leaf Check ***// 
            if (childCount == 0)
            {
                leafNodes.Add(parentNode);
            }
            yield return null;
        }

        //*** 마지막 위치에 따른 점수 계산 ***//




        //*** Construct Best Action List ***//
        List<APActionNode> bestSequence = new List<APActionNode>(); ;

        // 가장 높은 스코어 추출
        int bestScore = leafNodes.Aggregate((acc, curr) => curr._score > acc._score ? curr : acc)._score;

        // high score leaf들을 추출
        List<APActionNode> bestLeaves = leafNodes.FindAll(ln => ln._score == bestScore);

        // high score leaf들의 마지막 self 위치에 따른 점수변동
        CalcFinalPositionScore(bestLeaves, initialGameState);

        // 점수 변동한 leaf들로 다시 순위매김
        int secondBestScore = bestLeaves.Aggregate((acc, curr) => curr._score > acc._score ? curr : acc)._score;
        bestLeaves = bestLeaves.FindAll(ln => ln._score == secondBestScore);

        //// 추출한 leaf들중 랜덤하게 고르기위한 idx
        int randomIdx = UnityEngine.Random.Range(0, bestLeaves.Count);

        //// 결정한 시퀀스의 leaf노드
        APActionNode bestLeaf = bestLeaves[randomIdx];

        // 시퀀스 생성 perent를 따라올라감
        APActionNode currNode = bestLeaf;
        while (currNode.GetType() != typeof(RootNode)) // 미자막 RootNode는 안넣을겁니다.
        {
            bestSequence.Add(currNode);
            currNode = currNode._parent;
            yield return null;
        }

        // leaf - {} - {} - {} - {}
        // 를
        // {} - {} - {} - {} - leaf
        // 순으로 뒤집기
        bestSequence.Reverse();
        OnPlanCompleted(bestSequence);
    }



    private void CalcFinalPositionScore(List<APActionNode> leaves, APGameState initialGameState)
    {
        foreach(var leaf in leaves)
        {
            APGameState finalGameState = leaf._gameState;

            // 적 유닛들 중에
            List<APUnit> eUnitCanAttackMe = new List<APUnit>();
            foreach(var unit in finalGameState._units)
            {
                if (unit.isSelf) continue;
                if (!finalGameState.self.owner.team.enemyTeams.Contains(unit.owner.team)) continue;

                Cube unitPos = finalGameState._unitPos[unit];
                Range attackRange = unit.owner.basicAttackRange;
                List<Cube> cubesInRange = mapMgr.GetCubes(
                    attackRange.range,
                    attackRange.centerX,
                    attackRange.centerZ,
                    unitPos);


                // 만약 일반공격 사정거리가 닿는 적이 있다면 우선 모아놓기
                Cube selfPos = finalGameState._unitPos[finalGameState.self];
                if (cubesInRange.Contains(selfPos))
                    eUnitCanAttackMe.Add(unit);
            }

            // 그리고 적의 기본공격 몇방에 죽을수 있는지에 따라 점수 차감
            // 적이 많을수록 조금만 깎기
            foreach(var unit in eUnitCanAttackMe)
            {
                int attackDmg = unit.owner.BasicAttackDamageAvg;
                int howManyHit = Mathf.Max(finalGameState.self.health / attackDmg, 1);
                int scoreToSub = (500 / howManyHit) / eUnitCanAttackMe.Count;

                leaf._score -= scoreToSub;
            }
        }
    }
}
