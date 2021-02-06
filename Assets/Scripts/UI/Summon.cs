using UnityEngine;
using UnityEngine.EventSystems;

public class Summon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Unit unitPrefab;
    [SerializeField] GameObject imagePrefab;
    GameObject g;
    public static Unit selectedUnit; // 소환 UI가 통틀어 가지고 있을 변수

    public void OnBeginDrag(PointerEventData data)
    {
        selectedUnit = unitPrefab;
    }

    Cube prevRaycastedCube;
    GameObject currDraggingUnit;
    public void OnDrag(PointerEventData data)
    {
        if (selectedUnit != null) 
        { 
            RaycastHit hitObj;
            Ray ray = Camera.main.ScreenPointToRay(data.position);

            if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, LayerMask.GetMask("Cube")))
            {
                Cube currRaycastedCube = hitObj.transform.GetComponent<Cube>();

                if (prevRaycastedCube == null && currDraggingUnit == null) // 처음 큐브에 래이캐스트함.
                {
                    Debug.Log("4");
                    SelectCubeToPosition(currRaycastedCube);
                }
                else if (currRaycastedCube.GetUnit() != null) // 현재 레이캐스트 큐브에 유닛이 있음
                {
                    if (currRaycastedCube.GetUnit() == currDraggingUnit) return; // 근데 그 유닛이 드래깅 유닛임

                    Debug.Log("1");
                    DisselectCubeToPosition();
                }
                else if (prevRaycastedCube != null && currDraggingUnit != null && 
                        prevRaycastedCube != currRaycastedCube) // 이전에 레이캐스트한 큐브 != 현재 레이캐스트 큐브
                {
                    Debug.Log("2");
                    currDraggingUnit.transform.position = currRaycastedCube.platform.position;
                    SelectCubeToPosition(currRaycastedCube);
                }
                else if (prevRaycastedCube != null && currDraggingUnit != null &&
                    prevRaycastedCube == currRaycastedCube) // 이전에 레이캐스트한 큐브 == 현재 레이캐스트 큐브
                {
                    // do nothing
                    Debug.Log("3");
                }
            }
            else
            {
                Debug.Log("5");
                DisselectCubeToPosition();
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        
    }

    private void SelectCubeToPosition(Cube hitCube)
    {
        if (currDraggingUnit != null) Destroy(currDraggingUnit);

        prevRaycastedCube = hitCube;
        currDraggingUnit = Instantiate(selectedUnit.gameObject, prevRaycastedCube.platform.position, Quaternion.identity);
    }

    private void DisselectCubeToPosition()
    {
        if (currDraggingUnit != null) Destroy(currDraggingUnit);

        prevRaycastedCube = null;
        currDraggingUnit = null;
    }
}
