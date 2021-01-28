using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public StateMachine<Unit> stateMachine;
    [SerializeField] public int health;
    [SerializeField] public int basicAttackDamage;
    [SerializeField] public Animator anim;
    public virtual void Start()
    {
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));
    }
}
