using Framework.Data;
using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 考试准备界面
/// </summary>
public class ExamPreparePanel : PanelBase
{
    public Transform trans_difficultyChoose;//选择难度面板
    public Transform grid_difficultyChoose;//选择难度

    public List<Transform> examPosList = new List<Transform>();//考试位置
    public List<SingleExamQuestionView> examViewList = new List<SingleExamQuestionView>();//所有考试题目

    public Button btn_quit;

    public Transform trans_result;//结算
    public Text txt_result;
    public Button btn_result;//结算

    public List<AwardData> resultAwardList;//结算奖励

    public Transform trans_statusGrid;//状态

    public override void Init(params object[] args)
    {
        base.Init(args);

        EventCenter.Register(TheEventType.StartExam, OnExamStart);
        EventCenter.Register(TheEventType.BattleEnd, OnBattleEnd);
        EventCenter.Register(TheEventType.ResultAllExam, ShowResult);
        addBtnListener(btn_quit, () =>
        {
            OnQuitClick();
        });
        addBtnListener(btn_result, OnResultBtnClick);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        ShowAllDifficulttyExam();
        //ShowStatus();
        trans_result.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示状态
    /// </summary>
    public void ShowStatus()
    {
        SinglePropertyData hpPro = new SinglePropertyData();
        hpPro.PropertyId = (int)PropertyIdType.Hp;
        hpPro.PropertyLimit = BattleManager.Instance.GetInitExamPropertyById(PropertyIdType.Hp, RoleManager.Instance.playerPeople.protoData.PropertyData).PropertyNum;
        hpPro.PropertyNum = BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, RoleManager.Instance.playerPeople.protoData.PropertyData).PropertyNum;
        PanelManager.Instance.OpenSingle<StatusPropertyView>(trans_statusGrid, hpPro);

        SinglePropertyData scorePro = new SinglePropertyData();
        scorePro.PropertyId = (int)PropertyIdType.Score;
        scorePro.PropertyLimit = 100;
        scorePro.PropertyNum = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.CurScore;
        PanelManager.Instance.OpenSingle<StatusPropertyView>(trans_statusGrid, scorePro);

    }

    void OnQuitClick()
    {
        if (ExamManager.Instance.CheckIfAccomplishAllExam())
        {
            ExamManager.Instance.ResultTotalExam();
        }
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
            examViewList.Add(PanelManager.Instance.OpenSingle<SingleExamQuestionView>(examPosList[i],
                RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList[i]));
        }
        ShowStatus();
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
        EventCenter.Remove(TheEventType.BattleEnd, OnBattleEnd);
        EventCenter.Remove(TheEventType.ResultAllExam, ShowResult);

        PanelManager.Instance.CloseAllSingle(grid_difficultyChoose);
        PanelManager.Instance.CloseAllSingle(trans_statusGrid);
        examViewList.Clear();
    }

    /// <summary>
    /// 显示结算
    /// </summary>
    public void ShowResult(object[] args)
    {
        resultAwardList = args[0] as List<AwardData>;
        trans_result.gameObject.SetActive(true);
        txt_result.SetText("考试结束，您的得分是" + RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.CurScore + "分");
    }

    /// <summary>
    /// 结算
    /// </summary>
    void OnResultBtnClick()
    {
        Action OnQuitExam = delegate
        {
            PanelManager.Instance.ClosePanel(this);
        };
        PanelManager.Instance.OpenPanel<GetAwardPanel>(PanelManager.Instance.trans_layer2, resultAwardList, OnQuitExam);
    }

    /// <summary>
    /// 战斗结束 应该显示勾
    /// </summary>
    void OnBattleEnd(object[] args)
    {
        PanelManager.Instance.CloseAllSingle(trans_statusGrid);
        ShowStatus();

        PropertyData deadPro = args[0] as PropertyData;
        if (!deadPro.IsPlayer)
        {
            for(int i = 0; i < examViewList.Count; i++)
            {
                SingleExamQuestionView view = examViewList[i];
                if (view.singleEnemy.OnlyId == deadPro.OnlyId)
                {
                    view.RefreshShow();
                }
            }
        }
    }


}
