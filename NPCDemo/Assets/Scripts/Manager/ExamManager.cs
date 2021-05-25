using Framework.Data;
using RoleData;
using System;
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
        ExamData examData = new ExamData();
        examData.SettingId = examSetting.id.ToInt32();
        RoleManager.Instance._CurGameInfo.CurActionData.CurExamData = examData;
        for(int i = 0; i < 8; i++)
        {
            SingleExamEnemy enemy = new SingleExamEnemy();
            enemy.Id = DataTable.FindTestEnemyNumerialByLevel(examSetting.level.ToInt32()).id.ToInt32();
            enemy.Status = (int)SingleExamEnemyStatus.UnAccomplished;
            enemy.OnlyId = ConstantVal.SetId;
            InitExamProperty(enemy);
            RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList.Add(enemy);
        }
        //生成玩家的数据
        int count= RoleManager.Instance.playerPeople.protoData.PropertyData.ExamPropertyIdList.Count;
        RoleManager.Instance.playerPeople.protoData.PropertyData.CurExamPropertyIdList.Clear();
        RoleManager.Instance.playerPeople.protoData.PropertyData.CurExamPropertyDataList.Clear();

        for (int i = 0; i < count; i++)
        {
            int theId = RoleManager.Instance.playerPeople.protoData.PropertyData.ExamPropertyIdList[i];
            SinglePropertyData pro = RoleManager.Instance.playerPeople.protoData.PropertyData.ExamPropertyDataList[i];
            SinglePropertyData newPro = pro.Clone();
            RoleManager.Instance.playerPeople.protoData.PropertyData.CurExamPropertyIdList.Add(theId);
            RoleManager.Instance.playerPeople.protoData.PropertyData.CurExamPropertyDataList.Add(newPro);
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
        TestEnemyNumerialSetting setting = DataTable.FindTestEnemyNumerial(singleExamEnemy.Id);
        //singleExamEnemy.CurPropertyList.Clear();
        singleExamEnemy.Property = new PropertyData();
        singleExamEnemy.Property.OnlyId = singleExamEnemy.OnlyId;
        InitExamProperty(PropertyIdType.Attack, setting.attack.ToInt32(), singleExamEnemy.Property);
        InitExamProperty(PropertyIdType.Defense, setting.defense.ToInt32(), singleExamEnemy.Property);
        InitExamProperty(PropertyIdType.CritRate, setting.critRate.ToFloat(), singleExamEnemy.Property);
        InitExamProperty(PropertyIdType.CritNum, setting.crit.ToFloat(), singleExamEnemy.Property);
        InitExamProperty(PropertyIdType.SkillAdd, setting.skillHurtAdd.ToFloat(), singleExamEnemy.Property);
        InitExamProperty(PropertyIdType.Hp, setting.hp.ToInt32(), singleExamEnemy.Property);
        InitExamProperty(PropertyIdType.Speed, setting.attackSpeed.ToFloat(), singleExamEnemy.Property);


    }
    public void InitExamProperty(PropertyIdType idType,float num,PropertyData property)
    {

        SinglePropertyData singlePropertyData = new SinglePropertyData();
        singlePropertyData.PropertyId = (int)idType;
        singlePropertyData.PropertyNum = num;

        property.CurExamPropertyIdList.Add((int)idType);
        property.CurExamPropertyDataList.Add(singlePropertyData);

        SinglePropertyData initSinglePropertyData = new SinglePropertyData();
        initSinglePropertyData.PropertyId = (int)idType;
        initSinglePropertyData.PropertyNum = num;

        property.ExamPropertyIdList.Add((int)idType);
        property.ExamPropertyDataList.Add(initSinglePropertyData);

        //return singlePropertyData;
    }
    /// <summary>
    /// 开始单个题目 进入战斗面板
    /// </summary>
    public void StartSingleQuestion(SingleExamEnemy enemy)
    {
        //ExamSetting setting=
        PanelManager.Instance.OpenPanel<BattlePanel>(PanelManager.Instance.trans_layer2, enemy);
    }

    /// <summary>
    /// 通过唯一id获取考卷
    /// </summary>
    public SingleExamEnemy FindSingleExamEnemyWithOnlyId(UInt64 onlyId)
    {
        int count = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList.Count;
        for(int i = 0; i < count; i++)
        {
            SingleExamEnemy enemy = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList[i];
            if (enemy.OnlyId == onlyId)
            {
                return enemy;
            }
        }
        return null;
    }

    /// <summary>
    /// 找是否完成所有考试
    /// </summary>
    /// <returns></returns>
    public bool CheckIfAccomplishAllExam()
    {
        for(int i = 0; i < RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList.Count; i++)
        {
            SingleExamEnemy enemy = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.EnemyList[i];
            if(enemy.Status != (int)SingleExamEnemyStatus.Accomplished)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// 结算整个考试
    /// </summary>
    public void ResultTotalExam()
    {
        //按比例给钱
        int score = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.CurScore;
        int settingId = RoleManager.Instance._CurGameInfo.CurActionData.CurExamData.SettingId;
        ExamSetting setting = DataTable.FindExamSetting(settingId);
        string[] awardArr = setting.award.Split('|');
        int awardId = awardArr[0].ToInt32();
        int awardCount = Mathf.RoundToInt(awardArr[1].ToInt32() * score / (float)100);

        //升级前
        LevelInfo levelInfo = null;
        if (awardId == (int)PropertyIdType.Study)
        {
            levelInfo = RoleManager.Instance.GetPeopleLevelInfo(awardCount);
        }

        //考了100分可解锁下一关
        if (score == 100)
        {
            int nextId = setting.nextExamId.ToInt32();
            ExamSetting nextSetting = DataTable.FindExamSetting(nextId);
            if (nextSetting != null)
            {
                if (!RoleManager.Instance.playerPeople.protoData.Achievement.UnlockedExamIdList.Contains(nextId))
                {
                    RoleManager.Instance.playerPeople.protoData.Achievement.UnlockedExamIdList.Add(nextId);
                }
            }    
        }


        RoleManager.Instance.AddProperty((PropertyIdType)awardId, awardCount);
        RoleManager.Instance.InitBattleProperty();
        //血量回满 
        List<AwardData> awardList=new List<AwardData>();
        awardList.Add(new AwardData(AwardType.Property, awardId, awardCount));
        //把需要显示的发给ui
        EventCenter.Broadcast(TheEventType.ResultAllExam, awardList, levelInfo);
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
