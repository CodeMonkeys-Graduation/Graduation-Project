using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitItemUser : MonoBehaviour
{
    public Unit owner;
    private void Start()
    {
        owner = GetComponent<Unit>();
    }

}
