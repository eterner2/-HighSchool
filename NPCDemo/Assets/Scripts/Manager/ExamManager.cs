﻿using Framework.Data;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamManager : CommonInstance<ExamManager>
{
   
    //List<SingleExamEnemy> curExamEnemyList=

    /// <summary>
    /// 开始考试
    /// </summary>
    public void StartExam(ExamSetting examSetting)
    {
        //扣除体力
       string[] costArr= examSetting.cost.Split('|');
        PropertyIdType theId =(PropertyIdType)costArr[0].ToInt32();
        int num = costArr[1].ToInt32();
        SinglePropertyData pro = RoleManager.Instance.FindSinglePropertyData(theId);
        PropertySetting propertySetting = DataTable.FindPropertySetting(pro.PropertyId);

        if (RoleManager.Instance.FindSinglePropertyData(theId).PropertyNum < num)
        {
            PanelManager.Instance.OpenFloatWindow(propertySetting.name + "不足");
        }
        else
        {
            RoleManager.Instance.DeProperty(theId,num);
            GenerateExam(examSetting);
        }

    }


    /// <summary>
    /// 生成一组测试数据
    /// </summary>
    public void GenerateExam(ExamSetting examSetting)
    {
        RoleManager.Instance._CurGameInfo.CurActionData.CurExamData = new ExamData();
        for(int i = 0; i < 8; i++)
        {
            SingleExamEnemy enemy = new SingleExamEnemy();
            enemy.Id = DataTable._testEnemyNumerialList[examSetting.level.ToInt32()].id.ToInt32();
            enemy.Status = (int)SingleExamEnemyStatus.UnAccomplished;
            RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList.Add(enemy);
        }
        //发消息给view显示数据
        EventCenter.Broadcast(TheEventType.StartExam);
    }

    /// <summary>
    /// 初始化考试属性
    /// </summary>
    /// <param name="singleExamEnemy"></param>
    public void InitExamProperty(SingleExamEnemy singleExamEnemy)
    {
        //singleExamEnemy.Id;
        TestEnemyNumerialSetting setting = DataTable._testEnemyNumerialList[singleExamEnemy.Id];
        singleExamEnemy.CurPropertyList.Clear();
        InitExamProperty(PropertyIdType.Attack, setting.attack.ToInt32());
        InitExamProperty(PropertyIdType.Defense, setting.defense.ToInt32());
        InitExamProperty(PropertyIdType.CritRate, setting.critRate.ToFloat());
        InitExamProperty(PropertyIdType.CritNum, setting.crit.ToFloat());
        InitExamProperty(PropertyIdType.SkillAdd, setting.skillHurtAdd.ToFloat());
        InitExamProperty(PropertyIdType.Hp, setting.hp.ToInt32());
        InitExamProperty(PropertyIdType.Speed, setting.attackSpeed.ToFloat());


    }
    public SinglePropertyData InitExamProperty(PropertyIdType idType,float num)
    {

        SinglePropertyData singlePropertyData = new SinglePropertyData();
        singlePropertyData.PropertyId = (int)idType;
        singlePropertyData.PropertyNum = num;


        return singlePropertyData;
    }
    /// <summary>
    /// 开始单个题目 进入战斗面板
    /// </summary>
    public void StartSingleQuestion(SingleExamEnemy enemy)
    {
        //ExamSetting setting=
    }
}

/// <summary>
/// 单个考试完成状态
/// </summary>
public enum SingleExamEnemyStatus
{
    None=0,
    Accomplished=1,
    UnAccomplished=2,
}
