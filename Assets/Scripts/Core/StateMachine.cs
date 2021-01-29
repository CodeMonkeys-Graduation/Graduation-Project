using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public T owner;

    public enum StateChangeMethod { PopNPush, JustPush, ReturnToPrev }

    private volatile Stack<State<T>> stateStack = new Stack<State<T>>();
    public StateMachine(State<T> initialState)
    {
        stateStack.Push(initialState);
        initialState.Enter();
    }

    public void Run()
    {
        stateStack.Peek().Execute();
    }

    public bool IsStateType(System.Type type) => stateStack.Peek().GetType() == type;

    public void ChangeState(State<T> nextState, StateChangeMethod method)
    {
        switch (method)
        {
            case StateChangeMethod.PopNPush:
                {
                    State<T> prevState = stateStack.Pop();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
                
            case StateChangeMethod.JustPush:
                {
                    State<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }

            case StateChangeMethod.ReturnToPrev:
                {
                    State<T> currState = stateStack.Pop();
                    currState.Exit();
                    State<T> prevState = stateStack.Peek();
                    prevState.Enter();
                    break;
                }
        }

    }

}
