using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomNotify : UnityEvent<EventParam> { }

public class EventListener
{
    public CustomNotify OnNotify = new CustomNotify();
}

//public class EventListenerParam
//{
//    public CustomNotify OnNotify = new CustomNotify();
//}

