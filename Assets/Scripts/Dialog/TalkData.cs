using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkDataContainer
{
    public List<TalkData> talkDataContainer = new List<TalkData>();

    public TalkDataContainer()
    {
        talkDataContainer.Add(new TalkData());
    }
}
[System.Serializable]
public class TalkData
{
    public enum Emotion
    {
        Basic, //일반
        Sad, //슬픔
        Happiness, //행복
        Upset //화남
    }

    public int id;
    public string context;
    public int emotion;

}
