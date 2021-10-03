using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class BattleMgr : SingletonBehaviour<BattleMgr>
{
    //-- Set in Editor --//
    [SerializeField] PlayerData playerData;

    public static List<Cube> _canSummonCubes;
    public StateMachine<BattleMgr> stateMachine;

    public enum BMState
    {
        Init, Positioning, Battle
    }
    public BMState bmState;

    public void Start()
    {
        _canSummonCubes = new List<Cube>(FindObjectsOfType<Cube>().Where(cube => cube._isPlacable));
        stateMachine = new StateMachine<BattleMgr>(new BattleMgr_WaitSingleEvent_(this, EventMgr.Instance.onUICreated, new BattleMgr_Init_(this)));
    }

    public void Update()
    {
        stateMachine.Run();
    }

    public void NextState()
    {
        if (stateMachine.IsStateType(typeof(BattleMgr_Init_)))
        {
            stateMachine.ChangeState(new BattleMgr_Positioning_(this, playerData.hasUnitList, _canSummonCubes), StateMachine<BattleMgr>.StateTransitionMethod.JustPush);
        }

        else if (stateMachine.IsStateType(typeof(BattleMgr_Positioning_)))
        {
            stateMachine.ChangeState(new BattleMgr_Battle_(this), StateMachine<BattleMgr>.StateTransitionMethod.ClearNPush);
        }

        else
        {
            Debug.Assert(false, "Battle State에서는 더 이상 BattleMgr.NextState를 호출해서는 안됨");
        }
    }

}
