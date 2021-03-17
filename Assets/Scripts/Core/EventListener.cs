using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomNotify : UnityEvent<object> { }

public class EventListener
{
    public UnityEvent OnNotify = new UnityEvent();
}

public class EventListenerParam
{
    public CustomNotify OnNotify = new CustomNotify();
}

