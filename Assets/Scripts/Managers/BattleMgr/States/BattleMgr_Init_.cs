using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;


public class BattleMgr_Init_ : BattleMgr_State_
{
    List<Unit> _unitPrefabs;
    List<Cube> _canSummonCubes;

    public BattleMgr_Init_(BattleMgr owner, List<Unit> unitPrefabs, List<Cube> canSummonCubes) : base(owner)
    {
        _unitPrefabs = unitPrefabs;
        _canSummonCubes = canSummonCubes;
    }

    public override void Enter()
    {
        Debug.Log("Init State Enter");
        ChangeStateToWaitState();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Init State Exit");

        EventMgr.Instance.onGameInitExit.Invoke();
    }

    private void ChangeStateToWaitState()
    {
        BattleMgr_State_ nextState = new BattleMgr_Positioning_(owner, _unitPrefabs, _canSummonCubes);

        // 여기서 이걸 쓸 수가 없음... statemachine이 아직 생성되기 전임
        owner.stateMachine.ChangeState(
            new BattleMgr_WaitSingleEvent_(owner, EventMgr.Instance.onUICompleted, nextState,
             OnWaitEnter, OnWaitExecute, OnWaitExit),
            StateMachine<BattleMgr>.StateTransitionMethod.JustPush);
    }

    private void OnWaitEnter()
    {

    }

    private void OnWaitExecute()
    {

    }

    private void OnWaitExit()
    {
        EventMgr.Instance.onGameInitEnter.Invoke();
    }

}
