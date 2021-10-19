using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicDialogMgr : SingletonBehaviour<CinematicDialogMgr>
{
    TimelineController timelineController;
    DialogCanvas dialogController;

    [SerializeField] public SceneMgr.Scene nextScene;

    void Start()
    {
        nextScene = SceneMgr.Instance._currScene == SceneMgr.Scene.Dialog3 ? SceneMgr.Scene.Dialog4 : SceneMgr.Scene.UnitSelection;
        dialogController = null;
        timelineController = FindObjectOfType<TimelineController>();
    }

    public void DialogOn()
    {
        Debug.Log("타임라인 정지, 다이얼로그 시작");

        if (dialogController == null) dialogController = (DialogCanvas)UIMgr.Instance.GetCurrentCanvas();

        dialogController.Play();
        timelineController.Pause();
    }

    public void CinematicOn()
    {
        Debug.Log("다이얼로그 정지, 애니메이션 실행");

        if (dialogController == null) dialogController = (DialogCanvas)UIMgr.Instance.GetCurrentCanvas();

        dialogController.Pause();
        timelineController.Play();
    }

    public void CinematicEnd()
    {
        Debug.Log("다이얼로그 종료, 씬 이동");
        SceneMgr.Instance.LoadScene(nextScene);

        SaveManager.WatchedDialog(SceneMgr.Instance._currScene);
    }

    public void SetTalkData(TalkData talkData)
    {
        if (dialogController == null) dialogController = (DialogCanvas)UIMgr.Instance.GetCurrentCanvas();

        dialogController.SetTalk(talkData);
    }

    public void SelectSelectionData(SelectionData selectionData)
    {
        if (dialogController == null) dialogController = (DialogCanvas)UIMgr.Instance.GetCurrentCanvas();

        dialogController.SetSelection(selectionData);
    }

    public void Select(int idx)
    {
        if (dialogController == null) dialogController = (DialogCanvas)UIMgr.Instance.GetCurrentCanvas();

        dialogController.dialogAnimator.SetInteger("Selected", idx);
    }
}
