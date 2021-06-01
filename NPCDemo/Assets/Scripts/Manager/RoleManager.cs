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
    /// 初始化属性
    /// </summary>
    public void InitProperty(PropertyData propertyData)
    {

        propertyData.ExamPropertyDataList.Clear();
        propertyData.ExamPropertyIdList.Clear();
        propertyData.PropertyIdList.Clear();
        propertyData.PropertyDataList.Clear();

        ///先把初始随机的属性赋值
        for(int i = 0; i < DataTable._propertyList.Count; i++)
        {
            PropertySetting setting = DataTable._propertyList[i];

   
            //若是初始随机的属性，则按照配置表随机一个值
            if (setting.isInitRdm == "1")
            {
                SinglePropertyData singlePro = new SinglePropertyData();
                singlePro.PropertyLimit = setting.haveLimit.ToInt32();
                singlePro.PropertyId = setting.id.ToInt32();
                string[] rdmRange = setting.newRdmRange.Split('|');
                int val = RandomManager.Next(rdmRange[0].ToInt32(), rdmRange[1].ToInt32());
                propertyData.PropertyIdList.Add(setting.id.ToInt32());
                singlePro.PropertyNum = val;

                propertyData.PropertyDataList.Add(singlePro);
            }
        }
        RefreshProperty(propertyData);
      
    }

    /// <summary>
    /// 刷新属性升级后需要调用
    /// </summary>
    void RefreshProperty(PropertyData propertyData)
    {
        //学习
        TestNumerialSetting studySetting = DataTable.FindTestNumerialByLevel(propertyData.Level);
        List<List<float>> studyProChange = CommonUtil.Split2CfgFloat(studySetting.proChange);
        InitPropertyChangeWithNumerial(propertyData, studyProChange);


        //体育
        PhysicalUpgradeNumerialSetting physicalSetting = DataTable.FindPhysicalUpgradeNumerialByLevel(propertyData.PhysicalLevel);
        List<List<float>> physicalProChange = CommonUtil.Split2CfgFloat(physicalSetting.proChange);
        InitPropertyChangeWithNumerial(propertyData, physicalProChange);

        //艺术
        ArtUpgradeNumerialSetting artSetting = DataTable.FindArtUpgradeNumerialByLevel(propertyData.ArtLevel);
        List<List<float>> artProChange = CommonUtil.Split2CfgFloat(artSetting.proChange);
        InitPropertyChangeWithNumerial(propertyData, artProChange);
    }
    /// <summary>
    /// 通过从配置表解析出来的结构改变属性
    /// </summary>
    /// <param name="pro"></param>
    /// <param name="proChange"></param>
    public void InitPropertyChangeWithNumerial(PropertyData propertyData, List<List<float>> proChange)
    {
        for (int i = 0; i < proChange.Count; i++)
        {
            List<float> single = proChange[i];
            int theId = (int)single[0];
            float theNum = single[1];
            PropertySetting setting = DataTable.FindPropertySetting(theId);

            SinglePropertyData singlePro = new SinglePropertyData();
            singlePro.PropertyLimit = setting.haveLimit.ToInt32();
            singlePro.PropertyId = theId;
            ////若不是随等级变化，则按照配置表随机一个值
            //if (setting.isChangeWithLevel != "1")
            //{
            //    string[] rdmRange = setting.newRdmRange.Split('|');
            //    int val = RandomManager.Next(rdmRange[0].ToInt32(), rdmRange[1].ToInt32());
            //    theNum = val;
            //}
            singlePro.PropertyNum = theNum;
            if (setting.isExamBattle == "1")
            {
                if (propertyData.ExamPropertyIdList.Contains(theId))
                {
                    int index = propertyData.ExamPropertyIdList.IndexOf(theId);
                    propertyData.ExamPropertyDataList[index].PropertyNum = theNum;
                }
                else
                {
                    propertyData.ExamPropertyIdList.Add(theId);
                    propertyData.ExamPropertyDataList.Add(singlePro);
                }
          
            }
            else
            {
                if (propertyData.PropertyIdList.Contains(theId))
                {
                    int index = propertyData.PropertyIdList.IndexOf(theId);
                    propertyData.PropertyDataList[index].PropertyNum = theNum;
                }
                else
                {
                    propertyData.PropertyIdList.Add(theId);
                    propertyData.PropertyDataList.Add(singlePro);
                }
          
            }
        }
    }

    /// <summary>
    /// 初始化人物属性
    /// </summary>
    /// <param name="propertyData"></param>
    /// <param name="idType"></param>
    /// <param name="propertyNum"></param>
    public void InitSingleProperty(PropertyData propertyData, PropertyIdType idType,float propertyNum,bool isExamBattle)
    {
        PropertySetting setting = DataTable.FindPropertySetting((int)idType);

        //PropertyData propertyData = new PropertyData();
        //如果属性为-1 说明是考试属性 从property表读取
        if (isExamBattle)
        {
            //TestEnemyNumerialSetting examSetting = DataTable.FindTestNumerial((int)idType));
            ////考试数值需要和等级挂钩
            //SinglePropertyData examPro = new SinglePropertyData();
            //examPro.PropertyId = (int)idType;
            //examPro.PropertyNum = propertyNum;
            //examPro.PropertyLimit = -1;

            //curExamProperty不应该在这里赋值 而应该在考试时赋值
            //propertyData.CurExamPropertyIdList.Add((int)idType);
            //propertyData.CurExamPropertyDataList.Add(examPro);

            if (propertyData.ExamPropertyIdList.Contains((int)idType))
            {
                int index = propertyData.ExamPropertyIdList.IndexOf((int)idType);
                propertyData.ExamPropertyDataList[index].PropertyNum = propertyNum;
            }
            else
            {
                SinglePropertyData initsinglePropertyData = new SinglePropertyData();
                initsinglePropertyData.PropertyId = (int)idType;
                initsinglePropertyData.PropertyNum = propertyNum;
                initsinglePropertyData.PropertyLimit = -1;

                propertyData.ExamPropertyIdList.Add((int)idType);
                propertyData.ExamPropertyDataList.Add(initsinglePropertyData);
            }
    

        }
        else
        {
            if (propertyData.PropertyIdList.Contains((int)idType))
            {
                int index = propertyData.PropertyIdList.IndexOf((int)idType);
                propertyData.PropertyDataList[index].PropertyNum = propertyNum;
            }
            else
            {
                SinglePropertyData singlePropertyData = new SinglePropertyData();
                singlePropertyData.PropertyId = (int)idType;
                singlePropertyData.PropertyNum = propertyNum;
                singlePropertyData.PropertyLimit = setting.haveLimit.ToInt32();

                propertyData.PropertyIdList.Add((int)idType);

                propertyData.PropertyDataList.Add(singlePropertyData);
            }

          
        }
        //return singlePropertyData;
    }

    ///// <summary>
    ///// 学习升级
    ///// </summary>
    //void OnUpgrade()
    //{
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.Hp, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).hp.ToFloat(), true);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.Attack, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).attack.ToFloat(), true);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.Defense, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).defense.ToFloat(), true);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.CritNum, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).crit.ToFloat(), true);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.Speed, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).attackSpeed.ToFloat(), true);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.CritRate, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).critRate.ToFloat(), true);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.SkillAdd, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).skillHurtAdd.ToFloat(), true);

    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.StudyCharm, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).studyCharm.ToFloat(), false);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.StudyDefense, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).studyDefense.ToFloat(), false);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.PhysicalCharm, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).physicalCharm.ToFloat(), false);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.PhysicalDefense, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).physicalDefense.ToFloat(), false);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.ArtCharm, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).artCharm.ToFloat(), false);
    //    InitSingleProperty(playerPeople.protoData.PropertyData, PropertyIdType.ArtDefense, DataTable.FindTestNumerialByLevel(playerPeople.protoData.PropertyData.Level).artDefense.ToFloat(), false);
    //}

    /// <summary>
    /// 得到学习分数
    /// </summary>
    public void GetStudyScore()
    {
        //CurScore += 30;
        //int init
        PropertySetting pro = DataTable.FindPropertySetting((int)PropertyIdType.Study);
        //AddProperty(PropertyIdType.Study, 30);
        EventCenter.Broadcast(TheEventType.GetStudyScore, "获得"+ pro.name);
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
    /// <summary>
    /// 获取显示在面板上的属性
    /// </summary>
    /// <returns></returns>
    public List<SinglePropertyData> FindCommonPropertyDataList()
    {
        List<SinglePropertyData> proList = new List<SinglePropertyData>();
        for(int i = 0; i < playerPeople.protoData.PropertyData.PropertyDataList.Count; i++)
        {
            SinglePropertyData pro = playerPeople.protoData.PropertyData.PropertyDataList[i];
            proList.Add(pro);
        }
        return proList;
    }
    /// <summary>
    /// 获取显示在面板上的战斗属性
    /// </summary>
    /// <returns></returns>
    public List<SinglePropertyData> FindExamPropertyDataList()
    {
        List<SinglePropertyData> proList = new List<SinglePropertyData>();
        for (int i = 0; i < playerPeople.protoData.PropertyData.ExamPropertyDataList.Count; i++)
        {
            SinglePropertyData pro = playerPeople.protoData.PropertyData.ExamPropertyDataList[i];
            proList.Add(pro);
        }
        return proList;
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
    /// 初始化战斗属性
    /// </summary>
    public void InitBattleProperty()
    {
        int count =playerPeople.protoData.PropertyData.CurExamPropertyIdList.Count;
        for(int i = 0; i < count; i++)
        {
            SinglePropertyData curPro = playerPeople.protoData.PropertyData.CurExamPropertyDataList[i];
            SinglePropertyData rawPro= playerPeople.protoData.PropertyData.ExamPropertyDataList[i];
            curPro.PropertyNum = rawPro.PropertyNum;
        }
    }

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
            float realAdd = num;
            //如果该属性存在最大限制
            if (limit >= 0)
            {
                if (realAdd + singleData.PropertyNum >= limit)
                {
                    realAdd = limit - singleData.PropertyNum;
                }
                
            }

            //如果是经验 则增加等级
            switch (propertyIdType)
            {
                case PropertyIdType.Study:
                    LevelInfo levelInfo = GetPeopleLevelInfo(Mathf.RoundToInt(realAdd));
                    peopleProto.PropertyData.CurExp = levelInfo.ExpAfterUpgrade;
                    peopleProto.PropertyData.Level = levelInfo.canReachLevel;
                    break;                               
                case PropertyIdType.Physical:
                    LevelInfo physicalLevelInfo = GetPeoplePhysicalLevelInfo(Mathf.RoundToInt(realAdd));
                    peopleProto.PropertyData.CurPhysicalExp = physicalLevelInfo.ExpAfterUpgrade;
                    peopleProto.PropertyData.PhysicalLevel = physicalLevelInfo.canReachLevel;
                    break;
                case PropertyIdType.Art:
                    LevelInfo artLevelInfo = GetPeopleArtLevelInfo(Mathf.RoundToInt(realAdd));
                    peopleProto.PropertyData.CurArtExp = artLevelInfo.ExpAfterUpgrade;
                    peopleProto.PropertyData.ArtLevel = artLevelInfo.canReachLevel;
                    break;
                default:
                    break;
            }
     
            singleData.PropertyNum += realAdd;
            RefreshProperty(peopleProto.PropertyData);

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
    /// 获得属性并结算
    /// </summary>
    public void GetAwardAndResult(List<AwardData> awardDataList,Action cb)
    {
        //升级前
        LevelInfo levelInfo = null;
        for (int i = 0; i < awardDataList.Count; i++)
        {
            AwardData data = awardDataList[i];
            if (data.awardType == AwardType.Property) 
            {
                int awardId = data.id;
                int awardNum = data.num;
      
                if (awardId == (int)PropertyIdType.Study)
                {
                    levelInfo = RoleManager.Instance.GetPeopleLevelInfo(awardNum);
                }
                RoleManager.Instance.AddProperty((PropertyIdType)awardId, awardNum);
            }
        }
        PanelManager.Instance.OpenPanel<GetAwardPanel>(PanelManager.Instance.trans_layer2, awardDataList, cb, levelInfo);
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

        
        //for (int i = 0; i < pro.CurExamPropertyIdList.Count; i++)
        //{
        //    PropertyIdType idType = (PropertyIdType)pro.CurExamPropertyIdList[i];
        //    SinglePropertyData singlePro = pro.CurExamPropertyDataList[i];
        //    switch (idType)
        //    {
        //        case PropertyIdType.Attack:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attack.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attack.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.Defense:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].defense.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].defense.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.CritRate:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].critRate.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].critRate.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.CritNum:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].crit.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].crit.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.SkillAdd:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].skillHurtAdd.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].skillHurtAdd.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.Hp:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].hp.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].hp.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.Speed:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attackSpeed.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attackSpeed.ToFloat();
        //            }
        //            break;
        //    }
        //}

        //for (int i = 0; i < pro.ExamPropertyDataList.Count; i++)
        //{
        //    PropertyIdType idType = (PropertyIdType)pro.ExamPropertyIdList[i];
        //    SinglePropertyData singlePro = pro.ExamPropertyDataList[i];
        //    switch (idType)
        //    {
        //        case PropertyIdType.Attack:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attack.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attack.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.Defense:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].defense.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].defense.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.CritRate:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].critRate.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].critRate.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.CritNum:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].crit.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].crit.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.SkillAdd:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].skillHurtAdd.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].skillHurtAdd.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.Hp:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].hp.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].hp.ToFloat();
        //            }
        //            break;
        //        case PropertyIdType.Speed:
        //            if (isEnemy)
        //            {
        //                singlePro.PropertyNum = DataTable._testEnemyNumerialList[level - 1].attackSpeed.ToFloat();
        //            }
        //            else
        //            {
        //                singlePro.PropertyNum = DataTable._testNumerialList[level - 1].attackSpeed.ToFloat();
        //            }
        //            break;
        //    }
        //}
    }


    /// <summary>
    /// 获取人物等级数据
    /// </summary>
    public LevelInfo GetPeopleLevelInfo(int addExp)
    {
        //int curLevel=playerPeople.protoData.PropertyData.Level;
        int beforeExp = playerPeople.protoData.PropertyData.CurExp;
        int beforeLevel = playerPeople.protoData.PropertyData.Level;
        int canReachLevel = beforeLevel;
        int expAfterAllUpgrade = beforeExp + addExp;

        //int curLevel = 1;
        if (canReachLevel < DataTable._testNumerialList.Count)
        {     
            
            for (int i = canReachLevel; i < DataTable._testNumerialList.Count; i++)
            {

                TestNumerialSetting nextSetting = DataTable._testNumerialList[i];
                int nextLevelNeed = nextSetting.needExp.ToInt32();
                //就在这个等级了
                if (expAfterAllUpgrade < nextLevelNeed)
                {
                    break;
                }
                else
                {
                    canReachLevel++;
                    expAfterAllUpgrade -= nextLevelNeed;
                }
            }
        }
       LevelInfo info = new LevelInfo(canReachLevel, expAfterAllUpgrade,beforeExp,beforeLevel);

        return info;
    }

    /// <summary>
    /// 获取人物体育等级数据
    /// </summary>
    public LevelInfo GetPeoplePhysicalLevelInfo(int addExp)
    {
        //int curLevel=playerPeople.protoData.PropertyData.Level;
        int beforeExp = playerPeople.protoData.PropertyData.CurPhysicalExp;
        int beforeLevel = playerPeople.protoData.PropertyData.PhysicalLevel;
        int canReachLevel = beforeLevel;
        int expAfterAllUpgrade = beforeExp + addExp;

        //int curLevel = 1;
        if (canReachLevel < DataTable._testNumerialList.Count)
        {

            for (int i = canReachLevel; i < DataTable._testNumerialList.Count; i++)
            {

                TestNumerialSetting nextSetting = DataTable._testNumerialList[i];
                int nextLevelNeed = nextSetting.needExp.ToInt32();
                //就在这个等级了
                if (expAfterAllUpgrade < nextLevelNeed)
                {
                    break;
                }
                else
                {
                    canReachLevel++;
                    expAfterAllUpgrade -= nextLevelNeed;
                }
            }
        }
        LevelInfo info = new LevelInfo(canReachLevel, expAfterAllUpgrade, beforeExp, beforeLevel);

        return info;
    }

    /// <summary>
    /// 获取人物艺术等级数据
    /// </summary>
    public LevelInfo GetPeopleArtLevelInfo(int addExp)
    {
        //int curLevel=playerPeople.protoData.PropertyData.Level;
        int beforeExp = playerPeople.protoData.PropertyData.CurArtExp;
        int beforeLevel = playerPeople.protoData.PropertyData.ArtLevel;
        int canReachLevel = beforeLevel;
        int expAfterAllUpgrade = beforeExp + addExp;

        //int curLevel = 1;
        if (canReachLevel < DataTable._testNumerialList.Count)
        {

            for (int i = canReachLevel; i < DataTable._testNumerialList.Count; i++)
            {

                TestNumerialSetting nextSetting = DataTable._testNumerialList[i];
                int nextLevelNeed = nextSetting.needExp.ToInt32();
                //就在这个等级了
                if (expAfterAllUpgrade < nextLevelNeed)
                {
                    break;
                }
                else
                {
                    canReachLevel++;
                    expAfterAllUpgrade -= nextLevelNeed;
                }
            }
        }
        LevelInfo info = new LevelInfo(canReachLevel, expAfterAllUpgrade, beforeExp, beforeLevel);

        return info;
    }
}

public class LevelInfo
{
    public int canReachLevel;//能达到哪一级
    public int ExpAfterUpgrade;//生完所有级后剩余的经验值
    public int beforeExp;//之前的经验
    public int beforeLevel;//之前的等级
    public LevelInfo(int canReachLevel, int ExpAfterUpgrade,int beforeExp,int beforeLevel)
    {
        this.canReachLevel = canReachLevel;
        this.ExpAfterUpgrade = ExpAfterUpgrade;
        this.beforeExp = beforeExp;
        this.beforeLevel = beforeLevel;
    }
}
