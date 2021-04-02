using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RoleData;
using Framework.Data;

public class NewBehaviourScript : MonoBehaviour
{
    public List<People> allPeopleList = new List<People>();
    public List<Plan> planList = new List<Plan>();
   // List<string> actionNameList = new List<string> { "去网吧开黑", "去图书馆学习", "打羽毛球", "散步", "逛街", "去市区玩密室逃脱","逛超市","去吃自助餐"};

    public Transform trans_peopleGrid;

    public PeopleScriptable peopleScriptable;
    public ActionScriptable actionScriptable;
    public static NewBehaviourScript Instance;

    public GameObject panel_classroom;
    public GameObject panel_menu;
    public GameObject panel_outside;

    public Button btn_out;
    public Button btn_newDay;
    public Button btn_showAllPeople;
    public Image img_mask;
    private void Awake()
    {
        Instance = this;
        btn_out.onClick.AddListener(() =>
        {
            panel_classroom.gameObject.SetActive(false);
            panel_menu.gameObject.SetActive(true);
            panel_outside.gameObject.SetActive(false);
        });
        btn_newDay.onClick.AddListener(() =>
        {
           // img_mask.DOFade(0, 0);
            img_mask.DOFade(1, 0.5f).OnComplete(() =>
            {
                img_mask.DOFade(0, 0.5f);
                StartNewDay();

                trans_peopleGrid.gameObject.SetActive(false);
            });
        });
        btn_showAllPeople.onClick.AddListener(() =>
        {
            trans_peopleGrid.gameObject.SetActive(true);

        });


    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <60; i++)
        {
            //People p = new People(peopleScriptable.peopleDataList[i].name, peopleScriptable.peopleDataList[i].gender);
            //allPeopleList.Add(p);
            //GeneratePeople(p);
        }

