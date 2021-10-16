using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDataMgr
{
    public static DialogData_SO dialogDataContainer = Resources.Load<DialogData_SO>("DialogData_SO"); // 다이얼로그 데이터 캐싱 작업

    public static void InitDialogData()
    {
        dialogDataContainer.Clear();

        string[] dataNumers = { "0", "00", "01", "000", "0001", "1", "10", "11", "100", "2", "20", "3", "4", "40" };

        foreach(string num in dataNumers)
        {
            dialogDataContainer.Add(num, LoadDialogData(num));
        }
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
