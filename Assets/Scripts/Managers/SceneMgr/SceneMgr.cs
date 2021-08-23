using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    [System.Serializable]
    public enum Scene
    { 
        Main, 
        Dialog,
        UnitSelection,
        Battle
    }

    public Dictionary<Scene, string> SceneMap = new Dictionary<Scene, string>();

    [SerializeField] public Event _onSceneChanged;

    bool _IsOnLoad = false;

    // Start is called before the first frame update
    void Start()
    {
        SceneMap.Add(Scene.Main, Scene.Main.ToString());
        SceneMap.Add(Scene.Dialog, Scene.Dialog.ToString());
        SceneMap.Add(Scene.UnitSelection, Scene.UnitSelection.ToString());
        SceneMap.Add(Scene.Battle, Scene.Battle.ToString());
    }

    // Update is called once per frame
    void Update()
    {
    }

    // TODO Delete
    public void LoadSceneBtnTest(string sceneName)
    {
        foreach (var scene in SceneMap)
            if (scene.Value == sceneName)
                LoadScene(scene.Key);
    }

    public void LoadScene(Scene SceneEnum, float loadDelay = 0f, Action<float> onSceneLoad = null)
    {
        var AsyncOperation = SceneManager.LoadSceneAsync(SceneMap[SceneEnum], LoadSceneMode.Single);
        SceneChanged sceneChangedParam = new SceneChanged(SceneEnum);

        if (loadDelay > 0f)
        {
            AsyncOperation.allowSceneActivation = false;
            Action onLoadComplete = () => { _onSceneChanged.Invoke(sceneChangedParam); AsyncOperation.allowSceneActivation = true; };
            object[] parameters = { loadDelay, onSceneLoad, onLoadComplete };
            StartCoroutine(OnLoadSceneCoroutine(parameters));
        }
        else
        {
            AsyncOperation.allowSceneActivation = true;
            _onSceneChanged.Invoke(sceneChangedParam);
        }
    }

    private IEnumerator OnLoadSceneCoroutine(object[] parameters)
    {
        float curr = 0f;

        float delay = (float)parameters[0];
        Action<float> onSceneLoad = (Action<float>)parameters[1];
        Action onLoadComplete = (Action)parameters[2];

        while (curr < delay)
        {
            yield return null;
            curr += Time.deltaTime;
            onSceneLoad(curr);
        }

        onLoadComplete();
    }
}