        //for (int i = 0; i < 100; i++)
        //{
        //    People p = new People(i.ToString(),Gender.Male);
        //    allPeopleList.Add(p);
        //    GeneratePeople(p);
        //}
    }

    /// <summary>
    /// 生成所有学生
    /// </summary>
    void GeneratePeople(People people)
    {
        PeopleView peopleView = GenerateEntity(ObjectPoolSingle.PeopleView) as PeopleView;
        peopleView.transform.SetParent(trans_peopleGrid, false);
        peopleView.Init(people);
    }


    void StartNewDay()
    {
        planList = new List<Plan>();

        for (int i = 0; i < allPeopleList.Count; i++)
        {
            allPeopleList[i].Clear();
            allPeopleList[i].Record("新的一天开始了");
        }

        //定计划
        for (int i = 0; i < allPeopleList.Count; i++)
        {
            People me = allPeopleList[i];
            List<People> tmpList = new List<People>();

            //tmpList.Clear();

            int actionIndex = RandomManager.Next(0, actionScriptable.singleActionList.Count);
            me.actionName = actionScriptable.singleActionList[actionIndex].name;

            //邀请0-5人(不能重复邀请
            int num = RandomManager.Next(0, 6);
            if (num > 0)
            {
                for (int j = 0; j < num; j++)
                {
                    People choosed = null;
                    while (choosed == null
                        || choosed == me
                        || tmpList.Contains(choosed))
                    {
                        int index = RandomManager.Next(0, allPeopleList.Count);
                        choosed = allPeopleList[index];
                    }
                    tmpList.Add(choosed);
                    me.Invite(choosed);
                    me.Record("邀请" + choosed.name + me.actionName);
                    choosed.Record("被" + me.name + "邀请"+me.actionName);

                }
            }
            else
            {
                //planList.Add(new Plan(me.actionName, me, null));
                me.Record("想今天一个人" + me.actionName);
            }
        }

        Debug.Log(allPeopleList);
        HandleInvite(allPeopleList);

        Debug.Log(planList);
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
                if (meInviteOtherPeopleList.Contains(other)&&!other.finishInviteProcess)
                {
                    if(p.actionName == other.actionName)
                    {
                        p.Record("在我的回合发现" + other.name + "也邀请我" + p.actionName + ",感到十分高兴，于是一起" + p.actionName);
                        other.Record("在"+p.name+"的回合"+"发现" + p.name + "也邀请我" + other.actionName + ",感到十分高兴，于是一起" + other.actionName);
                        planList.Add(new Plan(p.actionName, p, other));

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
                            planList.Add(new Plan(p.actionName, p, other));

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
                            planList.Add(new Plan(other.actionName, p, other));

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
                    planList.Add(new Plan(p.actionName, p, null));
                    continue;
                }
                //如果没有我邀请的人 但有邀请我的人
                else if (candidateMeInviteOtherList.Count == 0 && candidateInviteMeList.Count > 0)
                {
                    string content = "";
                    int rdmIndex = RandomManager.Next(0, candidateInviteMeList.Count);

                    planList.Add(new Plan(candidateInviteMeList[rdmIndex].actionName, p, candidateInviteMeList[rdmIndex]));
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

                    ////等待回应
                    //int index = RandomManager.Next(0, candidateMeInviteOtherList.Count);
                    //People choosed = candidateMeInviteOtherList[index];

                    ////如果选择了我邀请的人，则拒绝所有邀请我的人 然后进入下一轮
                    //if (p.ifMeInvitePeople(choosed))
                    //{
                    //    //有人邀请我 选他然后拒绝别人
                    //    if (candidateInviteMeList.Count > 0)
                    //    {
                    //        string content = "";
                    //        content = "还是想和" + choosed.name + "一起" + p.actionName + ",所以拒绝了";

                    //        for (int j = 0; j < candidateInviteMeList.Count; j++)
                    //        {
                    //            content += candidateInviteMeList[j].name + "，";
                    //            candidateInviteMeList[j].Record("由于"
                    //                + p.name + "还是想和" + choosed.name + "一起" + p.actionName
                    //                + ",拒绝了你的邀请");

                    //            p.Refuse(candidateInviteMeList[j]);
                    //        }

                    //        p.Record(content);
                    //    }
                    //}
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
                        planList.Add(new Plan(choosed.actionName, p, choosed));
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
                #region old
                //bool allRefused = true;
                //People choosedMeInviteOther = null;
                ////如果我邀请的人都拒绝我了 只能从邀请我的人里面选 然后拒绝邀请我的其他人 并且只能选邀请我的人干的事 然后结束计划
                //for (int j = 0; j < p.meInviteOtherList.Count; j++)
                //{
                //    MeInviteOtherData meInviteOtherData = p.meInviteOtherList[j];
                //    if (!meInviteOtherData.refused && !meInviteOtherData.people.finishInviteProcess)
                //    {
                //        choosedMeInviteOther = meInviteOtherData.people;
                //        allRefused = false;
                //        break;
                //    }
                //}
                //if (allRefused)
                //{
                //    //如果没有邀请我的人 只能独自去
                //    if (candidateInviteMeList.Count == 0)
                //    {
                //        p.Record("决定独自去" + p.actionName);
                //    }
                //    //如果我邀请的人都拒绝我了
                //    else
                //    {
                //        //有人邀请我 选他然后拒绝别人
                //        if (candidateInviteMeList.Count > 0)
                //        {
                //            string content = "";
                //            int rdmIndex = RandomManager.Next(0, candidateInviteMeList.Count);

                //            planList.Add(new Plan(candidateInviteMeList[rdmIndex].actionName,p, candidateInviteMeList[rdmIndex]));
                //            content = "决定和" + candidateInviteMeList[rdmIndex].name + "一起"+ candidateInviteMeList[rdmIndex].actionName;
                //            //不止一个人邀请
                //            if (candidateInviteMeList.Count > 1)
                //            {
                //                content += ",并拒绝了";
                //            }
                //            for (int j = 0; j < candidateInviteMeList.Count; j++)
                //            {
                //                if (j != rdmIndex)
                //                {
                //                    content += candidateInviteMeList[j].name + "，";
                //                    candidateInviteMeList[j].Record("由于"+ candidateInviteMeList[rdmIndex].name+"也邀请了"+p.name+","
                //                        +p.name + "选择和"+ candidateInviteMeList[rdmIndex].name + "一起"+ candidateInviteMeList[rdmIndex].actionName
                //                        +",拒绝了你的邀请");
                //                    p.Refuse(candidateInviteMeList[j]);
                //                }

                //            }

                //            p.Record(content);
                //            //ForgivePeopleWhoIInvite(p,)
                //            candidateInviteMeList[rdmIndex].Record(p.name + "答应了邀请," + "一起" + candidateInviteMeList[rdmIndex].actionName);
                //            //我选的这个人还要拒绝掉其它邀请了他的人
                //            RefusePeopleWhoInviteMe(candidateInviteMeList[rdmIndex], p, candidateInviteMeList[rdmIndex].actionName);
                //            //我选的这个人还要对他邀请的人说不去了
                //            ForgivePeopleWhoIInvite(candidateInviteMeList[rdmIndex], p, candidateInviteMeList[rdmIndex].actionName);
                //        }
                //    }
                //}
                ////如果我邀请的人还有没拒绝我的 则随机看是选择我邀请的还是邀请我的
                //else
                //{
                //    int index = RandomManager.Next(0, candidateList.Count);
                //    People choosed = candidateList[index];
                //    //如果选择了我邀请的人，则拒绝所有邀请我的人 然后进入下一轮
                //    if (p.ifMeInvitePeople(choosed))
                //    {
                //        //有人邀请我 选他然后拒绝别人
                //        if (candidateInviteMeList.Count > 0)
                //        {
                //            string content = "";
                //            content = "还是想和" + choosed.name + "一起" + p.actionName + ",所以拒绝了";

                //            for (int j = 0; j < candidateInviteMeList.Count; j++)
                //            {
                //                content += candidateInviteMeList[j].name + "，";
                //                candidateInviteMeList[j].Record("由于"
                //                    + p.name + "还是想和" + choosed.name + "一起" + p.actionName
                //                    + ",拒绝了你的邀请");

                //                p.Refuse(candidateInviteMeList[j]);
                //            }

                //            p.Record(content);
                //        }
                //        nextHandleList.Add(p);
                //    }
                //    //如果选择了邀请我的人，则直接答应 并拒绝其他邀请我的人
                //    else
                //    {
                //        planList.Add(new Plan(choosed.actionName, p, choosed));
                //        //有人邀请我 选他然后拒绝别人
                //        if (candidateInviteMeList.Count > 0)
                //        {
                //            string content = "";

                //            content = "决定和" + choosed.name + "一起" + choosed.actionName;
                //            //不止一个人邀请
                //            if (candidateInviteMeList.Count > 1)
                //            {
                //                content += ",并拒绝了";
                //            }
                //            for (int j = 0; j < candidateInviteMeList.Count; j++)
                //            {
                //                if (candidateInviteMeList[j] != choosed)
                //                {
                //                    content += candidateInviteMeList[j].name + "，";
                //                    candidateInviteMeList[j].Record("由于" + choosed.name + "也邀请了" + p.name + ","
                //                        + p.name + "选择和" + choosed.name + "一起" + choosed.actionName
                //                        + ",拒绝了你的邀请");
                //                    p.Refuse(candidateInviteMeList[j]);
                //                }

                //            }

                //            p.Record(content);
                //            //对我邀请的人说不去了
                //            ForgivePeopleWhoIInvite(p, choosed, choosed.actionName);

                //            choosed.Record(p.name + "答应了邀请," + "一起" + choosed.actionName);
                //            //邀请我的人要拒绝掉其它邀请他的人
                //            RefusePeopleWhoInviteMe(choosed, p, choosed.actionName);
                //            //邀请我的人要对其它邀请他的人说不去了
                //            ForgivePeopleWhoIInvite(choosed, p, choosed.actionName);

                //        }

                //    }
                //}
                #endregion
            }

        }
        if (nextHandleList.Count > 0)
        {
            HandleInvite(nextHandleList);
        }
    }

    /// <summary>
    /// 选择一个人 并拒绝其它邀请我的人
    /// </summary>
    public void RefusePeopleWhoInviteMe(People main,People choosed,string actionName)
    {
        string recordStr = "选择和" + choosed.name + "一起" + actionName;
        int count = main.otherInviteMeList.Count;
        for(int i = 0; i < count; i++)
        {
            People theInvite = main.otherInviteMeList[i].people;
            if (!theInvite.finishInviteProcess && theInvite != choosed)
            {
                recordStr += "拒绝了" + theInvite.name+",";
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
            if (!theOtherInvite.finishInviteProcess && theOtherInvite != choosed&&!main.meInviteOtherList[i].refused)
            {
                recordStr += "对" + theOtherInvite.name + "说改计划了";
                //main.Refuse(theInvite);
                //  main.Record("拒绝了" + theInvite.name);
                theOtherInvite.Record(main.name + "选择和" + choosed.name + "一起" + actionName + "，并对你说改计划了");
            }
        }
        main.Record(recordStr);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    StartNewDay();
        //}
    }

    public Entity GenerateEntity(ObjectPoolSingle single)
    {
       GameObject obj= ObjectPoolManager.Instance.GetObjcectFromPool(single, single.ToString(), true);
        Entity entity = obj.GetComponent<Entity>();
        entity.obj = obj;
       return obj.GetComponent<Entity>();
    }

    public void ClearAllChildEntity(Transform trans)
    {
        int count = trans.childCount;
        for(int i = count - 1; i >= 0; i--)
        {
            Entity entity = trans.GetChild(i).GetComponent<Entity>();
            ObjectPoolManager.Instance.DisappearObjectToPool(entity.objType, entity.obj, entity.isTmpObj);
        }
    }

    /// <summary>
    /// 去外面
    /// </summary>
    public void GoOutSide(string placeName)
    {
        panel_classroom.gameObject.SetActive(false);
        panel_menu.gameObject.SetActive(false);
        panel_outside.gameObject.SetActive(true);
        panel_outside.GetComponent<OutsidePanel>().Init(placeName);
    }

    public void BackClassRoom()
    {
        panel_classroom.gameObject.SetActive(true);
        panel_menu.gameObject.SetActive(false);
        panel_outside.gameObject.SetActive(false);
    }
}

