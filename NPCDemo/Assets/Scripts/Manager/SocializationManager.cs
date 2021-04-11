using Framework.Data;
using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocializationManager : CommonInstance<SocializationManager>
{
    public List<Plan> planList = new List<Plan>();
    public Dictionary<int, List<Plan>> action_planDic=new Dictionary<int, List<Plan>>();//某个行为的计划列表字典 

    public override void Init()
    {
        base.Init();
        RedPoint son = RedPointManager.Instance.GetRedPointFromDic(RedPointType.AllChatMsg, 0);
        RedPoint parent = RedPointManager.Instance.GetRedPointFromDic(RedPointType.CellPhone, 0);
        RedPointManager.Instance.BindRedPoint(parent, son);
    }
    /// <summary>
    /// 开始新邀约事件
    /// </summary>
    public void StartNewInvite()
    {
        ClearAllPlan();
        InviteCalc();
    }

    /// <summary>
    /// 清理所有计划
    /// </summary>
    public void ClearAllPlan()
    {
        for(int i = 0; i < RoleManager.Instance.allPeopleList.Count; i++)
        {
            RoleManager.Instance.allPeopleList[i].Clear();
        }
        planList.Clear();
        action_planDic.Clear();
    }

    public void InviteCalc()
    {
        //定计划
        for (int i = 0; i <RoleManager.Instance.NPCPeopleList.Count; i++)
        {
            People me = RoleManager.Instance.NPCPeopleList[i];
            List<People> tmpList = new List<People>();

            //tmpList.Clear();
           List<ActionSetting> actionSettingList=  DataTable._actionList;
            int actionIndex = RandomManager.Next(0, actionSettingList.Count);
            ActionSetting actionSetting = actionSettingList[actionIndex];
            me.actionName = actionSetting.name;
            me.protoData.ActionId = actionSetting.id.ToInt32();
            //邀请0-5人(不能重复邀请
            if (me.weTalkFriends.Count > 0)
            {
                int num = RandomManager.Next(0, me.weTalkFriends.Count);
                if (num > 0)
                {
                    for (int j = 0; j < num; j++)
                    {
                        People choosed = null;
                        while (choosed == null
                            || choosed == me
                            || tmpList.Contains(choosed))
                        {
                            int index = RandomManager.Next(0, me.weTalkFriends.Count);
                            choosed = me.weTalkFriends[index];
                        }
                        tmpList.Add(choosed);
                        if (choosed.isPlayer)
                        {
                            //SendMsgToPlayer(me, choosed, actionSetting);TODO
                        }
                        me.Invite(choosed);
                        me.Record("邀请" + choosed.protoData.Name + me.actionName);
                        choosed.Record("被" + me.protoData.Name + "邀请" + me.actionName);

                    }
                }
                else
                {
                    me.Record("想今天一个人" + me.actionName);
                }
            }
          
            else
            {
                //planList.Add(new Plan(me.actionName, me, null));
                me.Record("想今天一个人" + me.actionName);
            }
        }

        //这里如果玩家有被邀请，则发一封信给玩家

    }

    /// <summary>
    /// 处理邀约
    /// </summary>
    public void HandleInvite(List<People> peopleList)
    {
        List<People> nextHandleList = new List<People>();
        //处理邀约
        for (int i = 0; i < peopleList.Count; i++)
        {
            bool finishMatch = false;
            People p = peopleList[i];
            if (p.finishInviteProcess)
                continue;
            List<People> candidateList = new List<People>();//所有可能配对的人的列表
            List<People> candidateInviteMeList = new List<People>();//邀请我的人的列表
            List<People> candidateMeInviteOtherList = new List<People>();//我邀请的人的列表
            //先检查是不是我邀请的人里面也有同时邀请我的
            for (int j = 0; j < p.otherInviteMeList.Count; j++)
            {
                //邀请我的其中一个
                People other = p.otherInviteMeList[j].people;
                if (other.finishInviteProcess)
                    continue;


                List<People> meInviteOtherPeopleList = new List<People>();
                for (int m = 0; m < p.meInviteOtherList.Count; m++)
                {
                    meInviteOtherPeopleList.Add(p.meInviteOtherList[m].people);
                }
                //如果我邀请的人也邀请我做同样的事 并且他没配对成功 直接配对成功 有多对则选第一对
                if (meInviteOtherPeopleList.Contains(other) && !other.finishInviteProcess)
                {
                    if (p.actionName == other.actionName)
                    {
                        p.Record("在我的回合发现" + other.name + "也邀请我" + p.actionName + ",感到十分高兴，于是一起" + p.actionName);
                        other.Record("在" + p.name + "的回合" + "发现" + p.name + "也邀请我" + other.actionName + ",感到十分高兴，于是一起" + other.actionName);
                        //planList.Add(new Plan(p.actionName, p, other));
                        AddPlan(p.actionName, p.protoData.ActionId, p, other);
                        //分别拒绝掉各自的鱼
                        RefusePeopleWhoInviteMe(p, other, p.actionName);
                        ForgivePeopleWhoIInvite(p, other, p.actionName);

                        RefusePeopleWhoInviteMe(other, p, p.actionName);
                        ForgivePeopleWhoIInvite(other, p, p.actionName);
                    }
                    else
                    {
                        //迁就谁
                        //string actionName = "";
                        int theIndex = RandomManager.Next(0, 2);
                        if (theIndex == 0)
                        {
                            p.Record("在我的回合发现" + other.name + "邀请我" + other.actionName + ",商量后决定听自己的，一起" + p.actionName);
                            other.Record("在" + p.name + "的回合" + "发现" + p.name + "邀请我" + p.actionName + ",商量后决定听对方的，于是一起" + p.actionName);
                            //planList.Add(new Plan(p.actionName, p, other));
                            AddPlan(p.actionName, p.protoData.ActionId, p, other);
                            //分别拒绝掉各自的鱼
                            RefusePeopleWhoInviteMe(p, other, p.actionName);
                            ForgivePeopleWhoIInvite(p, other, p.actionName);

                            RefusePeopleWhoInviteMe(other, p, p.actionName);
                            ForgivePeopleWhoIInvite(other, p, p.actionName);
                        }
                        else
                        {
                            p.Record("在我的回合发现" + other.name + "邀请我" + other.actionName + ",商量后决定听对方的，于是一起" + other.actionName);
                            other.Record("在" + p.name + "的回合" + "发现" + p.name + "邀请我" + p.actionName + ",商量后决定听自己的，一起" + other.actionName);
                            //planList.Add(new Plan(other.actionName, p, other));
                            AddPlan(other.actionName, other.protoData.ActionId, p, other);
                            //分别拒绝掉各自的鱼
                            RefusePeopleWhoInviteMe(p, other, other.actionName);
                            ForgivePeopleWhoIInvite(p, other, other.actionName);

                            RefusePeopleWhoInviteMe(other, p, other.actionName);
                            ForgivePeopleWhoIInvite(other, p, other.actionName);
                        }

                    }
                    finishMatch = true;
                    break;
                }
                //否则 把邀请我的其中一个加入列表
                candidateList.Add(other);
                candidateInviteMeList.Add(other);
            }
            if (!finishMatch)
            {
                //从我邀请的人和邀请我的人里面选一个，若选择了我邀请的人，则把邀请我的人全部拒绝掉 若选择了邀请我的人，则配对成功 当不存在我邀请的人，或我邀请的人全部拒绝了我，或邀请我的人已和别人配对成功 则我一个人活动

                //我邀请的人(没拒绝我的）加入候选列表
                for (int m = 0; m < p.meInviteOtherList.Count; m++)
                {
                    People meInvitedOther = p.meInviteOtherList[m].people;
                    if (!meInvitedOther.finishInviteProcess && !p.meInviteOtherList[m].refused)
                    {
                        candidateMeInviteOtherList.Add(meInvitedOther);
                        candidateList.Add(meInvitedOther);
                    }
                }

                //既没有邀请我的，也没有我邀请的
                if (candidateList.Count == 0)
                {
                    p.Record("决定独自去" + p.actionName);
                    //planList.Add(new Plan(p.actionName, p, null));
                    AddPlan(p.actionName, p.protoData.ActionId, p);
                    continue;
                }
                //如果没有我邀请的人 但有邀请我的人
                else if (candidateMeInviteOtherList.Count == 0 && candidateInviteMeList.Count > 0)
                {
                    string content = "";
                    int rdmIndex = RandomManager.Next(0, candidateInviteMeList.Count);
                    AddPlan(candidateInviteMeList[rdmIndex].actionName, candidateInviteMeList[rdmIndex].protoData.ActionId, p, candidateInviteMeList[rdmIndex]);
                    //planList.Add(new Plan(candidateInviteMeList[rdmIndex].actionName, p, candidateInviteMeList[rdmIndex]));
                    content = "决定和" + candidateInviteMeList[rdmIndex].name + "一起" + candidateInviteMeList[rdmIndex].actionName;
                    //不止一个人邀请
                    if (candidateInviteMeList.Count > 1)
                    {
                        content += ",并拒绝了";
                    }
                    for (int j = 0; j < candidateInviteMeList.Count; j++)
                    {
                        if (j != rdmIndex)
                        {
                            content += candidateInviteMeList[j].name + "，";
                            candidateInviteMeList[j].Record("由于" + candidateInviteMeList[rdmIndex].name + "也邀请了" + p.name + ","
                                + p.name + "选择和" + candidateInviteMeList[rdmIndex].name + "一起" + candidateInviteMeList[rdmIndex].actionName
                                + ",拒绝了你的邀请");
                            p.Refuse(candidateInviteMeList[j]);
                        }

                    }

                    p.Record(content);
                    //ForgivePeopleWhoIInvite(p,)
                    candidateInviteMeList[rdmIndex].Record(p.name + "答应了邀请," + "一起" + candidateInviteMeList[rdmIndex].actionName);
                    //我选的这个人还要拒绝掉其它邀请了他的人
                    RefusePeopleWhoInviteMe(candidateInviteMeList[rdmIndex], p, candidateInviteMeList[rdmIndex].actionName);
                    //我选的这个人还要对他邀请的人说不去了
                    ForgivePeopleWhoIInvite(candidateInviteMeList[rdmIndex], p, candidateInviteMeList[rdmIndex].actionName);
                }
                //如果没有邀请我的人 但有我邀请的人
                else if (candidateInviteMeList.Count == 0 && candidateMeInviteOtherList.Count > 0)
                {
                    nextHandleList.Add(p);

                }
                //如果两者都有
                else if (candidateInviteMeList.Count > 0 && candidateMeInviteOtherList.Count > 0)
                {
                    int index = RandomManager.Next(0, candidateList.Count);
                    People choosed = candidateList[index];
                    //如果选择了我邀请的人，则拒绝所有邀请我的人 然后进入下一轮
                    if (p.ifMeInvitePeople(choosed))
                    {
                        //如果有人邀请我 则婉拒他
                        if (candidateInviteMeList.Count > 0)
                        {
                            string content = "";
                            content = "还是想和" + choosed.name + "一起" + p.actionName + ",所以拒绝了";

                            for (int j = 0; j < candidateInviteMeList.Count; j++)
                            {
                                content += candidateInviteMeList[j].name + "，";
                                candidateInviteMeList[j].Record("由于"
                                    + p.name + "还是想和" + choosed.name + "一起" + p.actionName
                                    + ",拒绝了你的邀请");

                                p.Refuse(candidateInviteMeList[j]);
                            }

                            p.Record(content);
                        }
                        nextHandleList.Add(p);
                    }
                    //如果选择了邀请我的人，则直接答应 并拒绝其他邀请我的人
                    else
                    {
                        AddPlan(choosed.actionName, choosed.protoData.ActionId, p, choosed);
                        //planList.Add(new Plan(choosed.actionName, p, choosed));
                        //有人邀请我 则婉拒他
                        if (candidateInviteMeList.Count > 0)
                        {
                            string content = "";

                            content = "决定和" + choosed.name + "一起" + choosed.actionName;
                            //不止一个人邀请
                            if (candidateInviteMeList.Count > 1)
                            {
                                content += ",并拒绝了";
                            }
                            for (int j = 0; j < candidateInviteMeList.Count; j++)
                            {
                                if (candidateInviteMeList[j] != choosed)
                                {
                                    content += candidateInviteMeList[j].name + "，";
                                    candidateInviteMeList[j].Record("由于" + choosed.name + "也邀请了" + p.name + ","
                                        + p.name + "选择和" + choosed.name + "一起" + choosed.actionName
                                        + ",拒绝了你的邀请");
                                    p.Refuse(candidateInviteMeList[j]);
                                }

                            }

                            p.Record(content);
                            //对我邀请的人说不去了
                            ForgivePeopleWhoIInvite(p, choosed, choosed.actionName);

                            choosed.Record(p.name + "答应了邀请," + "一起" + choosed.actionName);
                            //邀请我的人要拒绝掉其它邀请他的人
                            RefusePeopleWhoInviteMe(choosed, p, choosed.actionName);
                            //邀请我的人要对其它邀请他的人说不去了
                            ForgivePeopleWhoIInvite(choosed, p, choosed.actionName);

                        }

                    }
                }

            }

        }
        if (nextHandleList.Count > 0)
        {
            HandleInvite(nextHandleList);
        }
    }

    public void AddPlan(string actionName,int actionId, People p1,People p2=null)
    {
        Plan newPlan = new Plan(actionName, p1, p2);
        newPlan.actionId = actionId;
        p1.protoData.ChoosedActionId = actionId;
        if(p2!=null)
        p2.protoData.ChoosedActionId = actionId;
        planList.Add(newPlan);
        if (!action_planDic.ContainsKey(actionId))
        {
            action_planDic.Add(actionId, new List<Plan>());
        }
        action_planDic[actionId].Add(newPlan);
    }



    /// <summary>
    /// 拒绝 可以让人心情不那么快下降
    /// </summary>
    void RefusePeole()
    {

    }


    /// <summary>
    /// 玩家选择了邀请他的人 玩家选择之前 AI是不会选的 这里直接走拒绝逻辑 如果多人邀请玩家 玩家选择在手机面板上回复拒绝 那么AI的心情下降不会那么快
    /// </summary>
    void OnPlayerChoosedInvite(People me, People choosed)
    {
        AddPlan(choosed.actionName, choosed.protoData.ActionId, me, choosed);
        //Plan newPlan = new Plan(choosed.actionName, me, choosed);
        //planList.Add(newPlan);
        //if (!action_planDic.ContainsKey(choosed.protoData.ActionId))
        //{
        //    action_planDic.Add(choosed.protoData.ActionId, new List<Plan>());
        //}
        //action_planDic[choosed.protoData.ActionId].Add(newPlan);

        List<People> otherInviteMePeopleList = new List<People>();
        for(int i = 0; i < me.otherInviteMeList.Count; i++)
        {
            if(me.otherInviteMeList[i].people!=choosed)
            otherInviteMePeopleList.Add(me.otherInviteMeList[i].people);


        }

        //有人邀请我 则婉拒他
        if (otherInviteMePeopleList.Count > 0)
        {
            string content = "";

            content = "决定和" + choosed.name + "一起" + choosed.actionName;
            //不止一个人邀请
            if (otherInviteMePeopleList.Count > 1)
            {
                content += ",并拒绝了";
            }
            for (int j = 0; j < me.otherInviteMeList.Count; j++)
            {
             
                    content += otherInviteMePeopleList[j].name + "，";
                otherInviteMePeopleList[j].Record("由于" + choosed.name + "也邀请了" + me.name + ","
                        + me.name + "选择和" + choosed.name + "一起" + choosed.actionName
                        + ",拒绝了你的邀请");
                    me.Refuse(otherInviteMePeopleList[j]);
                

            }

            me.Record(content);
            //对我邀请的人说不去了
            //if(!me.isPlayer)
            //ForgivePeopleWhoIInvite(p, choosed, choosed.actionName);

            choosed.Record(me.name + "答应了邀请," + "一起" + choosed.actionName);
            //邀请我的人要拒绝掉其它邀请他的人
            RefusePeopleWhoInviteMe(choosed, me, choosed.actionName);
            //邀请我的人要对其它邀请他的人说不去了
            ForgivePeopleWhoIInvite(choosed, me, choosed.actionName);

        }
    }

    /// <summary>
    /// 选择一个人 并拒绝其它邀请我的人
    /// </summary>
    public void RefusePeopleWhoInviteMe(People main, People choosed, string actionName)
    {
        string recordStr = "选择和" + choosed.name + "一起" + actionName;
        int count = main.otherInviteMeList.Count;
        for (int i = 0; i < count; i++)
        {
            People theInvite = main.otherInviteMeList[i].people;
            if (!theInvite.finishInviteProcess && theInvite != choosed)
            {
                recordStr += "拒绝了" + theInvite.name + ",";
                main.Refuse(theInvite);
                //  main.Record("拒绝了" + theInvite.name);
                theInvite.Record(main.name + "选择和" + choosed.name + "一起" + actionName + "，并拒绝了你");
            }
        }
        main.Record(recordStr);

    }
    /// <summary>
    /// 选择一个人 并拒绝其它我邀请的人
    /// </summary>
    public void ForgivePeopleWhoIInvite(People main, People choosed, string actionName)
    {
        string recordStr = "选择和" + choosed.name + "一起" + actionName;
        int count = main.meInviteOtherList.Count;
        for (int i = 0; i < count; i++)
        {
            People theOtherInvite = main.meInviteOtherList[i].people;
            if (!theOtherInvite.finishInviteProcess && theOtherInvite != choosed && !main.meInviteOtherList[i].refused)
            {
                recordStr += "对" + theOtherInvite.name + "说改计划了";
                //main.Refuse(theInvite);
                //  main.Record("拒绝了" + theInvite.name);
                theOtherInvite.Record(main.name + "选择和" + choosed.name + "一起" + actionName + "，并对你说改计划了");
            }
        }
        main.Record(recordStr);
    }

    /// <summary>
    /// 发信给玩家
    /// </summary>
    public void SendMsgToPlayer(People from, People to, WetalkMsgData wetalkMsgData)
    {
        SinglePeopleChatData singlePeopleChatData = null;
        //红点
        for (int i = 0; i < to.protoData.AllSinglePeopleChatDataList.Count; i++)
        {
            if (to.protoData.AllSinglePeopleChatDataList[i].Belong == from.protoData.OnlyId)
            {
                singlePeopleChatData = to.protoData.AllSinglePeopleChatDataList[i];
                //置顶
                var tmp= to.protoData.AllSinglePeopleChatDataList[0];
                to.protoData.AllSinglePeopleChatDataList[0] = singlePeopleChatData;
                to.protoData.AllSinglePeopleChatDataList[i] = tmp;
                break;
            }
        }
        //如果没有聊过天，则插到第一位
        if (singlePeopleChatData == null)
        {
            singlePeopleChatData = new SinglePeopleChatData();
            singlePeopleChatData.Belong = from.protoData.OnlyId;
            to.protoData.AllSinglePeopleChatDataList.Insert(0, singlePeopleChatData);

        }
        OneChatData oneChatData = new OneChatData();
        oneChatData.Belong = from.protoData.OnlyId;
        oneChatData.Content = wetalkMsgData.content;
        if (from.protoData.IsPlayer)
            oneChatData.Checked = true;
        oneChatData.IsPlayer = from.protoData.IsPlayer;
        singlePeopleChatData.ChatDataList.Add(oneChatData);
        //后续可能只保留20条的聊天记录
        //其它人发给玩家的 则有红点
        if (!from.protoData.IsPlayer)
        {
            RedPoint son = RedPointManager.Instance.GetRedPointFromDic(RedPointType.SinglePeopleChatMsg, from.protoData.OnlyId);
            RedPoint parent = RedPointManager.Instance.GetRedPointFromDic(RedPointType.AllChatMsg, 0);
            RedPointManager.Instance.BindRedPoint(parent, son);
            RedPointManager.Instance.ChangeRedPointStatus(RedPointType.SinglePeopleChatMsg, from.protoData.OnlyId, true);
            //发消息给手机ui显示
            EventCenter.Broadcast(TheEventType.SendWetalkMessage, wetalkMsgData);
            EventCenter.Broadcast(TheEventType.ShowMainPanelRedPoint);

        }

        //若此时在聊天界面，则调用已读方法
    }

    /// <summary>
    /// 已读
    /// </summary>
    public void CheckedChat(SinglePeopleChatData singlePeopleChatData)
    {
        for(int i = 0; i < singlePeopleChatData.ChatDataList.Count; i++)
        {
            singlePeopleChatData.ChatDataList[i].Checked = true;

        }
        RedPoint son = RedPointManager.Instance.GetRedPointFromDic(RedPointType.SinglePeopleChatMsg, singlePeopleChatData.Belong);
        RedPointManager.Instance.ChangeRedPointStatus(RedPointType.SinglePeopleChatMsg, singlePeopleChatData.Belong, false);
        //发消息给手机ui显示
        EventCenter.Broadcast(TheEventType.CheckedWetalkMessage, singlePeopleChatData);

    }

    /// <summary>
    /// 获取未读消息记录
    /// </summary>
    public int GetUnCheckChatNum(UInt64 onlyId)
    {
        int res = 0;

        for (int i = 0; i < RoleManager.Instance.playerPeople.protoData.AllSinglePeopleChatDataList.Count; i++)
        {
            SinglePeopleChatData data = RoleManager.Instance.playerPeople.protoData.AllSinglePeopleChatDataList[i];
            if (data.Belong == onlyId)
            {
                for(int j = 0; j < data.ChatDataList.Count; j++)
                {
                    OneChatData oneData = data.ChatDataList[i];
                    if (!oneData.Checked)
                        res++;
                }
                break;
            }
        }
        return res;
    }
}

/// <summary>
/// 人和人发消息数据
/// </summary>
public class WetalkMsgData
{
    public WetalkMsgType wetalkMsgType;
    public string content;
    public People from;
    public People to;
    public WetalkMsgData(WetalkMsgType type,string content,People from,People to)
    {
        this.wetalkMsgType = type;
        this.content = content;
        this.from = from;
        this.to = to;
    }
}

/// <summary>
/// 人与人消息类型
/// </summary>
public enum WetalkMsgType
{
    None=0,
    InviteAction=1,//邀请行为
    Nonsense=2,//废话（不需要回复）
}