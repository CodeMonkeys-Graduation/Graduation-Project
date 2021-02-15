using UnityEngine;
using UnityEngine.EventSystems;

public class SummonUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Unit unitPrefab;
    Cube prevRaycastedCube = null;
    GameObject currDraggingUnit = null;
    private static Unit selectedUnit; // 소환 UI가 통틀어 가지고 있을 변수

    public void OnBeginDrag(PointerEventData data)
    {
        selectedUnit = unitPrefab;
    }
    
    public void OnDrag(PointerEventData data)
    {
        RaycastHit hitObj;
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, LayerMask.GetMask("Cube")))
        {
            Cube currRaycastedCube = hitObj.transform.GetComponent<Cube>();

            if (currRaycastedCube == prevRaycastedCube) return; // 이전 큐브와 같은 큐브

            if(currRaycastedCube.WhoAccupied() == null) // 비어있는 큐브
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
        selectedUnit = null;
        prevRaycastedCube = null;
        currDraggingUnit = null;
    }

    private bool IsUnitTempPlaced() => prevRaycastedCube != null && currDraggingUnit != null;
    private void SetCubeNUnit(Cube cube)
    {
        // 드래깅하던 유닛이 어딘가 있음
        if (currDraggingUnit != null) currDraggingUnit.transform.position = cube.platform.position;
        // 드래깅하던 유닛이 없음
        else currDraggingUnit = Instantiate(selectedUnit.gameObject, cube.platform.position, Quaternion.identity);

        prevRaycastedCube = cube;
    }
    private void UnsetCubeNUnit()
    {
        if (currDraggingUnit != null) Destroy(currDraggingUnit.gameObject);
        prevRaycastedCube = null;
    }
}
