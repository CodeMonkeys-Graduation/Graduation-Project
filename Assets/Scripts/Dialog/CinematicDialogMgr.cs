using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicDialogMgr : MonoBehaviour
{
    [SerializeField] DialogController dialogController;
    [SerializeField] public int stageNumber;

    PlayableDirector playableDirector; // 타임라인을 실행시키고, 타임라인으로 
    
   
    void Start()
    {
        SolveCurrentStageNumber(SceneMgr.Instance._currScene.ToString());

        dialogController = Instantiate(dialogController, transform);
        dialogController.Init(stageNumber);
        dialogController.gameObject.SetActive(false);

        playableDirector = FindObjectOfType<PlayableDirector>(); 
    }

    void SolveCurrentStageNumber(string scenename)
    {
        string num = scenename.Substring(scenename.Length - 1);
        Debug.Log(num);
        stageNumber = int.Parse(num) - 1; // 다이얼로그 1 ㅡ> Number 0
    }

    public void TimelineEnd()
    {
        Debug.Log("애니메이션 종료, 다이얼로그 시작");

        //pd.gameObject.SetActive(false);
        dialogController.gameObject.SetActive(true);
    }
    
    public void TimelineStart()
    {
        Debug.Log("다이얼로그 종료, 애니메이션 실행");
        playableDirector.Play();
    }
}
