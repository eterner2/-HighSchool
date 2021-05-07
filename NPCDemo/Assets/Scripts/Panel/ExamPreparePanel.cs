using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 考试准备界面
/// </summary>
public class ExamPreparePanel : PanelBase
{
    public Transform trans_difficultyChoose;//选择难度面板
    public Transform grid_difficultyChoose;//选择难度

    public List<Transform> examPosList = new List<Transform>();//考试位置

    public override void Init(params object[] args)
    {
        base.Init(args);

        EventCenter.Register(TheEventType.StartExam, OnExamStart);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        ShowAllDifficulttyExam();

    }

    /// <summary>
    /// 开始考试
    /// </summary>
    void OnExamStart()
    {
        trans_difficultyChoose.gameObject.SetActive(false);
        int count = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList.Count;
        for(int i = 0; i < count; i++)
        {
            PanelManager.Instance.OpenSingle<SingleExamQuestionView>(examPosList[i],
                RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList[i]);
        }
    }

    public void ShowAllDifficulttyExam()
    {
       int count= DataTable._examList.Count;
        for(int i = 0; i < count; i++)
        {
            ExamSetting examSetting = DataTable._examList[i];
            PanelManager.Instance.OpenSingle<SingleExamDifficultyView>(grid_difficultyChoose, examSetting);
        }
        trans_difficultyChoose.gameObject.SetActive(true);

    }

    public override void Clear()
    {
        base.Clear();
        EventCenter.Remove(TheEventType.StartExam, OnExamStart);

        PanelManager.Instance.CloseAllSingle(grid_difficultyChoose);
    }

    
}
