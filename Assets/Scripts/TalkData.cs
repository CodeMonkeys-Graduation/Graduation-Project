using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkDataContainer
{
    public List<TalkData> talkDataContainer = new List<TalkData>();

    public TalkDataContainer()
    {
        talkDataContainer.Add(new TalkData());
    }
}

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

    public TalkData()
    {
        id = 0;
        context = null;
        emotion = 0;
    }
}
