using Framework.Data;
using RoleData;
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
        CreateNewTimeData(gameInfo);
        CreateNewPlayer(gameInfo);
        //CreateNewPropertyData(gameInfo);

        _CurGameInfo = gameInfo;
    }

    /// <summary>
    /// 创建新的日期数据
    /// </summary>
    void CreateNewTimeData(GameInfo gameInfo)
    {
        TimeData timeData = new TimeData();
        timeData.Year = 1;
        timeData.Month = 9;
        timeData.TheWeekDay = 3;
        gameInfo.TimeData = timeData;
    }
    /// <summary>
    /// 创建新的玩家
    /// </summary>
    void CreateNewPlayer(GameInfo gameInfo)
    {
        PeopleProtoData peopleProtoData = new PeopleProtoData();
        CreateNewPropertyData(peopleProtoData);
        gameInfo.PlayerPeople = peopleProtoData;
        gameInfo.AllPeopleList.Add(peopleProtoData);
    }

    /// <summary>
    /// 创建新的属性数据
    /// </summary>
    /// <param name="gameInfo"></param>
    void CreateNewPropertyData(PeopleProtoData peopleProtoData)
    {
        PropertyData propertyData = new PropertyData();
        propertyData.PropertyIdList.Add((int)PropertyIdType.Study);
        propertyData.PropertyNumList.Add(0);
        peopleProtoData.PropertyData = propertyData;
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
    /// 获取能力值
    /// </summary>
    public int FindPropertyNum(PropertyIdType propertyIdType)
    {
        //int studyId=ConstantVal
        if (_CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.Contains((int)propertyIdType))
        {
            int index= _CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.IndexOf((int)propertyIdType);
            return _CurGameInfo.PlayerPeople.PropertyData.PropertyNumList[index];
        }
        return 0;
    }

    /// <summary>
    /// 增加能力值
    /// </summary>
    /// <param name="propertyIdType"></param>
    /// <param name="num"></param>
    public void AddProperty(PropertyIdType propertyIdType, int num)
    {
        if (_CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.Contains((int)propertyIdType))
        {
            int index = _CurGameInfo.PlayerPeople.PropertyData.PropertyIdList.IndexOf((int)propertyIdType);
            _CurGameInfo.PlayerPeople.PropertyData.PropertyNumList[index] += num;
        }
    }
}
