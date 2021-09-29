using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TurnMgr_PlayerItemBag_ State에서 ItemBag에서 Item을 클릭하여 선택한 Item의 사용범위를 나타내는 State
/// </summary>
public class TurnMgr_PlayerItemUse_ : TurnMgr_State_
{
    private Item _item;
    private List<Cube> cubesUseRange;
    private List<Cube> cubesCanUse;
    private Cube cubeClicked;

    public TurnMgr_PlayerItemUse_(TurnMgr owner, Unit unit, Item item) : base(owner, unit)
    {
        _item = item;
    }

    public override void Enter()
    {
        // get all cubes in range
        cubesUseRange = MapMgr.Instance.GetCubes(_item.useRange, unit.GetCube);
        cubesCanUse = _item.RangeExtraCondition(cubesUseRange);

        CameraMgr.Instance.SetTarget(unit, true);
        MapMgr.Instance.BlinkCubes(cubesUseRange, 0.1f);
        MapMgr.Instance.BlinkCubes(cubesCanUse, 0.7f);
        unit.StartBlink();
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (RaycastWithCubeMask(out hit))
            {
                cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubesCanUse.Contains(cubeClicked))
                {
                    List<Cube> cubesInUseSplash = MapMgr.Instance.GetCubes(_item.useSplash, cubeClicked);

                    string popupContent = $"Are you Sure?";

                    owner.stateMachine.ChangeState(
                        new TurnMgr_Popup_(owner, unit, Input.mousePosition,
                        popupContent, UseOnClickedCube, OnClickNo, () => cubesInUseSplash.ForEach(c => c.SetBlink(0.7f)), null, () => MapMgr.Instance.StopBlinkAll()),
                        StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
                }

                else
                {
                    AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_NoAccept, AudioMgr.AudioType.UI);
                }
            }

            else if (RaycatWithUnitMask(out hit))
            {
                cubeClicked = hit.transform.GetComponent<Unit>().GetCube;
                if (cubesCanUse.Contains(cubeClicked))
                {
                    List<Cube> cubesInUseSplash = MapMgr.Instance.GetCubes(_item.useSplash, cubeClicked);

                    string popupContent = $"Are you Sure?";

                    owner.stateMachine.ChangeState(
                        new TurnMgr_Popup_(owner, unit, Input.mousePosition,
                        popupContent, UseOnClickedCube, OnClickNo, () => cubesInUseSplash.ForEach(c => c.SetBlink(0.7f)), null, () => MapMgr.Instance.StopBlinkAll()),
                        StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
                }
                else
                {
                    AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_NoAccept, AudioMgr.AudioType.UI);
                }
            }
        }
    }

    public override void Exit()
    {
        unit.StopBlink();
        MapMgr.Instance.StopBlinkAll();
    }

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }

    private bool RaycatWithUnitMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Unit"));
        return hit.transform != null && unit.team.enemyTeams.Contains(hit.transform.GetComponent<Unit>().team);
    }

    private void UseOnClickedCube()
    {
        TurnMgr_State_ nextState = new TurnMgr_PlayerBegin_(owner, unit);
        owner.stateMachine.ChangeState(
            new TurnMgr_WaitSingleEvent_(owner, unit, EventMgr.Instance.onUnitIdleEnter, nextState,
            (param) => ((UnitStateEvent)param)._owner == unit),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        unit.StopBlink();

        List<Cube> cubesToUseSplash = MapMgr.Instance.GetCubes(_item.useSplash, cubeClicked);

        ItemCommand itemCommand;
        if (ItemCommand.CreateCommand(unit, _item, cubesToUseSplash, out itemCommand))
        {
            unit.EnqueueCommand(itemCommand);
        }
    }

    private void OnClickNo() => owner.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);

}