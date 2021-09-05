using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ObserverPattern;

public interface Ipanel
{
    void SetPanel(EventParam u = null);
    void UnsetPanel();
}

public abstract class Battle_UI : MonoBehaviour, Ipanel
{
    BattleUIType _battle_ui_type;

    public Battle_UI(BattleUIType uitype)
    {
        _battle_ui_type = uitype;
    }

    public abstract void SetPanel(EventParam u = null);
    public abstract void UnsetPanel();
}

public abstract class Normal_UI : MonoBehaviour, Ipanel
{
    NormalUIType _uitype;

    public Normal_UI(NormalUIType uitype)
    {
        _uitype = uitype;
    }
    public abstract void SetPanel(EventParam u);
    public abstract void UnsetPanel();
}
