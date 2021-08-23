using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadTestBtn : MonoBehaviour
{
    [SerializeField] public string sceneName;
    Button _Btn;
    bool IsSubscribedOnClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        _Btn = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsSubscribedOnClicked)
        {
            if(SceneMgr.Instance != null)
            {
                _Btn.onClick.AddListener(() => SceneMgr.Instance.LoadSceneBtnTest(sceneName));
                IsSubscribedOnClicked = true;
            }
        }
    }
}
