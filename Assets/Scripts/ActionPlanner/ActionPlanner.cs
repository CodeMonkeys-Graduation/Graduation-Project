using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionPlanner : MonoBehaviour
{
    [SerializeField] MapMgr mapMgr;
    [SerializeField] TurnMgr turnMgr;

    public void Plan(Unit requester)
    {
        APGameState gameState = new APGameState(requester, turnMgr.turns.ToList(), mapMgr.map.Cubes.ToList());




    }
}
