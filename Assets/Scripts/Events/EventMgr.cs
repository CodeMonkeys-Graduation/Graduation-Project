using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : SingletonBehaviour<EventMgr>
{

    [Header("PathFinding Event")]
    [SerializeField] public Event onPathfindRequesterCountZero;
    [SerializeField] public Event onPathUpdatingStart;

    [Header("Unit Event")]
    [SerializeField] public Event onUnitAttackExit;
    [SerializeField] public Event onUnitDeadEnter;
    [SerializeField] public Event onUnitDeadExit;
    [SerializeField] public Event onUnitIdleEnter;
    [SerializeField] public Event onUnitItemExit;
    [SerializeField] public Event onUnitRunEnter;
    [SerializeField] public Event onUnitRunExit;
    [SerializeField] public Event onUnitSkillExit;

    [Header("Turn Event")]
    [SerializeField] public Event onTurnActionEnter; // begin, popup을 제외한 모든 TurnState
    [SerializeField] public Event onTurnActionExit;
    [SerializeField] public Event onTurnBeginEnter; // begin
    [SerializeField] public Event onTurnBeginExit;
    [SerializeField] public Event onTurnItemEnter; // item
    [SerializeField] public Event onTurnItemExit;
    [SerializeField] public Event onTurnMove; // move
    [SerializeField] public Event onTurnNobody; // nobody 
    [SerializeField] public Event onTurnPlan; // AI Plan 생각 중...

    [Header("Game Event")]
    [SerializeField] public Event onGameBattleEnter;
    [SerializeField] public Event onGameBattleExit;
    [SerializeField] public Event onGameInitEnter;
    [SerializeField] public Event onGameInitExit;
    [SerializeField] public Event onGamePositioningEnter;
    [SerializeField] public Event onGamePositioningExit;
    
    
}
