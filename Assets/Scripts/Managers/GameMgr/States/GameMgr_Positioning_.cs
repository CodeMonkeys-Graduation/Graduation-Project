using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParam : EventParam
{
    public Unit _unit;

    public UnitParam(Unit unit)
    {
        _unit = unit;
    }
}


public class GameMgr_Positioning_ : GameMgr_State_
{
    SummonPanel _summonPanel;
    SummonCubeContainer _summonCubeContainer;

    List<Unit> _unitPrefabs;
    List<Cube> _canSummonCubes;

    Cube _prevRaycastedCube = null;
    Unit _selectedUnit;

    EventListener el_onUnitSummonEnd = new EventListener();

    public GameMgr_Positioning_(GameMgr owner, SummonPanel summonPanel, List<Unit> unitPrefabs, List<Cube> canSummonCubes) : base(owner)
    {
        _summonCubeContainer = MonoBehaviour.FindObjectOfType<SummonCubeContainer>();
        _summonPanel = summonPanel;
        _unitPrefabs = unitPrefabs;
        _canSummonCubes = canSummonCubes;
    }

    public override void Enter()
    {
        Debug.Log("유닛을 배치해주세요.");
        EventMgr.Instance.onGamePositioningEnter.Invoke();

        foreach (Unit unit in _unitPrefabs) 
            _summonPanel.SetSummonPanel(unit, true); // summonUI에 unit에 해당하는 버튼 세팅

        _summonCubeContainer.SetCanSummonCubeContainer(_canSummonCubes);

        EventMgr.Instance.onUnitSummonEnd.Register(
            el_onUnitSummonEnd, 
            (param) => 
                {
                    Unit u = ((UnitParam)param)._unit;
                    _summonPanel.SetSummonPanel(u, false);
                }
            );  
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitObj;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, LayerMask.GetMask("Unit")))
            {
                if(hitObj.transform.GetComponent<Unit>().team.controller == Team.Controller.AI) return;

                _selectedUnit = hitObj.transform.GetComponent<Unit>();
                _selectedUnit.StartTransparent();
                _prevRaycastedCube = _selectedUnit.GetCube;
            }
        }

        else if(Input.GetMouseButton(0))
        {
            RaycastHit hitObj;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, LayerMask.GetMask("Cube")))
            {
                Cube currRaycastedCube = hitObj.transform.GetComponent<Cube>();

                if (currRaycastedCube == _prevRaycastedCube) return; // 이전 큐브와 같은 큐브

                if (!currRaycastedCube.IsAccupied() && _canSummonCubes.Find((c) => currRaycastedCube == c) != null) // 비어있는 큐브
                    SetCubeNUnit(currRaycastedCube);
                else // 유닛이 있는 큐브
                    _prevRaycastedCube = null;
            }
            else // 마우스포인트가 큐브에 있지않음
            {
                _prevRaycastedCube = null;
            }
        }

        else if(Input.GetMouseButtonUp(0))
        {
            _selectedUnit?.GetComponent<Unit>().StopTransparent();

            _selectedUnit = null;
            _prevRaycastedCube = null;
        }
        
    }

    public override void Exit()
    {
        EventMgr.Instance.onGamePositioningExit.Invoke();
    }

    private void SetCubeNUnit(Cube cube)
    {
        if(_selectedUnit != null) _selectedUnit.transform.position = cube.Platform.position;
        _prevRaycastedCube = cube;
    }

}
