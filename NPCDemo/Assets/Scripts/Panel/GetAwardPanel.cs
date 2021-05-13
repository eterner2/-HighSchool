using Framework.Data;
using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAwardPanel : PanelBase
{
    public Transform trans_grid;

    public List<AwardData> awardDataList = new List<AwardData>();
    public Action closeCallBack;

   
    public override void Init(params object[] args)
    {
        base.Init(args);
        awardDataList = args[0] as List<AwardData>;
        closeCallBack = args[1] as Action;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        for(int i=0;i< awardDataList.Count; i++)
        {
            PanelManager.Instance.OpenSingle<AwardView>(trans_grid, awardDataList[i]);
        }
    }

    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_grid);
    }
    public override void OnClose()
    {
        base.OnClose();
        closeCallBack?.Invoke();
    }

    /// <summary>
    /// 显示经验值增加
    /// </summary>
    public void ShowExpAdd()
    {
        int numBeforeAdd = 0;
        //反推出升级之前的等级 然后播放一个动画
        for (int i = 0; i < awardDataList.Count; i++)
        {
            AwardData awardData = awardDataList[i];
            if (awardData.awardType==AwardType.Property
                && awardData.id == (int)PropertyIdType.Study)
            {
                int curLevel = RoleManager.Instance.playerPeople.protoData.PropertyData.Level;
                int curNum =Mathf.RoundToInt(RoleManager.Instance.FindSinglePropertyData(PropertyIdType.Study).PropertyNum);
                int addedNum = awardData.num;
                numBeforeAdd = curNum - addedNum;

                break;
            }
        }


        int canReachLevel = 1;
        int studyNum = numBeforeAdd;// Mathf.RoundToInt(RoleManager.Instance.FindSinglePropertyData(PropertyIdType.Study).PropertyNum);
        int studyNumAfterAllUpgrade = 0;
        //int curLevel = 1;
        if (canReachLevel < DataTable._peopleUpgradeList.Count)
        {
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

        //之前的等级为canreachLevel

    }

    void PlayExpAnim(int beforeLevel,int afterLevel,)
    {

    }
}
