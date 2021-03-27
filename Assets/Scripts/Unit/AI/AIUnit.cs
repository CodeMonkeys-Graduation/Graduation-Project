using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class AIUnit : MonoBehaviour
{
    public StateMachine<AIUnit> stateMachine;
    public Unit unit;
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        stateMachine = new StateMachine<AIUnit>(new AIUnitIdle(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
