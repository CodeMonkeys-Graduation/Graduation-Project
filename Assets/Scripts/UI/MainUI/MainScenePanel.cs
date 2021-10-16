using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainScenePanel : PanelUIComponent, IPointerClickHandler
{

    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.Dialog4);
    }
}
