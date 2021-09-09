using ObserverPattern;
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


public class BattleMgr_Positioning_ : BattleMgr_State_
{
    List<Unit> _unitPrefabs;
    List<Cube> _canSummonCubes;

    Cube _prevRaycastedCube = null;
    Unit _selectedUnit;

    public BattleMgr_Positioning_(BattleMgr owner, List<Unit> unitPrefabs, List<Cube> canSummonCubes) : base(owner)
    {
        _unitPrefabs = unitPrefabs;
        _canSummonCubes = canSummonCubes;
    }

    public override void Enter()
    {
        Debug.Log("Positioning State Enter");
        EventMgr.Instance.onGamePositioningEnter.Invoke(new UISummon(_unitPrefabs, true)); // 여기서 SummonUI에 세팅
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

                MapMgr.Instance.BlinkCubes(_canSummonCubes, 0.5f);

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
            MapMgr.Instance.StopBlinkAll();

            _selectedUnit?.GetComponent<Unit>().StopTransparent();

            _selectedUnit = null;
            _prevRaycastedCube = null;
        }
        
    }

    public override void Exit()
    {
        Debug.Log("Positioning State Exit");
        EventMgr.Instance.onGamePositioningExit.Invoke();
    }

    private void SetCubeNUnit(Cube cube)
    {
        if(_selectedUnit != null) _selectedUnit.transform.position = cube.Platform.position;
        _prevRaycastedCube = cube;
    }

}
