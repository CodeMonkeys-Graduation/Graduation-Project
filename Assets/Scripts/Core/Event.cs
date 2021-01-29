using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "new event", menuName = "Custom Event")]
public class Event : ScriptableObject
{
    List<EventListener> eListeners = new List<EventListener>();

    public void Register(EventListener l)
    {
        eListeners.Add(l);
    }

    public void Unregister(EventListener l)
    {
        eListeners.Remove(l);
    }

    public void Invoke()
    {
        //eListeners.ForEach(l => l.OnNotify.Invoke());
        foreach (var l in eListeners.ToArray())
            l.OnNotify.Invoke();
    }
}
