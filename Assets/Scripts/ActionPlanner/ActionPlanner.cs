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

    public void Plan(Unit requester, Action<List<ActionNode>> OnPlanCompleted)
    {
        StartCoroutine(Plan_Coroutine(requester, OnPlanCompleted));
    }

    public IEnumerator Plan_Coroutine(Unit requester, Action<List<ActionNode>> OnPlanCompleted)
    {
        APGameState gameState = new APGameState(requester, turnMgr.turns.ToList(), mapMgr.map.Cubes.ToList());
        List<ActionNode> leafNodes = new List<ActionNode>();

        Queue<ActionNode> queue = new Queue<ActionNode>();

        queue.Enqueue(new RootNode(gameState));

        while(queue.Count > 0)
        {
            ActionNode currPlannigNode = queue.Dequeue();
            int childCount = 0;

            //************** MOVE NODES **************// 
            if(currPlannigNode.GetType() == typeof(RootNode) || 
                currPlannigNode.GetType() != typeof(ActionNode_Move))
            {
                List<ActionNode_Move> moveNodes = new List<ActionNode_Move>();
                bool simulCompleted = false;

                // 시뮬레이션이 끝나면 호출할 콜백함수
                Action<List<ActionNode>> OnSimulationCompleted = (nodes) =>
                {
                    nodes.ForEach(n => moveNodes.Add(n as ActionNode_Move));
                    simulCompleted = true;
                };

                // 시뮬레이션 시작
                MovePlanner movePlanner = new MovePlanner(gameState, e_onUnitMoveExit, actionPointPanel);
                movePlanner.Simulate(this, pathfinder, OnSimulationCompleted);

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
            



            //************** ATTACK NODES **************// 






            //************** ITEM NODES **************// 






            //************** SKILL NODES **************// 






            // Leaf Check
            if (childCount == 0)
            {
                leafNodes.Add(currPlannigNode);
            }

        }


        // Construct Best Action List
        ActionNode bestLeaf = leafNodes.Aggregate((acc, curr) => curr._score > acc._score ? curr : acc);
        List<ActionNode> bestSequence = new List<ActionNode>();
        ActionNode currNode = bestLeaf;
        while (currNode != null)
        {
            bestSequence.Add(currNode);
            currNode = currNode._parent;
        }

        OnPlanCompleted(bestSequence);
    }
}
