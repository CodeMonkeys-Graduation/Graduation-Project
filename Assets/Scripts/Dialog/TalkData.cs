using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkDataContainer
{
    public List<TalkData> talkDataContainer = new List<TalkData>();
    public List<SelectionData> selectionDataContainer = new List<SelectionData>();

    public TalkDataContainer()
    {
        talkDataContainer.Add(new TalkData());
        selectionDataContainer.Add(new SelectionData());
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
    public bool hasSelection;
}

[System.Serializable]
public class SelectionData
{
    public string yes;
    public string no;
}