public class Plan
{
    public string actionName;
    public List<People> peopleList = new List<People>();
    public Plan(string actionName, People p1,People p2=null)
    {
        this.actionName = actionName;
        peopleList.Add(p1);
        p1.actionName = actionName;
        p1.FinishInviteProcess();

        if (p2 != null)
        {
            peopleList.Add(p2);
            p2.actionName = actionName;
            p2.FinishInviteProcess();

        }

    }
}
[System.Serializable]
public class People
{
    public PeopleProtoData protoData;//保存的时候把数据保存在此 载入的时候把这个载入

    public string actionName;//想做什么
    public string name;
    public bool finishInviteProcess;//结束邀约进程
    public Gender gender = Gender.None;//是男
    public List<MeInviteOtherData> meInviteOtherList = new List<MeInviteOtherData>();//我邀请的人
    public List<OtherInviteMeData> otherInviteMeList = new List<OtherInviteMeData>();//邀请我的人

    public List<string> recordList = new List<string>();
    public bool isPlayer = false;

    /// <summary>
    /// TODO通过配表的Setting创建新People（PeopleData后续要改成PeopleSetting）
    /// </summary>
    /// <param name="peopleData"></param>
    public People(PeopleData peopleData)
    {
        this.name = peopleData.name;
        this.gender = peopleData.gender;
        if (this.name == "毛鹏程")
        {
            isPlayer = true;
        }
        PeopleProtoData peopleProtoData = new PeopleProtoData();
        CreateNewPropertyData(peopleProtoData);
        this.protoData = peopleProtoData;
    }
    /// <summary>
    /// TODO通过存档的Protocol来创建People（DEMO开发阶段为了可读性 另外存一份 后续全部采用Protocol数据）
    /// </summary>
    /// <param name="peopleProtoData"></param>
    public People(PeopleProtoData peopleProtoData)
    {

    }

   

