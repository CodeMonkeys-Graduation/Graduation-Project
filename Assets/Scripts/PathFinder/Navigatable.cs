using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Navigatable<T> : MonoBehaviour
{
    public List<Navigatable<T>> neighbors = new List<Navigatable<T>>();
    [SerializeField] public Transform platform;
    public abstract T WhoAccupied();
}
