using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObserverPattern;

public class DevSceneLoadBtn : Normal_UI
{
    [SerializeField] public SceneMgr.Scene SceneToLoad;

    public DevSceneLoadBtn() : base(NormalUIType.Start)
    {

    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedBtn);
    }

    public override void SetPanel(EventParam param)
    {

    }
    public override void UnsetPanel()
    {

    }
    public void OnClickedBtn()
    {
        SceneMgr.Instance.LoadScene(SceneToLoad);
    }
}
