using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ObserverPattern;

public interface IPanel
{
    void SetPanel(EventParam u = null);
    void UnsetPanel();
}

public abstract class UIComponent : MonoBehaviour, IPanel
{
    [SerializeField] public UIType _uitype;
    public abstract void SetPanel(EventParam u = null);
    public abstract void UnsetPanel();
}