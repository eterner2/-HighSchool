using Framework.Data;
using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager
{
    private static RoleManager inst = null;

    public static RoleManager Instance
    {
        get
        {
            if (inst == null)
            {
                inst = new RoleManager();
            }
            return inst;
        }

    }

    public GameInfo _CurGameInfo;
    //public int CurScore = 0;
    public List<People> allPeopleList;//所有人
    public List<People> NPCPeopleList;//所有npc（除了玩家之外的所有人）
    public People playerPeople;//玩家

    //public PropertyData examPropertyData;//考试数据

    public void Init(int index)
    {
        //EventCenter.Register(TheEventType.)
        //没有存档
        if (index == -1)
        {
            CreateNew();
        }
    }


    //初始化玩家数据
    
    void CreateNew()
    {
        GameInfo gameInfo = new GameInfo();
        _CurGameInfo = gameInfo;

        CreateNewTimeData(gameInfo);
       // CreateNewPlayer(gameInfo);
        CreateNewPeople(gameInfo);
        //CreateNewPropertyData(gameInfo);
        InitTmpExamPropertyData();
        gameInfo.CurGameModule = (int)GameModuleType.WeekDay;
    }

    /// <summary>
    /// 初始化临时考试数据
    /// </summary>
    void InitTmpExamPropertyData()
    {
        //examPropertyData = new PropertyData();
        //InitSingleProperty(examPropertyData, PropertyIdType.Hp);
        //InitSingleProperty(examPropertyData, PropertyIdType.Attack);
        //InitSingleProperty(examPropertyData, PropertyIdType.Defense);
        //InitSingleProperty(examPropertyData, PropertyIdType.CritRate);
        //InitSingleProperty(examPropertyData, PropertyIdType.Speed);
        //InitSingleProperty(examPropertyData, PropertyIdType.CritNum);
        //InitSingleProperty(examPropertyData, PropertyIdType.SkillAdd);

    }

    /// <summary>
    /// 创建新的日期数据
    /// </summary>
    void CreateNewTimeData(GameInfo gameInfo)
    {
        TimeData timeData = new TimeData();
        timeData.Year = 1;
        timeData.Month = 9;
        timeData.TheWeekDay = 5;
        timeData.DayBeforeExam = 300;
        gameInfo.TimeData = timeData;
    }
    ///// <summary>
    ///// 创建新的玩家
    ///// </summary>
    //void CreateNewPlayer(GameInfo gameInfo)
    //{
    //    PeopleProtoData peopleProtoData = new PeopleProtoData();
    //    CreateNewPropertyData(peopleProtoData);
    //    gameInfo.PlayerPeople = peopleProtoData;
    //    gameInfo.AllPeopleList.Add(peopleProtoData);
    //}

    /// <summary>
    /// 创建新的所有人
    /// </summary>
    /// <param name="gameInfo"></param>
    void CreateNewPeople(GameInfo gameInfo)
    {
        allPeopleList = new List<People>();
        NPCPeopleList = new List<People>();
        //暂时用这个scriptable TODO改成读表
        PeopleScriptable peopleScriptable = NewBehaviourScript.Instance.peopleScriptable;
        for(int i = 0; i < peopleScriptable.peopleDataList.Count; i++)
        {
          
            People p = new People(peopleScriptable.peopleDataList[i]);
            //if (p.name == "毛鹏程")
            //    p.isPlayer = true;

            if (p.isPlayer)
            {
                playerPeople = p;
                gameInfo.PlayerPeople = p.protoData;
            }
            else
            {
                NPCPeopleList.Add(p);

            }
            allPeopleList.Add(p);
        }


    
    }
    /// <summary>
    /// 初始化人物属性
    /// </summary>
    /// <param name="propertyData"></param>
    /// <param name="idType"></param>
    /// <param name="propertyNum"></param>
    public void InitSingleProperty(PropertyData propertyData, PropertyIdType idType,float propertyNum)
    {
        //PropertyData propertyData = new PropertyData();


        //如果属性为-1 说明非考试属性 从property表读取
        if (propertyNum !=-1)
        {
            //TestEnemyNumerialSetting examSetting = DataTable.FindTestNumerial((int)idType));
            //考试数值需要和等级挂钩
            SinglePropertyData examPro = new SinglePropertyData();
            examPro.PropertyId = (int)idType;
            examPro.PropertyNum = propertyNum;
            examPro.PropertyLimit = -1;

            propertyData.CurExamPropertyIdList.Add((int)idType);
            propertyData.CurExamPropertyDataList.Add(examPro);


            SinglePropertyData initsinglePropertyData = new SinglePropertyData();
            initsinglePropertyData.PropertyId = (int)idType;
            initsinglePropertyData.PropertyNum = propertyNum;
            initsinglePropertyData.PropertyLimit = -1;

            propertyData.ExamPropertyIdList.Add((int)idType);
            propertyData.ExamPropertyDataList.Add(initsinglePropertyData);

        }
        else
        {
            PropertySetting setting = DataTable.FindPropertySetting((int)idType);

            string[] rdmRange = setting.newRdmRange.Split('|');
            int val = RandomManager.Next(rdmRange[0].ToInt32(), rdmRange[1].ToInt32());


            SinglePropertyData singlePropertyData = new SinglePropertyData();
            singlePropertyData.PropertyId = (int)idType;
            singlePropertyData.PropertyNum = val;
            singlePropertyData.PropertyLimit = setting.haveLimit.ToInt32();

            propertyData.PropertyIdList.Add((int)idType);

            propertyData.PropertyDataList.Add(singlePropertyData);
        }
        //return singlePropertyData;
    }
    /// <summary>
    /// 得到学习分数
    /// </summary>
    public void GetStudyScore()
    {
        //CurScore += 30;
        //int init
        PropertySetting pro = DataTable.FindPropertySetting((int)PropertyIdType.Study);
        AddProperty(PropertyIdType.Study, 30);
        EventCenter.Broadcast(TheEventType.GetStudyScore, "获得"+ pro.name+30);
        //GameObject.Find("SinglePropertyView/txt_num").GetComponent<Text>().SetText(CurScore.ToString());
        //PanelManager.Instance.OpenSingle<FlyTxtView>(GameObject.Find("DeskPanel/trans_proChangeParent").transform,"获得知识+30");

    }

    /// <summary>
    /// 获取能力
    /// </summary>
    public SinglePropertyData FindSinglePropertyData(PropertyIdType propertyIdType,PeopleProtoData peopleProto=null)
    {
        //int studyId=ConstantVal
        if (peopleProto == null)
            peopleProto = _CurGameInfo.PlayerPeople;
        if (_CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.Contains((int)propertyIdType))
        {
            int index = _CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.IndexOf((int)propertyIdType);
            return _CurGameInfo.PlayerPeople.PropertyData.PropertyDataList[index];
        }
        return null;
    }
    ///// <summary>
    ///// 获取能力值
    ///// </summary>
    //public int FindPropertyNum(PropertyIdType propertyIdType)
    //{
    //    //int studyId=ConstantVal
    //    if (_CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.Contains((int)propertyIdType))
    //    {
    //        int index= _CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.IndexOf((int)propertyIdType);
    //        return _CurGameInfo.PlayerPeople.PropertyData.PropertyDataList[index].PropertyNum;
    //    }
    //    return 0;
    //}

    /// <summary>
    /// 增加能力值 传了人就是给人加
    /// </summary>
    /// <param name="propertyIdType"></param>
    /// <param name="num"></param>
    public void AddProperty(PropertyIdType propertyIdType, int num,PeopleProtoData peopleProto=null)
    {
        if (peopleProto == null)
        {
            peopleProto = _CurGameInfo.PlayerPeople;
        }
        if (peopleProto.PropertyData.PropertyIdList.Contains((int)propertyIdType))
        {
            int index = peopleProto.PropertyData.PropertyIdList.IndexOf((int)propertyIdType);
            SinglePropertyData singleData = peopleProto.PropertyData.PropertyDataList[index];
            float limit = singleData.PropertyLimit;
            singleData.PropertyNum += num;
            //如果该属性存在最大限制
            if (limit >= 0)
            {
                if (singleData.PropertyLimit >= limit)
                {
                    singleData.PropertyNum = limit;
                }
            }

            //如果是经验 则增加等级
            if (propertyIdType == PropertyIdType.Study)
            {
                LevelInfo levelInfo = GetPeopleLevelInfo(peopleProto.PropertyData.Level,Mathf.RoundToInt(FindSinglePropertyData(PropertyIdType.Study, peopleProto).PropertyNum));
                peopleProto.PropertyData.Level = levelInfo.canReachLevel;
            }
        }
    }

    /// <summary>
    /// 减少能力值
    /// </summary>
    /// <param name="propertyIdType"></param>
    /// <param name="num"></param>
    public void DeProperty(PropertyIdType propertyIdType, int num,PeopleProtoData peopleProto=null)
    {
        if (peopleProto == null)
        {
            peopleProto = _CurGameInfo.PlayerPeople;
        }
        if (peopleProto.PropertyData.PropertyIdList.Contains((int)propertyIdType))
        {
            int index = peopleProto.PropertyData.PropertyIdList.IndexOf((int)propertyIdType);
            SinglePropertyData singleData = peopleProto.PropertyData.PropertyDataList[index];
            singleData.PropertyNum += num;
            //如果该属性小于0 则等于0
            if (singleData.PropertyNum <= 0)
            {
                singleData.PropertyNum = 0;
            }
        }
    }

    /// <summary>
    /// 这人是不是我的好友
    /// </summary>
    /// <param name="people"></param>
    /// <returns></returns>
    public bool CheckIfMyWetalkFriend(People other, People me)
    {
        for (int i = 0; i < me.weTalkFriends.Count; i++)
        {
            People theFriend = me.weTalkFriends[i];
            if (theFriend.protoData.OnlyId == other.protoData.OnlyId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 根据onlyid找人
    /// </summary>
    /// <returns></returns>
    public People FindPeopleWithOnlyId(UInt64 onlyId)
    {
        for(int i=0;i< allPeopleList.Count; i++)
        {
            People people = allPeopleList[i];
            if (people.protoData.OnlyId == onlyId)
                return people;
        }
        return null;
    }

    /// <summary>
    /// 测试设置
    /// </summary>
    /// <param name="pro"></param>
    public void TestSetProperty(bool isEnemy,int level)
    {
        PropertyData pro=null;
        if (isEnemy)
        {
            //pro = RoleManager.Instance.examPropertyData;
        }
        else
        {
            pro = RoleManager.Instance.playerPeople.protoData.PropertyData;
        }

        
        for (int i = 0; i < pro.CurExamPropertyIdList.Count; i++)
        {
            PropertyIdType idType = (PropertyIdType)pro.CurExamPropertyIdList[i];
            SinglePropertyData singlePro = pro.CurExamPropertyDataList[i];
            switch (idType)
            {
                case PropertyIdType.Attack:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attack.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attack.ToFloat();
                    }
                    break;
                case PropertyIdType.Defense:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].defense.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].defense.ToFloat();
                    }
                    break;
                case PropertyIdType.CritRate:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].critRate.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].critRate.ToFloat();
                    }
                    break;
                case PropertyIdType.CritNum:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].crit.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].crit.ToFloat();
                    }
                    break;
                case PropertyIdType.SkillAdd:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].skillHurtAdd.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].skillHurtAdd.ToFloat();
                    }
                    break;
                case PropertyIdType.Hp:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].hp.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].hp.ToFloat();
                    }
                    break;
                case PropertyIdType.Speed:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attackSpeed.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attackSpeed.ToFloat();
                    }
                    break;
            }
        }

        for (int i = 0; i < pro.ExamPropertyDataList.Count; i++)
        {
            PropertyIdType idType = (PropertyIdType)pro.ExamPropertyIdList[i];
            SinglePropertyData singlePro = pro.ExamPropertyDataList[i];
            switch (idType)
            {
                case PropertyIdType.Attack:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attack.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attack.ToFloat();
                    }
                    break;
                case PropertyIdType.Defense:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].defense.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].defense.ToFloat();
                    }
                    break;
                case PropertyIdType.CritRate:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].critRate.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].critRate.ToFloat();
                    }
                    break;
                case PropertyIdType.CritNum:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].crit.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].crit.ToFloat();
                    }
                    break;
                case PropertyIdType.SkillAdd:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].skillHurtAdd.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].skillHurtAdd.ToFloat();
                    }
                    break;
                case PropertyIdType.Hp:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].hp.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].hp.ToFloat();
                    }
                    break;
                case PropertyIdType.Speed:
                    if (isEnemy)
                    {
                        singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attackSpeed.ToFloat();
                    }
                    else
                    {
                        singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attackSpeed.ToFloat();
                    }
                    break;
            }
        }
    }


    /// <summary>
    /// 获取人物等级数据
    /// </summary>
    public LevelInfo GetPeopleLevelInfo(int curLevel,int curExp)
    {
        //int curLevel=playerPeople.protoData.PropertyData.Level;
        int canReachLevel = curLevel;
        int studyNum = curExp;// Mathf.RoundToInt(RoleManager.Instance.FindSinglePropertyData(PropertyIdType.Study).PropertyNum);
        
        int studyNumAfterAllUpgrade=0;

        //int curLevel = 1;
        if (canReachLevel < DataTable._peopleUpgradeList.Count)
        {     
            
            //升到这一级用掉了多少学习数量
            for (int i = 1; i < canReachLevel; i++)
            {
                int theNum = DataTable._peopleUpgradeList[i].needExp.ToInt32();
                studyNum -= theNum;

            }
            studyNumAfterAllUpgrade = studyNum;
            for (int i = canReachLevel; i < DataTable._peopleUpgradeList.Count; i++)
            {

                PeopleUpgradeSetting nextSetting = DataTable._peopleUpgradeList[i];
                int nextLevelNeed = nextSetting.needExp.ToInt32();
                //就在这个等级了
                if (studyNumAfterAllUpgrade < nextLevelNeed)
                {
                    break;
                }
                else
                {
                    canReachLevel++;
                    studyNumAfterAllUpgrade -= nextLevelNeed;
                }
            }
        }
        LevelInfo info = new LevelInfo(canReachLevel, curLevel, studyNum, studyNumAfterAllUpgrade);

        return info;
    }
}

public class LevelInfo
{
    public int canReachLevel;//能达到哪一级
    public int curLevel;//当前哪一级
    public int curExp;//当前经验值
    public int curExpAfterAllUpgrade;//生完所有级后剩余的经验值

    public LevelInfo(int canReachLevel,int curLevel,int curExp,int curExpAfterAllUpgrade)
    {
        this.canReachLevel = canReachLevel;
        this.curLevel = curLevel;
        this.curExp = curExp;
        this.curExpAfterAllUpgrade = curExpAfterAllUpgrade;
    }
}
