using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(order = 0, fileName = "E_OnXXX", menuName = "New Event")]
public class Event : ScriptableObject
{
    List<EventListener> eListeners = new List<EventListener>();
    List<Action> registeredActions = new List<Action>();
    [SerializeField] public int listenerCount = 0;
    
    public void Register(EventListener l, UnityAction action)
    {
        l.OnNotify.AddListener(action);
        eListeners.Add(l);

        listenerCount = eListeners.Count;
    }

    public void Register(Action action)
    {
        registeredActions.Add(action);
    }

    public void Unregister(EventListener l)
    {
        eListeners.Remove(l);

        listenerCount = eListeners.Count;
    }

    public void Invoke()
    {
        //eListeners.ForEach(l => l.OnNotify.Invoke());
        foreach (var l in eListeners.ToArray())
            l.OnNotify.Invoke();

        foreach(var action in registeredActions)
            action?.Invoke();

        registeredActions.Clear();

        listenerCount = eListeners.Count;
    }
}
