using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class SummonBtn : UIComponent, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] public Unit unitPrefab;
    [SerializeField] public Text unitCount;

    EventListener el_onUnitSummonReady = new EventListener();

    Cube prevRaycastedCube = null;
    GameObject currDraggingUnit = null;

    private static Unit selectedUnit; // 소환 UI가 통틀어 가지고 있을 변수

    public void OnBeginDrag(PointerEventData data)
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI);
        selectedUnit = unitPrefab;
    }
    
    public void OnDrag(PointerEventData data)
    {
        RaycastHit hitObj;
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        List<Cube> canConsumeCubes = BattleMgr._canSummonCubes;
        if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, LayerMask.GetMask("Cube")))
        {
            Cube currRaycastedCube = hitObj.transform.GetComponent<Cube>();

            if (currRaycastedCube == prevRaycastedCube) return; // 이전 큐브와 같은 큐브
            
            if (!currRaycastedCube.IsAccupied() && canConsumeCubes.Find((c) => (c == currRaycastedCube)) != null) // 비어있는 큐브
                SetCubeNUnit(currRaycastedCube);
            else // 유닛이 있는 큐브
                UnsetCubeNUnit();
        }
        else // 마우스포인트가 큐브에 있지않음
        {
            UnsetCubeNUnit();
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        Unit u = currDraggingUnit?.GetComponent<Unit>();

        if (u != null)
        {
            UIMgr.Instance.SetUIComponent<SummonPanel>(new UISummonParam(new List<Unit>() { selectedUnit }, false), true);
            Debug.Log(u + "소환 완료");
            u.StopTransparent();
        }

        selectedUnit = null;
        prevRaycastedCube = null;
        currDraggingUnit = null;
    }
    
    private bool IsUnitTempPlaced() => prevRaycastedCube != null && currDraggingUnit != null;
    private void SetCubeNUnit(Cube cube)
    {
        // 이미 드래깅하던 유닛이 존재
        if (currDraggingUnit != null)
        {
            currDraggingUnit.transform.position = cube.Platform.position;
        }
        // 첫 드래깅
        else
        {
            currDraggingUnit = Instantiate(selectedUnit.gameObject, cube.Platform.position, Quaternion.identity);
            EventMgr.Instance.onUnitInitEnd.Register(el_onUnitSummonReady, (param) => { currDraggingUnit?.GetComponent<Unit>().StartTransparent(); });
        }
        prevRaycastedCube = cube;
    }
    private void UnsetCubeNUnit()
    {
        if (currDraggingUnit != null) Destroy(currDraggingUnit.gameObject);
        prevRaycastedCube = null;
        currDraggingUnit = null;
    }

}
