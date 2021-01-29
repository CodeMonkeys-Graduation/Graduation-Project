using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(order = 0, fileName = "new event", menuName = "Custom Event")]
public class Event : ScriptableObject
{
    List<EventListener> eListeners = new List<EventListener>();
    [SerializeField] int listenerCount = 0;
    public void Register(EventListener l, UnityAction action)
    {
        l.OnNotify.AddListener(action);
        eListeners.Add(l);

        listenerCount = eListeners.Count;
    }

    public void Register(EventListener l)
    {
        eListeners.Add(l);

        listenerCount = eListeners.Count;
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


        listenerCount = eListeners.Count;
    }
}
