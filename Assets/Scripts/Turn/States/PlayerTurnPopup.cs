using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PlayerTurnPopup : PlayerTurnAttack
{
    Transform popup;


    public PlayerTurnPopup(TurnMgr owner, Unit unit, 
        Transform popup, Vector2 popupPos, string popupContent, UnityAction yes, UnityAction no) : base(owner, unit)
    {

    }

    public override void Enter() // �˾��� ����, 
    {
        popup = owner.attackPopup;
        //cubeClicked = unit.attackTargetCube; // popupPos

        SetButtons();
        SetUI(cubeClicked.WhoAccupied().name);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        FreeButtons();
        owner.attackPopup.gameObject.SetActive(false);
    }

    private void SetUI(string unitname)
    {
        TextMeshProUGUI text = popup.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = "It is " + unitname + ", r u Attack?";

        popup.localPosition = Input.mousePosition;
        popup.gameObject.SetActive(true);
    }

    private void SetButtons()
    {
        Button attack = popup.Find("Attack").GetComponent<Button>();
        Button cancel = popup.Find("Cancel").GetComponent<Button>();

        attack.onClick.AddListener(() => OnClickCubeCanAttack());
        cancel.onClick.AddListener(() => owner.stateMachine.ChangeState(new PlayerTurnAttack(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev));
    }

    private void FreeButtons()
    {
        Button attack = popup.Find("Attack").GetComponent<Button>();
        Button cancel = popup.Find("Cancel").GetComponent<Button>();

        attack.onClick.RemoveAllListeners();
        cancel.onClick.RemoveAllListeners();
    }

    private void OnClickCubeCanAttack()
    {
        TurnState nextState = new PlayerTurnBegin(owner, unit);
        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, owner.e_onUnitAttackExit, nextState),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        unit.StopBlink();

        List<Cube> cubesToAttack = owner.mapMgr.GetCubes(
            unit.basicAttackSplash.range,
            unit.basicAttackSplash.centerX,
            unit.basicAttackSplash.centerX,
            cubeClicked);

        unit.Attack(cubesToAttack, cubeClicked);
    }

}
