﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    [System.Serializable]
    public enum Scene // ToString()으로 검사하기때문에 Scene의 이름과 정확히 일치해야합니다. 
        // 그리고 이미 있는 중간에 새로운 enum을 넣지말고 아래에 이어서 넣습니다. (중간에 넣으면 설정한 것들이 깨져버림...)
    { 
        Main, 
        Dialog1,
        UnitSelection,
        Battle1,
        Battle2,
        Battle3,
        Battle4,
    }

    public static Dictionary<string, Scene> sceneMap = new Dictionary<string, Scene>() {
        { Scene.Main.ToString(), Scene.Main },
        { Scene.Dialog1.ToString(), Scene.Dialog1 },
        { Scene.UnitSelection.ToString(), Scene.UnitSelection },
        { Scene.Battle1.ToString(), Scene.Battle1 },
        { Scene.Battle2.ToString(), Scene.Battle2 },
        { Scene.Battle3.ToString(), Scene.Battle3 },
        { Scene.Battle4.ToString(), Scene.Battle4 },
    };

    [SerializeField/*DEBUG*/] public Scene _currScene;

    private void Awake()
    {
        base.Awake();

        // AudioMgr이 Start에서 _currScene을 접근하기때문에 Awake에서 수행해야함.
        sceneMap.TryGetValue(SceneManager.GetActiveScene().name, out Scene currScene);
        _currScene = currScene;
    }

    public void LoadScene(Scene sceneEnum, Action<float> onSceneLoad = null)
    {
        _currScene = sceneEnum;
        StartCoroutine(OnLoadSceneCoroutine(sceneEnum, onSceneLoad));
    }
    private IEnumerator OnLoadSceneCoroutine(Scene SceneEnum, Action<float> onSceneLoad)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(SceneEnum.ToString(), LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;

            if(onSceneLoad != null)
                onSceneLoad(asyncOperation.progress);
        }

        asyncOperation.allowSceneActivation = true;
    }
}
