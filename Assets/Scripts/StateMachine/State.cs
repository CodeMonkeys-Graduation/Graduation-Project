using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public T owner;
    public State(T owner)
    {
        this.owner = owner;
    }
    /// <summary>
    /// State에 Enter할때 한번 호출합니다.
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// owner의 StateMachine에서 프레임마다 호출합니다.
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// State에서 Exit할때 한번 호출합니다.
    /// </summary>
    public abstract void Exit();
}
