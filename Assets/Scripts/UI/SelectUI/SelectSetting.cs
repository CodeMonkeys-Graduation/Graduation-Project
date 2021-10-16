using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSetting : PanelUIComponent
{



    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
