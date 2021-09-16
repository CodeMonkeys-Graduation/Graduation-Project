using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioMgr : SingletonBehaviour<AudioMgr>
{
    [System.Serializable]
    public class SceneBGMDictionary : SerializableDictionaryBase<SceneMgr.Scene, AudioClip> { }

    [SerializeField] private SceneBGMDictionary sceneBGMs;

    [SerializeField] private AudioMixer audioMixer;

    private Dictionary<AudioType, AudioMixerGroup> audioMixerGroups = new Dictionary<AudioType, AudioMixerGroup>();

    private List<(AudioType type, AudioSource audioSource)> audioInstances
        = new List<(AudioType type, AudioSource audioSource)>();

    // Audio Group과 이름을 정확히 맞출 것.
    // AudioMgr.Start참고
    public enum AudioType
    {
        BGM,
        SFX,
        UI,
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            var groups = audioMixer.FindMatchingGroups(type.ToString());
            if(groups.Length > 0)
            {
                audioMixerGroups.Add(type, groups[0]);
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateBGM();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateBGM();
    }

    private void UpdateBGM()
    {
        var bgmAudiosTuple = audioInstances.FindAll(tuple => tuple.type == AudioType.BGM);
        foreach(var tuple in bgmAudiosTuple)
        {
            audioInstances.Remove(tuple);
            Destroy(tuple.audioSource.gameObject);
        }
        
        sceneBGMs.TryGetValue(SceneMgr.Instance._currScene, out AudioClip bgmClip);
        if(bgmClip != null)
        {
            PlayAudio(bgmClip, AudioType.BGM, true);
        }
    }

    public void PlayAudio(AudioClip clip, AudioType type, bool loop)
    {
        GameObject audioGO = new GameObject(type.ToString());
        DontDestroyOnLoad(audioGO);
        AudioSource audioSource = audioGO.AddComponent<AudioSource>();

        audioSource.playOnAwake = true;
        audioSource.loop = loop;
        audioSource.clip = clip;
        if (audioMixerGroups.TryGetValue(type, out AudioMixerGroup group))
            audioSource.outputAudioMixerGroup = group;

        audioInstances.Add((type, audioSource));

        audioSource.Play();

        if (!loop)
        {
            StartCoroutine(DestroyOnEnd(audioGO, clip.length));
        }
    }

    private IEnumerator DestroyOnEnd(GameObject audioGO, float seconds)
    {
        yield return new WaitForSeconds(seconds + 0.5f);

        if(audioGO != null)
        {
            audioInstances.RemoveAll((tuple) => tuple.audioSource.gameObject == audioGO);
            Destroy(audioGO);
        }
    }
}
