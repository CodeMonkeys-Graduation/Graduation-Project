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
        APGameState gameState = new APGameState(requester, turnMgr.turns.ToList(), mapMgr.map.Cubes.ToList());
        List<APActionNode> leafNodes = new List<APActionNode>();

        Queue<APActionNode> queue = new Queue<APActionNode>();

        queue.Enqueue(new RootNode(gameState));

        while(queue.Count > 0)
        {
            APActionNode currPlannigNode = queue.Dequeue();
            int childCount = 0;

            //************** MOVE NODES **************// 
            {
                if (requester.GetActionSlot(ActionType.Move) != null &&
                    currPlannigNode.GetType() == typeof(RootNode) ||
                    currPlannigNode.GetType() != typeof(ActionNode_Move))
                {
                    List<ActionNode_Move> moveNodes = new List<ActionNode_Move>();
                    bool simulCompleted = false;

                    // 시뮬레이션이 끝나면 호출할 콜백함수
                    Action<List<APActionNode>> OnSimulationCompleted = (nodes) =>
                    {
                        if(nodes != null)
                            nodes.ForEach(n => { moveNodes.Add(n as ActionNode_Move); });
                        simulCompleted = true;
                    };

                    // 시뮬레이션 시작
                    MovePlanner movePlanner = new MovePlanner(currPlannigNode._gameState, e_onUnitMoveExit, pathfinder, actionPointPanel);
                    movePlanner.Simulate(this, OnSimulationCompleted);

                    // 시뮬레이션이 끝날때까지 대기
                    while (!simulCompleted) yield return null;

                    // 부모노드 세팅 및 인큐
                    foreach (var node in moveNodes)
                    {
                        node._parent = currPlannigNode;
                        childCount++;
                        queue.Enqueue(node);
                    }
                }
            }





            //************** ATTACK NODES **************// 
            {
                if(requester.GetActionSlot(ActionType.Attack) != null)
                {
                    List<ActionNode_Attack> attackNodes = new List<ActionNode_Attack>();
                    bool simulCompleted = false;

                    // 시뮬레이션이 끝나면 호출할 콜백함수
                    Action<List<APActionNode>> OnSimulationCompleted = (nodes) =>
                    {
                        if (nodes != null)
                            nodes.ForEach(n => { attackNodes.Add(n as ActionNode_Attack); });
                        simulCompleted = true;
                    };

                    // 시뮬레이션 시작
                    AttackPlanner attackPlanner = new AttackPlanner(currPlannigNode._gameState, e_onUnitAttackExit, mapMgr);
                    attackPlanner.Simulate(this, OnSimulationCompleted);

                    // 시뮬레이션이 끝날때까지 대기
                    while (!simulCompleted) yield return null;

                    // 부모노드 세팅 및 인큐
                    foreach (var node in attackNodes)
                    {
                        node._parent = currPlannigNode;
                        childCount++;
                        queue.Enqueue(node);
                    }

                }
            }



            //************** ITEM NODES **************// 
            {
                if (requester.GetActionSlot(ActionType.Item) != null
                    // currPlannigNode의 gameState의 self가 아이템을 가지고 있는지 // 아이템 자료구조 추가
                    )
                {
                    
                }
            }





            //************** SKILL NODES **************// 
            {
                if (requester.GetActionSlot(ActionType.Skill) != null)
                {
                    
                }
            }





            // Leaf Check
            if (childCount == 0)
            {
                leafNodes.Add(currPlannigNode);
            }

        }


        // Construct Best Action List
        int bestScore = leafNodes.Aggregate((acc, curr) => curr._score > acc._score ? curr : acc)._score;
        List<APActionNode> bestLeaves = leafNodes.FindAll(ln => ln._score == bestScore);
        int randomIdx = UnityEngine.Random.Range(0, bestLeaves.Count);

        APActionNode bestLeaf = bestLeaves[randomIdx];

        List<APActionNode> bestSequence = new List<APActionNode>();
        APActionNode currNode = bestLeaf;
        while (currNode != null)
        {
            bestSequence.Add(currNode);
            currNode = currNode._parent;
        }

        bestSequence.Reverse();
        OnPlanCompleted(bestSequence);
    }
}