    /// <summary>
    /// 创建新的属性数据
    /// </summary>
    /// <param name="gameInfo"></param>
    void CreateNewPropertyData(PeopleProtoData peopleProtoData)
    {

        PropertyData propertyData = new PropertyData();



        InitSingleProperty(propertyData, PropertyIdType.Study);
        InitSingleProperty(propertyData, PropertyIdType.Art);
        InitSingleProperty(propertyData, PropertyIdType.Physical);
        InitSingleProperty(propertyData, PropertyIdType.Money);
        InitSingleProperty(propertyData, PropertyIdType.TiLi);
        InitSingleProperty(propertyData, PropertyIdType.Mood);
        InitSingleProperty(propertyData, PropertyIdType.SelfControl);
        InitSingleProperty(propertyData, PropertyIdType.Charm);


        peopleProtoData.PropertyData = propertyData;
    }


    public void InitSingleProperty(PropertyData propertyData, PropertyIdType idType)
    {
        //PropertyData propertyData = new PropertyData();
        PropertySetting setting = DataTable.FindPropertySetting((int)idType);

        string[] rdmRange = setting.newRdmRange.Split('|');
        int val = RandomManager.Next(rdmRange[0].ToInt32(), rdmRange[1].ToInt32());

        propertyData.PropertyIdList.Add((int)idType);

        SinglePropertyData singlePropertyData = new SinglePropertyData();
        singlePropertyData.PropertyId = (int)idType;
        singlePropertyData.PropertyNum = val;
        singlePropertyData.PropertyLimit = setting.haveLimit.ToInt32();

        propertyData.PropertyDataList.Add(singlePropertyData);

        //return singlePropertyData;
    }

