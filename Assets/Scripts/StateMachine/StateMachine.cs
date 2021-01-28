using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T>
{
    public T owner;

    public enum StateChangeMethod { PopNPush, JustPush, ReturnToPrev }

    public Stack<IState<T>> stateStack;

    public virtual void Run()
    {
        stateStack.Peek().Execute();
    }

    public virtual void ChangeState(IState<T> nextState, StateChangeMethod method)
    {
        switch (method)
        {
            case StateChangeMethod.PopNPush:
                {
                    IState<T> prevState = stateStack.Pop();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
                
            case StateChangeMethod.JustPush:
                {
                    IState<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }

            case StateChangeMethod.ReturnToPrev:
                {
                    IState<T> currState = stateStack.Pop();
                    currState.Exit();
                    IState<T> prevState = stateStack.Peek();
                    prevState.Enter();
                    break;
                }
        }

    }

}
