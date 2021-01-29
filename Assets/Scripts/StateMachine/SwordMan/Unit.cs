using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header ("Reset Before Play")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform body;

    [Header ("Set in Editor")]
    [SerializeField] public int health;
    [SerializeField] public int basicAttackDamage;
    [SerializeField] public int basicAttackRange;
    [SerializeField] public float moveSpeed;
    [SerializeField] public int actionPoints;
    [SerializeField] public int agility;


    [Header ("Set in Runtime")]
    [SerializeField] public Cube cubeOnPosition;
    [HideInInspector] public StateMachine<Unit> stateMachine;

    public virtual void Start()
    {
        SetCubeOnPosition();
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));
    }

    public void MoveTo(Vector3 nextDestination)
    {
        float distanceRemain = Vector3.Distance(nextDestination, owner.transform.position);
        Vector3 dir = (nextDestination - transform.position).normalized;
        Vector3 move = dir * moveSpeed * Time.deltaTime;

        LookDirection(dir);
        transform.Translate(Vector3.ClampMagnitude(move, distanceRemain));
    }

    public void SetCubeOnPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f, LayerMask.GetMask("Cube")))
        {
            cubeOnPosition = hit.transform.GetComponent<Cube>();
        }

    }
    private void LookDirection(Vector3 dir) => body.rotation = Quaternion.LookRotation(dir, Vector3.up);
}
