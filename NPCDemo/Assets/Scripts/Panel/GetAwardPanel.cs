using DG.Tweening;
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

    public LevelInfo levelInfo;//升级前的经验值和等级关系（用于显示

    public Transform trans_exp;//经验变化

    public float expMoveTotalTime;//经验值动的总时间
    public float singleExpMoveTime;//单级经验值动的时间
    public float expMoveTimer;
    public bool startExpMove;
    public float afterExpBarProcess;//升级后经验条的位置
    public float beforeExpBarProcess;//移动前经验条的位置

    public int expMoveCurLevel;//当前移动的等级
    public float expMoveCurPorocess;//当前移动的进度
    public float expMoveCurSpeed;//当前移动的速度（每帧动多少）

    public Image img_expBar;//经验条
    public Text txt_level;//等级
    public Text txt_exp;//经验值
    int curExpLimit;//当前经验上限

    public override void Init(params object[] args)
    {
        base.Init(args);
        awardDataList = args[0] as List<AwardData>;
        closeCallBack = args[1] as Action;
        levelInfo = args[2] as LevelInfo;

        expMoveTotalTime = 2;
        startExpMove = false;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        for(int i=0;i< awardDataList.Count; i++)
        {
            PanelManager.Instance.OpenSingle<AwardView>(trans_grid, awardDataList[i]);
        }
        if (levelInfo != null)
            ShowExpAdd();
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


    void Moving()
    {

    }
    private void Update()
    {
        if (startExpMove)
        {
            //分两种情况
            //在统一等级移动
            float endProcess = afterExpBarProcess;
            if (expMoveCurLevel == levelInfo.canReachLevel)
            {
                endProcess = afterExpBarProcess;
        
            }
            else
            {
                endProcess = 1;
            }

            if (img_expBar.fillAmount <= endProcess)
            {
                expMoveTimer += Time.deltaTime;
                //移动速度
                float realAdd = expMoveCurSpeed;

                //加到底了
                if (img_expBar.fillAmount + realAdd >= endProcess)
                {
                    realAdd = endProcess - img_expBar.fillAmount;
                    img_expBar.fillAmount += realAdd;

                    //跳出循环
                    if (expMoveCurLevel < levelInfo.canReachLevel)
                    {
                        img_expBar.fillAmount = 0;
                        expMoveCurLevel++;
                        curExpLimit = DataTable._peopleUpgradeList[expMoveCurLevel].needExp.ToInt32();
                        txt_exp.SetText(img_expBar.fillAmount * curExpLimit + "/" + curExpLimit);
                        txt_level.SetText(expMoveCurLevel.ToString());
                    }
                    else
                    {
                        startExpMove = false;
                        txt_exp.SetText(img_expBar.fillAmount * curExpLimit + "/" + curExpLimit);

                        //如果满级了
                        if (expMoveCurLevel== DataTable._peopleUpgradeList.Count)
                        {
                            txt_exp.SetText("已满级");
                        }

                    }
                }
                else
                {
                    img_expBar.fillAmount += realAdd;
                    txt_exp.SetText(img_expBar.fillAmount * curExpLimit + "/" + curExpLimit);

                }
            }
            

        }
    }
    /// <summary>
    /// 显示经验值增加
    /// </summary>
    public void ShowExpAdd()
    {

        //之前的等级为canreachLevel
        expMoveCurLevel = RoleManager.Instance.playerPeople.protoData.PropertyData.Level;
        int levelChange = levelInfo.canReachLevel - expMoveCurLevel;

        singleExpMoveTime = expMoveTotalTime / (levelChange + 1);

        beforeExpBarProcess = 0;

        //经验条的位置
        if (expMoveCurLevel < DataTable._peopleUpgradeList.Count)
        {
            beforeExpBarProcess = RoleManager.Instance.playerPeople.protoData.PropertyData.CurExp / DataTable._peopleUpgradeList[expMoveCurLevel].needExp.ToFloat();
            curExpLimit = DataTable._peopleUpgradeList[expMoveCurLevel].needExp.ToInt32();
        }
        //满级了
        else
        {
            beforeExpBarProcess = 1;
        }
        img_expBar.fillAmount = beforeExpBarProcess;

        afterExpBarProcess = 0;
        //经验条的位置
        if (levelInfo.canReachLevel < DataTable._peopleUpgradeList.Count)
        {
            afterExpBarProcess = levelInfo.ExpAfterUpgrade / DataTable._peopleUpgradeList[levelInfo.canReachLevel].needExp.ToFloat();
        }
        else
        {
            afterExpBarProcess = 1;
        }
        expMoveCurSpeed = (1 / singleExpMoveTime)* Time.deltaTime;

        txt_level.SetText(expMoveCurLevel.ToString());
        //InitExpMoveSpeed();
        startExpMove = true;
        expMoveTimer = 0;
    }

 

}
