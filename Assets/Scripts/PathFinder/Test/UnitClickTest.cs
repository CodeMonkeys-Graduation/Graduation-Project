using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitClickTest : MonoBehaviour
{
    [Header("Reset Before Test")]
    [SerializeField] public Unit selectedUnit;
    [SerializeField] public CameraMove cameraMove;
    [SerializeField] public MapMgr mapMgr;
    private void Reset()
    {
        mapMgr = FindObjectOfType<MapMgr>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Unit")))
            {
                hit.transform.GetComponent<IClickable>()?.OnClick();
                selectedUnit = hit.transform.GetComponent<Unit>();
                cameraMove.SetTarget(selectedUnit);
                List<Path> paths = selectedUnit.cubeOnPosition.paths.Where((p) => p.path.Count <= selectedUnit.actionPointsRemain).ToList();
                HashSet<Cube> cubesToBlink = new HashSet<Cube>();
                foreach (var p in paths)
                {
                    foreach (var c in p.path)
                    {
                        cubesToBlink.Add(c);
                    }
                }
                mapMgr.BlinkCubes(cubesToBlink.ToList(), 0.5f);
            }

            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube")) && selectedUnit)
            {
                Path path = selectedUnit.cubeOnPosition.paths.Find((p) => p.destination == hit.transform.GetComponent<Cube>());
                selectedUnit.stateMachine.ChangeState(new UnitRun(selectedUnit, path), StateMachine<Unit>.StateChangeMethod.PopNPush);
                selectedUnit = null;
                mapMgr.StopBlinkAll();
            }
        }
    }


}
