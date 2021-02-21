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

        // BFS Tree Construction
        Queue<APActionNode> queue = new Queue<APActionNode>();
        queue.Enqueue(new RootNode(gameState));

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

        }


        //*** Construct Best Action List ***//
        List<APActionNode> bestSequence = new List<APActionNode>(); ;

        // 가장 높은 스코어 추출
        int bestScore = leafNodes.Aggregate((acc, curr) => curr._score > acc._score ? curr : acc)._score;

        // 가장 높은 스코어의 시퀀스들을 추출
        List<APActionNode> bestLeaves = leafNodes.FindAll(ln => ln._score == bestScore);

        //// 추출한 시퀀스들중 랜덤하게 고르기위한 idx
        int randomIdx = UnityEngine.Random.Range(0, bestLeaves.Count);

        //// 결정한 시퀀스의 leaf노드
        APActionNode bestLeaf = bestLeaves[randomIdx];

        // 시퀀스 생성 perent를 따라올라감
        APActionNode currNode = bestLeaf;
        while (currNode.GetType() != typeof(RootNode)) // 미자막 RootNode는 안넣을겁니다.
        {
            bestSequence.Add(currNode);
            currNode = currNode._parent;
        }

        // leaf - {} - {} - {} - {}
        // 를
        // {} - {} - {} - {} - leaf
        // 순으로 뒤집기
        bestSequence.Reverse();
        OnPlanCompleted(bestSequence);
    }

}
