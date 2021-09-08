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

public abstract class Battle_UI : MonoBehaviour, IPanel
{
    BattleUIType _battle_ui_type;

    public Battle_UI(BattleUIType uitype)
    {
        _battle_ui_type = uitype;
    }

    public abstract void SetPanel(EventParam u = null);
    public abstract void UnsetPanel();
}

public abstract class Normal_UI : MonoBehaviour, IPanel
{
    NormalUIType _uitype;

    public Normal_UI(NormalUIType uitype)
    {
        _uitype = uitype;
    }
    public abstract void SetPanel(EventParam u);
    public abstract void UnsetPanel();
}
