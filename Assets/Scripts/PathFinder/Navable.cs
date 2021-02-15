using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Navable<T> : MonoBehaviour
{
    public List<Navable<T>> neighbors = new List<Navable<T>>();
    [SerializeField] public Transform platform;
    public abstract T WhoAccupied();
}
