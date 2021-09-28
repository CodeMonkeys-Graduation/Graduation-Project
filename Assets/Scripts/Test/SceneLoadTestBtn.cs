using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadTestBtn : UIComponent
{
    [SerializeField] public string sceneName;
    Button _Btn;
    bool IsSubscribedOnClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        _Btn = GetComponent<Button>();
    }

    public override void SetPanel(UIParam u = null)
    {
        throw new System.NotImplementedException();
    }
    public override void UnsetPanel()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsSubscribedOnClicked)
        {
            if(SceneMgr.Instance != null)
            {
                //_Btn.onClick.AddListener(() => SceneMgr.Instance.LoadSceneBtnTest(sceneName));
                IsSubscribedOnClicked = true;
            }
        }
    }
}
