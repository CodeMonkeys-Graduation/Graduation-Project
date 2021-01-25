using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsuedoCube : MonoBehaviour
{
    [SerializeField] protected List<PsuedoCube> neighbors = new List<PsuedoCube>();
    public List<PsuedoCube> Neighbors { get => neighbors; set => neighbors = value; }

    [SerializeField] public Transform platform;
    public float groundHeight;

    private void Start()
    {
        groundHeight = transform.position.y;
        PsuedoCube[] cubeArr = FindObjectsOfType<PsuedoCube>();
        foreach(var c in cubeArr)
        {
            c.RegisterIfItsNeighbor(this);
        }
    }

    public void RegisterIfItsNeighbor(PsuedoCube registerer)
    {
        Vector2 registererPlanePos = new Vector2(registerer.transform.position.x, registerer.transform.position.z);
        Vector2 myPlanePos = new Vector2(transform.position.x, transform.position.z);

        if (Vector2.Distance(registererPlanePos, myPlanePos) < 1.1f && registerer != this)
        {
            Neighbors.Add(registerer);
        }
    }
}
