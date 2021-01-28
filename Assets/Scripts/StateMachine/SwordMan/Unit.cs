using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public StateMachine<Unit> stateMachine;
    [SerializeField] public int health;
    [SerializeField] public int basicAttackDamage;
    [SerializeField] public int basicAttackRange;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform body;
    [SerializeField] public Cube cubeOnPosition;
    [SerializeField] public float moveSpeed;

    [SerializeField] public int actionPoints;
    [SerializeField] public int agility;

    public virtual void Start()
    {
        SetCubeOnPosition();
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));
    }

    public void SetCubeOnPosition() => cubeOnPosition = FindObjectOfType<MapMgr>().GetNearestCube(transform.position);
    public void LookDirection(Vector3 dir) => body.rotation = Quaternion.LookRotation(dir, Vector3.up);
}
