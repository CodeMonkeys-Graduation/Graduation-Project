using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface EventParam {  }

public class UnitStateEvent : EventParam
{
    public Unit _owner;
    public UnitStateEvent(Unit owner) { _owner = owner; }
}

[CreateAssetMenu(order = 0, fileName = "E_OnXXX", menuName = "New Event")]
public class Event : ScriptableObject
{
    List<EventListener> eListeners = new List<EventListener>();
    [SerializeField] public int listenerCount = 0;
    
    public void Register(EventListener l, UnityAction<EventParam> action)
    {
        l.OnNotify.AddListener(action);
        eListeners.Add(l);

        listenerCount = eListeners.Count;
    }


    public void Unregister(EventListener l)
    {
        eListeners.Remove(l);

        listenerCount = eListeners.Count;

    }

    public void Invoke(EventParam param = null)
    {
        //eListeners.ForEach(l => l.OnNotify.Invoke());
        foreach (var l in eListeners.ToArray())
            l.OnNotify.Invoke(param);

        listenerCount = eListeners.Count;
    }
}
