using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uitest : MonoBehaviour
{
    [SerializeField] public DevSceneLoadBtn sp;

    public void OnClickUITest()
    {
        sp = UIMgr.Instance.GetUIComponent<DevSceneLoadBtn>();
    }
}
