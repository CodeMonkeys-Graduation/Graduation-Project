using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    [System.Serializable]
    public enum Scene // ToString()으로 검사하기때문에 Scene의 이름과 정확히 일치해야합니다. 
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

    public void LoadScene(Scene sceneEnum, Action<float> onSceneLoad = null)
    {
        StartCoroutine(OnLoadSceneCoroutine(sceneEnum, onSceneLoad));
    }

    private IEnumerator OnLoadSceneCoroutine(Scene SceneEnum, Action<float> onSceneLoad)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(SceneEnum.ToString(), LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);

            if(onSceneLoad != null)
                onSceneLoad(asyncOperation.progress);
        }

        asyncOperation.allowSceneActivation = true;
    }
}
