using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个题目
/// </summary>
public class SingleExamQuestionView :SingleViewBase
{
    public SingleExamEnemy singleEnemy;


    public Transform trans_gou;

    public Button btn_battle;//开战

    public override void Init(params object[] args)
    {
        base.Init(args);
        singleEnemy =args[0] as SingleExamEnemy;
        addBtnListener(btn_battle, () =>
        {
            //如果精力耗尽，则输
           if(RoleManager.Instance.FindSinglePropertyData(PropertyIdType.Hp).PropertyNum <= 0)
            {
                PanelManager.Instance.OpenFloatWindow("精力已耗尽，无法做题");
            }
            else
            {
                ExamManager.Instance.StartSingleQuestion(singleEnemy);

            }
        });
    }
    public override void OnOpenIng()
    {
        base.OnOpenIng();
        if(singleEnemy.Status == (int)SingleExamEnemyStatus.Accomplished)
        {
            btn_battle.enabled = false;
            trans_gou.gameObject.SetActive(true);
        }
        else
        {
            btn_battle.enabled = true;

            trans_gou.gameObject.SetActive(false);

        }



    }

    /// <summary>
    /// 刷新显示
    /// </summary>
    public void RefreshShow()
    {
        UInt64 enemyId = singleEnemy.OnlyId;
        singleEnemy = ExamManager.Instance.FindSingleExamEnemyWithOnlyId(enemyId);
        OnOpenIng();
    }

    
}
