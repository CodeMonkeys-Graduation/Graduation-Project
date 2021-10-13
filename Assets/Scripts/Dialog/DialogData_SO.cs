using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;


[CreateAssetMenu(order = 0, fileName = "DialogData_SO", menuName = "Dialog/DialogData_SO")]

[System.Serializable]
public class DialogData_SO : ScriptableObject // 캐싱용 컨테이너
{
    [System.Serializable]
    public class DialogDataDictionary : SerializableDictionaryBase<string, DialogData> { }

    public DialogDataDictionary dialogData_SO = new DialogDataDictionary();

    public void Add(string dkey, DialogData dData)
    {
        dialogData_SO.Add(dkey, dData);
    }

    public void Clear()
    {
        dialogData_SO.Clear();
    }
}

[System.Serializable]
public class DialogData // 하나의 엑셀 시트는 연속되는 대화 SET + 선택지 한 SET로 이루어짐
{
    public List<TalkData> talkDataSet = new List<TalkData>();
    public SelectionData selectionData = null;

    public TalkData GetTalkData(int idx)
    {
        if (idx >= talkDataSet.Count) return null;

        return talkDataSet[idx];
    }

    public SelectionData GetSelectionData()
    {
        return selectionData;
    }
}

[System.Serializable]
public class TalkData
{
    public string name;
    public string dialogue;
    public string emotion; // Json으로부터 데이터를 입력받음, 순서 변동하면 안됨
}

[System.Serializable]
public class SelectionData
{
    public string yes;
    public string no;
}
