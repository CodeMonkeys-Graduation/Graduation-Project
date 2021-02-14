//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class UnitClickTest : MonoBehaviour
//{
//    [Header("Reset Before Test")]
//    [SerializeField] public Unit selectedUnit;
//    [SerializeField] public CameraMove cameraMove;
//    [SerializeField] public MapMgr mapMgr;
//    private void Reset()
//    {
//        mapMgr = FindObjectOfType<MapMgr>();
//    }
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            RaycastHit hit;
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Unit")))
//            {
//                selectedUnit = hit.transform.GetComponent<Unit>();
//                cameraMove.SetTarget(selectedUnit);
//                List<PFPath> paths = selectedUnit.GetCube.paths.Where((p) => p.path.Count <= selectedUnit.actionPointsRemain).ToList();
//                HashSet<Cube> cubesToBlink = new HashSet<Cube>();
//                foreach (var p in paths)
//                {
//                    foreach (var c in p.path)
//                    {
//                        cubesToBlink.Add(c);
//                    }
//                }
//                mapMgr.BlinkCubes(cubesToBlink.ToList(), 0.5f);
//            }

//            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube")) && selectedUnit)
//            {
//                PFPath path = selectedUnit.GetCube.paths.Find((p) => p.destination == hit.transform.GetComponent<Cube>());
//                selectedUnit.stateMachine.ChangeState(new UnitMove(selectedUnit, path), StateMachine<Unit>.StateTransitionMethod.PopNPush);
//                selectedUnit = null;
//                mapMgr.StopBlinkAll();
//            }
//        }
//    }


//}
