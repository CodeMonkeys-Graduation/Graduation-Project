using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDataMgr
{
    public static DialogDataContainer dialogDataContainer = Resources.Load<DialogDataContainer>("DialogDataContainer"); // 다이얼로그 데이터 캐싱 작업

    public static void InitDialogData()
    {
        dialogDataContainer.Add("0", LoadDialogData("0"));
        dialogDataContainer.Add("00", LoadDialogData("00"));
        dialogDataContainer.Add("01", LoadDialogData("01"));
        dialogDataContainer.Add("000", LoadDialogData("000"));
        dialogDataContainer.Add("001", LoadDialogData("001"));
    }

    public static List<TalkData> GetTalkDataForEditing(string number) // 에디팅용 함수, 게임 실행 전에 사용하므로 JSON에서 직접 파싱해서 써야 함 + 성능 이슈가 없음
    {
        DialogData ddc = LoadDialogData(number);
        return ddc.talkDataSet;
    }

    public static SelectionData GetSelectionDataForEditing(string number)
    {
        DialogData ddc = LoadDialogData(number);
        return ddc.selectionData;
    }

    public static DialogData LoadDialogData(string number)
    {
        string jsonFileName = "Dialogs/Dialog" + number;
        TextAsset jsonData = Resources.Load<TextAsset>(jsonFileName);

        Debug.Log("DIALOGDATA LOAD COMPLETE");

        return JsonUtility.FromJson<DialogData>(jsonData.ToString());
    }
}
