using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 考试难度选择
/// </summary>
public class SingleExamDifficultyView : SingleViewBase
{
    public ExamSetting examSetting;
    public Transform trans_lock;
    public Button btn_startBattle;//开始战斗
    public Text txt_lv;//等级
    public override void Init(params object[] args)
    {
        base.Init(args);
        examSetting = args[0] as ExamSetting;
        addBtnListener(btn_startBattle, () =>
        {
            ExamManager.Instance.StartExam(examSetting);
        });
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        ShowUnlockStatus();
        txt_lv.SetText("Lv." + examSetting.level);
    }

    /// <summary>
    /// 显示解锁状态
    /// </summary>
    void ShowUnlockStatus()
    {
        int level = examSetting.level.ToInt32();
        //解锁
        if (RoleManager.Instance.playerPeople.protoData.Achievement.UnlockedExamIdList.Contains(examSetting.id.ToInt32())
            ||examSetting.initLevel=="1")
        {
            trans_lock.gameObject.SetActive(false);
        }
        else
        {
            trans_lock.gameObject.SetActive(true);
        }
    }
}
