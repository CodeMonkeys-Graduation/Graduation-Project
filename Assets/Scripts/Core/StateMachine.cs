using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StateMachine<T>
{
    public T owner;

    public enum StateTransitionMethod { PopNPush, JustPush, ReturnToPrev, ClearNPush }

    public Stack<State<T>> stateStack = new Stack<State<T>>();
    public int StackCount { get => stateStack.Count; }
    Type defaultState;

    public StateMachine(State<T> initialState)
    {
        stateStack.Push(initialState);
        initialState.Enter();
        defaultState = initialState.GetType();
    }

    public void Run()
    {
        stateStack.Peek().Execute();
    }


    public bool IsStateType(System.Type type) => stateStack.Peek().GetType() == type;

    public void ChangeState(State<T> nextState, StateTransitionMethod method)
    {
        switch (method)
        {
            case StateTransitionMethod.PopNPush:
                {
                    State<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Pop();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
                
            case StateTransitionMethod.JustPush:
                {
                    State<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }

            case StateTransitionMethod.ReturnToPrev:
                {
                    State<T> currState = stateStack.Peek();
                    currState.Exit();
                    stateStack.Pop();
                    State<T> prevState = stateStack.Peek();
                    prevState.Enter();
                    break;
                }

            case StateTransitionMethod.ClearNPush:
                {
                    State<T> currState = stateStack.Peek();
                    currState.Exit();
                    stateStack.Clear();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
        }

    }

}
