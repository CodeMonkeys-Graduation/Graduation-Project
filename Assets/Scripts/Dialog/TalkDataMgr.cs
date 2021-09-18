using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkDataMgr : MonoBehaviour
{
    public static TalkDataContainer talkSet;

    [SerializeField] int stageProgress = 0;

    void Awake()
    {
        talkSet = LoadTalkData(stageProgress);
    }

    public TalkDataContainer LoadTalkData(int stageProgress)
    {
        string jsonFileName = "talkData-" + stageProgress.ToString();
        TextAsset jsonData = Resources.Load<TextAsset>(jsonFileName);

        Debug.Log("TALKDATA LOAD COMPLETE");

        return JsonUtility.FromJson<TalkDataContainer>(jsonData.ToString());
    }

}
