using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Summon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Unit selectedUnit; // 소환 UI가 통틀어 가지고 있을 변수
    public Unit unit;
    public GameObject prefab;
    GameObject g;

    public void UnitSelect(Unit unit)
    {
        selectedUnit = unit;
    }
    public void OnBeginDrag(PointerEventData data)
    {
        UnitSelect(unit);

        if (selectedUnit != null)
        {
            g = Instantiate(prefab);
            g.transform.SetParent(this.transform);
            g.transform.localPosition = this.transform.localPosition;
        }
    }
    public void OnDrag(PointerEventData data)
    {
        if (selectedUnit != null) 
        { 
            g.transform.position = data.position; 
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (selectedUnit != null)
        {
            rayCasting(Camera.main.ScreenPointToRay(data.position));
            Destroy(g);
        }
    }
    
    public void rayCasting(Ray ray)
    {
        RaycastHit hitObj;

        if(Physics.Raycast(ray, out hitObj, Mathf.Infinity, LayerMask.GetMask("Cube")))
        {
            if(hitObj.collider != null)
            {
                GameObject unit = Instantiate(selectedUnit.gameObject);
                unit.transform.localPosition = hitObj.transform.localPosition;
            }
        }
    }
}
