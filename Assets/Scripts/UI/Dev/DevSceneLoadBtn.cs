using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSceneLoadBtn : MonoBehaviour
{
    [SerializeField] public SceneMgr.Scene SceneToLoad;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedBtn);
    }

    public void OnClickedBtn()
    {
        SceneMgr.Instance.LoadScene(SceneToLoad);
    }
}
