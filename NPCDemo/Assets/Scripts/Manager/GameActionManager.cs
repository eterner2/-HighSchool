using Framework.Data;
using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoInstance<GameActionManager>
{
    public float ProcessSpeed = 5;//一次行动的总时间（单位秒 后续配表）
    public bool startMove = false;//开始走
    public Vector2Int[] PhaseRangeArr = new Vector2Int[2];//阶段（暂时平均分成2段）
    public int lastPhaseIndex = -1;//上个阶段（这个值改变代表上了一节课）

    ActionData actionData = null;
    /// <summary>
    /// 开始行动
    /// </summary>
    public void StartAction()
    {
        lastPhaseIndex = -1;
        actionData = RoleManager.Instance._CurGameInfo.CurActionData;
        PhaseRangeArr[0] = new Vector2Int(0, 49);
        PhaseRangeArr[1] = new Vector2Int(51, 100);
        lastPhaseIndex = 0;

        EventCenter.Broadcast(TheEventType.OnActionStart);
        startMove = true;
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueAction()
    {
        EventCenter.Broadcast(TheEventType.ActionPause, false);
        startMove = true;

    }

    private void Update()
    {
        if (startMove)
        {
            Processing(Time.deltaTime);
        }
    }

    void Processing(float deltaTime)
    {
        float theProcessAdd = (100 / ProcessSpeed) * deltaTime;
        actionData.Process += (theProcessAdd);
        EventCenter.Broadcast(TheEventType.ActionProcess, actionData.Process);

        //时间过去了
        if (actionData.Process >= 100)
        {
            actionData.Process = 100;
            startMove = false;
            //结束
            EndGameAction();
        }
        else
        {
            int curPhaseIndex = CommonUtil.GetPhaseIndex((int)actionData.Process,PhaseRangeArr);
            if (curPhaseIndex > 0)
            {
                if (curPhaseIndex > lastPhaseIndex)
                {  
                    //进入聊天 可能搭一次讪
                    if (lastPhaseIndex == 0)
                    {
                        EventCenter.Broadcast(TheEventType.ActionPause);
                        List<DialogData> dialogList = new List<DialogData>();
                        dialogList.Add(new DialogData(null, "休息一下吧"));
                        DialogManager.Instance.CreateDialog(dialogList,()=> 
                        {
                            EventCenter.Broadcast(TheEventType.ActionPause, true);

                        });
                        startMove = false;

                    }
                    lastPhaseIndex = curPhaseIndex;
                    //得到分数
                    //addScoreTime++;
                    //RoleManager.Instance.GetStudyScore();
                   
                }
            }
        }

    }

    /// <summary>
    /// 结束
    /// </summary>
    void EndGameAction()
    {
        List<AwardData> awardList = new List<AwardData>();

        //玩家和npc的属性都要变化 TODO玩家每次见到的NPC有限 一个图书馆可能只抽出一部分NPC
        for (int i = 0; i < RoleManager.Instance.allPeopleList.Count; i++)
        {
            People people = RoleManager.Instance.allPeopleList[i];
            ActionSetting actionSetting = DataTable.FindActionSetting(people.protoData.ChoosedActionId);
            List<List<int>> proChange = CommonUtil.SplitCfg(actionSetting.proChange);
 
            for (int j = 0; j < proChange.Count; j++)
            {
                List<int> singleChange = proChange[j];
                if (singleChange[1] > 0)
                {
                    RoleManager.Instance.AddProperty((PropertyIdType)singleChange[0], singleChange[1], people.protoData);
                    if (people.protoData.OnlyId == RoleManager.Instance.playerPeople.protoData.OnlyId)
                    {
                        AwardData award = new AwardData(AwardType.Property, singleChange[0], singleChange[1]);

                        //singlePropertyData.PropertyId = singleChange[0];
                        //singlePropertyData.PropertyNum = singleChange[1];
                        awardList.Add(award);

                    }
    

                }
            }

        }
        Action backHome = delegate
        {
            GameModuleManager.Instance.InitGameModule(GameModuleType.Home);
        };
        PanelManager.Instance.OpenPanel<GetAwardPanel>(PanelManager.Instance.trans_layer2, awardList, backHome,null);
        //for(int i=0;i<)
    }

  
}
