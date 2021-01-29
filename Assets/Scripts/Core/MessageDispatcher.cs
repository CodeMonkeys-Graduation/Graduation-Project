using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher : MonoBehaviour
{
    public enum MessageType
    {
        UnitRunExit, OnMoveBtnClick
    }

    List<(MonoBehaviour Listener, Action OnReceive, MessageDispatcher.MessageType Message)> registry = 
        new List<(MonoBehaviour, Action, MessageDispatcher.MessageType)>();
    [SerializeField] public int registryCount;
    private void Update()
    {
        registryCount = registry.Count;
    }

    public void Register(MonoBehaviour listener, Action action, MessageType message)
    {
        registry.Add((listener, action, message));
    }

    public void Unregister(MonoBehaviour listener, MessageType message)
    {
        foreach(var register in registry)
        {
            if (register.Listener == listener && register.Message == message)
            {
                registry.Remove(register);
                return;
            }
                
        }
    }

    public void Unregister(MonoBehaviour listener)
    {
        foreach (var register in registry)
        {
            if (register.Listener == listener)
            {
                registry.Remove(register);
            }

        }
    }

    public void SendMessage(MessageType message)
    {
        var remove = new List<(MonoBehaviour, Action, MessageDispatcher.MessageType)>();
        foreach (var listener in registry)
        {
            if(listener.Message == message)
            {
                listener.OnReceive.Invoke();
                remove.Add(listener);
            }
        }
    }




}
