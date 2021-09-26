using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
public enum UIType
{
    Action,
    ActionPoint,
    Back,
    End,
    Item,
    Next,
    Play,
    Popup,
    Start,
    Status,
    Summon,
    Turn
}
public class BaseCanvas : MonoBehaviour
{
    public Canvas canvasPrefab;

    [System.Serializable] 
    public class UIDictionary : SerializableDictionaryBase<UIType, UIComponent> { }
    public UIDictionary _prefab_dictionary = new UIDictionary();
    
    protected UIDictionary _dictionary = new UIDictionary();
    protected void Awake()
    {
        foreach (var u in _prefab_dictionary)
        {
            UIComponent ui = Instantiate(u.Value, transform);
            _dictionary.Add(ui._uitype, ui);
        }
    }

    public UIComponent GetUIComponent(UIType ut)
    {
        return _dictionary[ut];
    }

    public void TurnOnUIComponent(UIType ut)
    {
        _dictionary[ut].gameObject.SetActive(true);
    }
    public void TurnOffUIComponent(UIType ut)
    {
        _dictionary[ut].gameObject.SetActive(false);
    }
    public void TurnOnUIs(List<UIType> l_ui)
    {
        foreach (var v in _dictionary)
        {
            if (l_ui.Contains(v.Key))
            {
                v.Value.gameObject.SetActive(true);
            }
        }
    }
    public void TurnOffUIs(List<UIType> l_ui)
    {
        foreach (var v in _dictionary)
        {
            if (l_ui.Contains(v.Key))
            {
                v.Value.gameObject.SetActive(false);
            }
        }
    }
    public void TurnOnAllUI()
    {
        foreach(var v in _dictionary)
        {
            v.Value.gameObject.SetActive(true);
        }
    }
    public void TurnOffAllUI()
    {
        foreach (var v in _dictionary)
        {
            v.Value.gameObject.SetActive(false);
        }
    }
    public void TurnOnCanvas()
    {
        gameObject.SetActive(true);
    }

    public void TurnOffCanvas()
    {
        gameObject.SetActive(false);
    }
}
