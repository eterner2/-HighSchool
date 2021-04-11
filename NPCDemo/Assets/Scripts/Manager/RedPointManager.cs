using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPointManager:CommonInstance<RedPointManager>
{

    List<RedPoint> RedPointList = new List<RedPoint>();
    Dictionary<RedPointType,List<RedPoint>> RedPointDic = new Dictionary<RedPointType, List<RedPoint>>();

    /// <summary>
    /// 初始化红点
    /// </summary>
    public override void Init()
    {
        base.Init();

    }

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        RedPointDic = new Dictionary<RedPointType, List<RedPoint>>();
    }

    /// <summary>
    /// dic里面增加一种红点
    /// </summary>
    public RedPoint AddRedPointToDic(RedPointType redPointType, UInt64 id)
    {
        RedPoint point = new RedPoint();
        point.id = id;
        point.redPointType = redPointType;
        if (!RedPointDic.ContainsKey(redPointType))
        {
            RedPointDic.Add(redPointType, new List<RedPoint>());
        }
        RedPointDic[redPointType].Add(point);
        //AddRedPointToDic()
        return point;
    }

    /// <summary>
    /// 从红点dic里面取到一个红点
    /// </summary>
    /// <returns></returns>
    public RedPoint GetRedPointFromDic(RedPointType redPointType, UInt64 id)
    {
        if (!RedPointDic.ContainsKey(redPointType))
        {
            return AddRedPointToDic(redPointType, id);
        }
        else
        {
            //字典该类有这个id
            bool haveSameId = false;
            List<RedPoint> redPointList = RedPointDic[redPointType];
            for (int i = 0; i < redPointList.Count; i++)
            {
                RedPoint thePoint = redPointList[i];
                UInt64 theId = thePoint.id;
                if (id == theId)
                {
                    haveSameId = true;
                    return thePoint;
                }
            }
            //字典该类没有id
            if (!haveSameId)
            {
                return AddRedPointToDic(redPointType, id);
            }
        }
        return null;

    }

    /// <summary>
    /// 绑定两个红点
    /// </summary>
    public void BindRedPoint(RedPoint parent,RedPoint sun)
    {
        if(!parent.sunList.Contains(sun))   
            parent.sunList.Add(sun);
        sun.Parent = parent;
    }

    /// <summary>
    /// 改变某个红点状态
    /// </summary>
    public void ChangeRedPointStatus(RedPointType redPointType, UInt64 id,bool status)
    {
        if (RedPointDic.ContainsKey(redPointType))
        {
            List<RedPoint> redPointList = RedPointDic[redPointType];
            for(int i = 0; i < redPointList.Count; i++)
            {
                RedPoint thePoint = redPointList[i];
                UInt64 theId = thePoint.id;
                if (id == theId)
                {
                    thePoint.nodeVal = status;
                    UpdateStatus(thePoint);
                }
            }
        }
        
    }

    /// <summary>
    /// 设置ui的显示
    /// </summary>
    /// <param name="obj"></param>
    /// <param name=""></param>
    public void SetRedPointUI(GameObject obj,RedPointType redPointType, UInt64 id)
    {
        if (RedPointDic.ContainsKey(redPointType))
        {
            List<RedPoint> redPointList = RedPointDic[redPointType];
            for (int i = 0; i < redPointList.Count; i++)
            {
                RedPoint thePoint = redPointList[i];
                UInt64 theId = thePoint.id;
                if (id == theId)
                {
                    obj.gameObject.SetActive(thePoint.nodeVal);
                }
            }
        }
        else
        {
            Debug.Log("字典没有注册该红点就永不显示" + redPointType);
            obj.gameObject.SetActive(false);
        }
    }



    /// <summary>
    /// 更新红点状态,根据子物体改变它的父物体
    /// </summary>
    public void UpdateStatus(RedPoint redPoint)
    {    
        if (redPoint == null)
            return;

        int sunCount = redPoint.sunList.Count;
        if (sunCount != 0)
        {
            bool val = false;
            for (int i = sunCount - 1; i >= 0; i--)
            {
                RedPoint sunPoint = redPoint.sunList[i];
                if (sunPoint.nodeVal)
                {
                    val = true;
                    break;
                }
            }
            redPoint.nodeVal = val;
        }

        UpdateUI(redPoint);

        if (redPoint.Parent != null)
        {
            UpdateStatus(redPoint.Parent);
        }
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    /// <param name="redPoint"></param>
    public void UpdateUI(RedPoint redPoint)
    {
        if (redPoint.PointObj != null)
            redPoint.PointObj.SetActive(redPoint.nodeVal);
    }



    ///// <summary>
    ///// 改变状态
    ///// </summary>
    //public void ChangeRedPointStatus(RedPoint redPoint,bool change)
    //{
    //    if (redPoint.PointObj != null)
    //        redPoint.PointObj.SetActive(change);
    //}

}

public class RedPoint
{
    public UInt64 id;//id
    //public UInt64 onlyId;//唯一id
    public List<RedPoint> sunList = new List<RedPoint>();//儿子红点
    public RedPointType redPointType;//任务类型
    public bool nodeVal { get; set; }//红点显示与否
   public GameObject PointObj { get; set; }//红点物体
   public RedPoint Parent { get; set; }//父红点
   //public Dictionary<int, RedPoint> SonDic { get; set; }//儿子红点
   public Action check;//检查

}

public enum RedPointType
{
    None=0,
    CellPhone=1,//手机
    AllChatMsg,//所有聊天信息
    SinglePeopleChatMsg,//单个人的聊天信息
    SingleChatMsg,//单个聊天信息
}