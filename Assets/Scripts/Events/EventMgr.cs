using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : SingletonBehaviour<EventMgr>
{

    [Header("PathFinding Event")]
    [SerializeField] public ObserverEvent onPathfindRequesterCountZero;
    [SerializeField] public ObserverEvent onPathUpdatingStart;

    [Header("Positioning Event")]
    [SerializeField] public ObserverEvent onUnitInitEnd;
    [SerializeField] public ObserverEvent onUnitSummonEnd;

    [Header("Unit Event")]
    [SerializeField] public ObserverEvent onUnitCommandResult;
    [SerializeField] public ObserverEvent onUnitAttackExit;
    [SerializeField] public ObserverEvent onUnitDeadEnter;
    [SerializeField] public ObserverEvent onUnitDeadExit;
    [SerializeField] public ObserverEvent onUnitIdleEnter;
    [SerializeField] public ObserverEvent onUnitItemExit;
    [SerializeField] public ObserverEvent onUnitRunEnter;
    [SerializeField] public ObserverEvent onUnitRunExit;
    [SerializeField] public ObserverEvent onUnitSkillExit;
    [SerializeField] public ObserverEvent onUnitDeadCountZero;

    [Header("Turn Event")]
    [SerializeField] public ObserverEvent onTurnActionEnter; // begin, popup을 제외한 모든 TurnState
    [SerializeField] public ObserverEvent onTurnActionExit;
    [SerializeField] public ObserverEvent onTurnBeginEnter;
    [SerializeField] public ObserverEvent onTurnBeginExit;
    [SerializeField] public ObserverEvent onTurnItemEnter; 
    [SerializeField] public ObserverEvent onTurnItemExit;
    [SerializeField] public ObserverEvent onTurnMove;
    [SerializeField] public ObserverEvent onTurnNobody;
    [SerializeField] public ObserverEvent onTurnPlan; 

    [Header("Game Event")]
    [SerializeField] public ObserverEvent onGameInitEnter;
    [SerializeField] public ObserverEvent onGameInitExit;
    [SerializeField] public ObserverEvent onGamePositioningEnter;
    [SerializeField] public ObserverEvent onGamePositioningExit;
    [SerializeField] public ObserverEvent onGameBattleEnter;
    [SerializeField] public ObserverEvent onGameBattleExit;
    [SerializeField] public ObserverEvent onGameVictoryEnter;
    [SerializeField] public ObserverEvent onGameVictoryExit;
    [SerializeField] public ObserverEvent onGameDefeatEnter;
    [SerializeField] public ObserverEvent onGameDefeatExit;


}