    /// <summary>
    /// 邀请
    /// </summary>
    /// <param name="people"></param>
    public void Invite(People other)
    {
        if (other == this)
        {
            Debug.LogError("你不能邀请自己！！！");
            return;
        }

        meInviteOtherList.Add(new MeInviteOtherData(other));
        other.BeInvite(this);
    }

    /// <summary>
    /// 被邀请
    /// </summary>
    /// <param name="people"></param>
    public void BeInvite(People people)
    {
        if (people == this)
        {
            Debug.LogError("你不能被自己邀请！！！");
            return;
        }

        otherInviteMeList.Add(new OtherInviteMeData(people));
        
    }

    /// <summary>
    /// 是我邀请的人
    /// </summary>
    /// <returns></returns>
    public bool ifMeInvitePeople(People other)
    {
        //bool res = false;
        for(int i = 0; i < meInviteOtherList.Count; i++)
        {
            if (other == meInviteOtherList[i].people)
                return true;
        }
        return false;
    }
    ///
    /// 是邀请我的人
    /// </summary>
    /// <returns></returns>
    public bool ifOtherInviteMePeople(People other)
    {
        //bool res = false;
        for (int i = 0; i < otherInviteMeList.Count; i++)
        {
            if (other == otherInviteMeList[i].people)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 拒绝别人的邀约
    /// </summary>
    /// <param name="people"></param>
    public void Refuse(People other)
    {
        for (int i = 0; i < otherInviteMeList.Count; i++)
        {
            if (other == otherInviteMeList[i].people)
            {
                for(int j = 0; j < other.meInviteOtherList.Count; j++)
                {
                    if (this == other.meInviteOtherList[j].people)
                    {
                        other.meInviteOtherList[j].refused = true;
                        break;
                    }
                }
                break;
            }
                //return true;
        }
    }


    /// <summary>
    /// 结束邀请流程
    /// </summary>
    public void FinishInviteProcess()
    {
        finishInviteProcess = true;
    }

    /// <summary>
    /// 记录
    /// </summary>
    /// <param name="content"></param>
    public void Record(string content)
    {

        recordList.Add(content);
    }

    public void Clear()
    {
        meInviteOtherList.Clear();
        otherInviteMeList.Clear();
        finishInviteProcess = false;
    }
}

/// <summary>
/// 邀请我数据
/// </summary>
public class OtherInviteMeData
{
    public People people;
    public OtherInviteMeData(People people)
    {
        this.people = people;
    }
}

/// <summary>
/// 我邀请别人的数据
/// </summary>
public class MeInviteOtherData
{
    public People people;
    public bool refused = false;

    public MeInviteOtherData(People people)
    {
        this.people = people;
    }
}

public enum Gender
{
    None=0,
    Male=1,
    Female=2,
}