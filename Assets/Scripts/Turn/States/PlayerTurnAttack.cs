using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTurnAttack : TurnState
{
    List<Cube> cubesCanAttack;
    List<Cube> cubesAttackRange;
    Transform popup;
    Cube cubeClicked;

    public PlayerTurnAttack(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        // get all cubes in range
        cubesAttackRange = owner.mapMgr.GetCubes(
            unit.basicAttackRange.range,
            unit.basicAttackRange.centerX,
            unit.basicAttackRange.centerZ,
            unit.GetCube
            );

        // filter cubes
        cubesCanAttack = cubesAttackRange
            .Where(CubeCanAttackConditions)
            .ToList();
    }

    public override void Enter()
    {
        SetButtons();

        owner.mapMgr.BlinkCubes(cubesAttackRange,0.3f);
        owner.mapMgr.BlinkCubes(cubesCanAttack, 0.7f);
        unit.StartBlink();
        owner.endTurnBtn.SetActive(true);
        owner.backBtn.SetActive(true);
    }

    public override void Execute()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (RaycastWithCubeMask(out hit))
            {
                cubeClicked = hit.transform.GetComponent<Cube>();
                if (cubesCanAttack.Contains(cubeClicked))
                {
                    SetUI();
                }
            }
        }
    }

    public override void Exit()
    {
        unit.StopBlink();
        owner.mapMgr.StopBlinkAll();

        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
        owner.attackPopup.gameObject.SetActive(false);
    }

    private bool CubeCanAttackConditions(Cube cube)
        => cube != unit.GetCube &&
            cube.GetUnit() != null &&
            unit.team.enemyTeams.Contains(cube.GetUnit().team);

    private void SetUI()
    {
        Transform popup = owner.attackPopup;
        TextMeshProUGUI text = popup.Find("Text").GetComponent<TextMeshProUGUI>();

        popup.localPosition = Input.mousePosition;
        text.text = "It is " + unit.name + ", r u Attack?";

        popup.gameObject.SetActive(true);
    }

    private void SetButtons()
    {
        popup = owner.attackPopup;

        Button attack = popup.Find("Attack").GetComponent<Button>();
        Button cancel = popup.Find("Cancel").GetComponent<Button>();

        attack.onClick.AddListener(() => OnClickCubeCanAttack());
        cancel.onClick.AddListener(() => owner.stateMachine.ChangeState(new PlayerTurnBegin(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush));
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

    private bool RaycastWithCubeMask(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube"));
    }


}
