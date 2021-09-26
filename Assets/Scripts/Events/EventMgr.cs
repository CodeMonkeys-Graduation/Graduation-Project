using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : SingletonBehaviour<EventMgr>
{
    [Header("SceneChanged")]
    [SerializeField] public ObserverEvent OnSceneChanged;

    [Header("PathFinding Event")]
    [SerializeField] public ObserverEvent onPathfindRequesterCountZero;
    [SerializeField] public ObserverEvent onPathUpdatingStart;

    [Header("Positioning Event")]
    [SerializeField] public ObserverEvent onUnitInitEnd;

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

    [Header("UI Event")]
    [SerializeField] public ObserverEvent onUICreated;

    [Header("Turn Event")]
    [SerializeField] public ObserverEvent onTurnActionEnter; // begin, popup을 제외한 모든 TurnState
    [SerializeField] public ObserverEvent onTurnActionExit;
    [SerializeField] public ObserverEvent onTurnBeginEnter;
    [SerializeField] public ObserverEvent onTurnBeginExit;
    [SerializeField] public ObserverEvent onTurnItemEnter; 
    [SerializeField] public ObserverEvent onTurnItemExit;
    [SerializeField] public ObserverEvent onTurnPopupEnter;
    [SerializeField] public ObserverEvent onTurnPopupExit;
    [SerializeField] public ObserverEvent onTurnMove;
    [SerializeField] public ObserverEvent onTurnNobody;
    [SerializeField] public ObserverEvent onTurnPlan; 

    [Header("Game Event")]
    [SerializeField] public ObserverEvent onGameInitEnter;
    [SerializeField] public ObserverEvent onGameInitExit;
    [SerializeField] public ObserverEvent onGamePositioningEnter;
    [SerializeField] public ObserverEvent onGamePositioningExecute;
    [SerializeField] public ObserverEvent onGamePositioningExit;
    [SerializeField] public ObserverEvent onGameBattleEnter;
    [SerializeField] public ObserverEvent onGameBattleExit;


}
