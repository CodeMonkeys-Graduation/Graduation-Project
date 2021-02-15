using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    [SerializeField] LayerMask cubeLayer;
    public Cube start;
    public Cube destination;
    [SerializeField] List<Cube> path;
    [SerializeField] int moveRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, cubeLayer))
            {
                if(hit.collider.GetComponent<Cube>())
                {
                    if(!start)
                    {
                        FindObjectOfType<MapMgr>().StopBlinkAll();
                        start = hit.collider.GetComponent<Cube>();
                        foreach(var path in start.paths.Where((p) => p.path.Count <= moveRange))
                        {
                            PathVisualize(path);
                        }
                    }
                    else
                    {
                        FindObjectOfType<MapMgr>().StopBlinkAll();
                        destination = hit.collider.GetComponent<Cube>();
                        PathVisualize(start.paths.Find((p) => p.destination == destination));

                        start = null;
                        destination = null;
                    }
                }
            }
        }
    }

    private void PathVisualize(PFPath<Cube, Unit> path) 
    {
        foreach (var cube in path.path)
        {
            cube.SetBlink(1f);
        }
    }
}
