using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicDialogMgr : SingletonBehaviour<CinematicDialogMgr>
{
    [SerializeField] DialogController dialogController;
    [SerializeField] TimelineController timelineController;

    [SerializeField] public SceneMgr.Scene nextScene;

    void Start()
    {
        dialogController = Instantiate(dialogController, transform);
        dialogController.Init(SceneMgr.Instance._currScene);

        timelineController = FindObjectOfType<TimelineController>();
        timelineController.Init(SceneMgr.Instance._currScene);

        dialogController.Pause();
        timelineController.Play();
    }

    public void DialogOn()
    {
        Debug.Log("타임라인 정지, 다이얼로그 시작");

        dialogController.Play();
        timelineController.Pause();
    }
    
    public void CinematicOn()
    {
        Debug.Log("다이얼로그 정지, 애니메이션 실행");
        dialogController.Pause();
        timelineController.Play();
    }

    public void CinematicEnd()
    {
        Debug.Log("다이얼로그 종료, 씬 이동");
        SceneMgr.Instance.LoadScene(nextScene);
    }
}
