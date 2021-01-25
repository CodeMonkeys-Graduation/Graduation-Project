using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] protected List<Cube> neighbors = new List<Cube>();
    public List<Cube> Neighbors { get => neighbors; set => neighbors = value; }
    
    [SerializeField] public Transform platform;
    public float groundHeight;

    private void Start()
    {
        groundHeight = transform.position.y;
    }
}
