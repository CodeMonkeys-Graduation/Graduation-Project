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

    public void Plan(Unit requester)
    {
        StartCoroutine(Plan_Coroutine(requester));
    }

    public IEnumerator Plan_Coroutine(Unit requester)
    {
        APGameState gameState = new APGameState(requester, turnMgr.turns.ToList(), mapMgr.map.Cubes.ToList());
        List<ActionNode> leafNodes = new List<ActionNode>();

        Queue<ActionNode> queue = new Queue<ActionNode>();

        queue.Enqueue(new RootNode(gameState));

        while(queue.Count > 0)
        {
            ActionNode currNode = queue.Dequeue();
            int childCount = 0;

            //************** MOVE NODES **************// 
            List<ActionNode_Move> moveNodes = new List<ActionNode_Move>();
            bool simulCompleted = false;

            // 시뮬레이션이 끝나면 호출할 콜백함수
            Action<List<ActionNode>> OnSimulationCompleted = (nodes) =>
            {
                nodes.ForEach(n => moveNodes.Add(n as ActionNode_Move));
                simulCompleted = true;
            };

            // 시뮬레이션 시작
            MovePlanner movePlanner = new MovePlanner(gameState);
            movePlanner.Simulate(this, pathfinder, OnSimulationCompleted);

            // 시뮬레이션이 끝날때까지 대기
            while (!simulCompleted) yield return null;

            // 부모노드 세팅
            foreach(var node in moveNodes)
            {
                node._parent = currNode;
                childCount++;
            }



            //************** ATTACK NODES **************// 






            //************** ITEM NODES **************// 






            //************** SKILL NODES **************// 






            // Leaf Check
            if (childCount == 0)
            {
                leafNodes.Add(currNode);
            }

        }

        





    }
}
