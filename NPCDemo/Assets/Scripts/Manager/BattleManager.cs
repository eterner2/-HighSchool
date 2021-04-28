using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;

public class BattleManager : CommonInstance<BattleManager>
{


    /// <summary>
    /// 初始化当前战斗属性
    /// </summary>
    public void InitCurExamPropertyData(PropertyData PropertyData)
    {
        for (int i = 0; i < PropertyData.ExamPropertyIdList.Count; i++)
        {
            SinglePropertyData initData = PropertyData.ExamPropertyDataList[i];
            SinglePropertyData singleExamPropertyData = new SinglePropertyData();
            singleExamPropertyData.PropertyId = initData.PropertyId;
            singleExamPropertyData.PropertyNum = initData.PropertyNum;

            PropertyData.CurExamPropertyDataList.Add(singleExamPropertyData);
            PropertyData.CurExamPropertyIdList.Add(singleExamPropertyData.PropertyId);


        }
    }

    /// <summary>
    /// 改变考试属性的值
    /// </summary>
    /// <param name="propertyIdType"></param>
    /// <param name="val"></param>
    public void ChangeCurExamPropertyData(PropertyData data, PropertyIdType propertyIdType,int val)
    {
        for (int i = 0; i < data.CurExamPropertyIdList.Count; i++)
        {
            if (data.CurExamPropertyIdList[i] == (int)propertyIdType)
            {
                SinglePropertyData singleData = data.CurExamPropertyDataList[i];
                singleData.PropertyNum += val;
                if (singleData.PropertyNum < 0)
                {
                    singleData.PropertyNum = 0;
                }
            }
        }
    }

    /// <summary>
    /// 通过id得到当前战斗属性
    /// </summary>
    /// <param name="propertyData"></param>
    public SinglePropertyData GetCurExamPropertyById(PropertyIdType propertyIdType, PropertyData PropertyData)
    {
        for(int i = 0; i < PropertyData.CurExamPropertyIdList.Count; i++)
        {
            if (PropertyData.CurExamPropertyIdList[i] == (int)propertyIdType)
            {
                return PropertyData.CurExamPropertyDataList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 通过id得到初始战斗属性
    /// </summary>
    /// <param name="propertyData"></param>
    public SinglePropertyData GetInitExamPropertyById(PropertyIdType propertyIdType, PropertyData PropertyData)
    {
        for (int i = 0; i < PropertyData.ExamPropertyIdList.Count; i++)
        {
            if (PropertyData.ExamPropertyIdList[i] == (int)propertyIdType)
            {
                return PropertyData.ExamPropertyDataList[i];
            }
        }
        return null;
    }


    /// <summary>
    /// 攻击1打2
    /// </summary>
    public void Attack(PropertyData property1, PropertyData property2)
    {
        bool crit = false;//是否暴击（ui显示要区分）
        //攻击者buff
 
        //float buffAddCritHurt = 150;//暴击伤害 （暂定都是150）

        ////受击者buff
        //float buffAddDefence = 0;
        //for (int i = 0; i < me.caracterCacheData.curBuffList.Count; i++)
        //{
        //    Buff buff = me.caracterCacheData.curBuffList[i];
        //    if ((BuffType)buff.buffSetting.type.ToInt32() == BuffType.Defense)
        //    {
        //        buffAddDefence += buff.buffSetting.param.ToFloat();
        //    }
        //}

        float attack = GetCurExamPropertyById(PropertyIdType.Attack,property1).PropertyNum;

        float defence = GetCurExamPropertyById(PropertyIdType.Defense, property2).PropertyNum;

        float critRate = GetCurExamPropertyById(PropertyIdType.CritRate, property1).PropertyNum;
        float critNum = GetCurExamPropertyById(PropertyIdType.CritNum, property1).PropertyNum;
        float skillAdd= GetCurExamPropertyById(PropertyIdType.SkillAdd, property1).PropertyNum;
        //float critHurt = 120;

        float critMul = 1;
        if (RandomManager.Next(0, 100) < critRate*100)
        {
            crit = true;
            critMul = 1 + critNum;
        }
        float skillAddVal = (1 + skillAdd);

        int res = Mathf.RoundToInt((attack * attack / (attack + defence)) * critMul* skillAddVal);

        ChangeCurExamPropertyData(property2, PropertyIdType.Hp, -res);
        //发信息给UI显示
        HitData hit = new HitData(res, crit, property2.IsPlayer);
        EventCenter.Broadcast(TheEventType.BattleHit, hit);

        ///打死了
        if (GetCurExamPropertyById(PropertyIdType.Hp,property2).PropertyNum <= 0)
        {
            EventCenter.Broadcast(TheEventType.BattleEnd, property2);

        }
    }


}

/// <summary>
/// 打击
/// </summary>
public class HitData
{
    public int num;//打了多少血
    public bool ifCrit;//是否暴击
    public bool isPlayer;//是打玩家
    
    public HitData(int num,bool ifCrit,bool isPlayer)
    {
        this.num = num;
        this.ifCrit = ifCrit;
        this.isPlayer = isPlayer;
    }
}


/// <summary>
/// 敌人数据
/// </summary>
public enum EnemyType
{
    MockExam
}