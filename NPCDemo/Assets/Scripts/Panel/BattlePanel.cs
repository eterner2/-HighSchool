using DG.Tweening;
using RoleData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : PanelBase
{
    
    public float delayStartTime = 1f;//延迟多久以后开始
    public float delayStartTimer = 0;
    bool startCalcDelay;

    bool startBattle = false;
    public VSAnimSingle VSAnimSingle;

    public SingleExamEnemy curEnemy;
    //public 
   
    public Transform trans_playerPos;
    public Transform trans_enemyPos;

    public SingleBattlePeopleView playerBattleView;
    public SingleBattlePeopleView enemyBattleView;

    public int roleLevel;//玩家等级
    public int enemyLevel;//敌人等级

    public Button btn_restartTest;//重新开始测试
    public Button btn_NextEnemy;//换个敌人
    public float basicAttackSpeed = 2f;//基本速度
    public bool isTestBattle = false;//是否测试玩法

    public Transform trans_gameEnd;
    public Text txt_gameEnd;

    public Button btn_gameEndLeave;//离开
    public Transform trans_award;//奖励
    public Transform trans_awardGrid;

    

    public override void Init(params object[] args)
    {
        base.Init(args);
        basicAttackSpeed = 2;
        this.curEnemy = args[0] as SingleExamEnemy;
        EventCenter.Register(TheEventType.BattleHit, OnHit);
        EventCenter.Register(TheEventType.BattleEnd, OnBattleEnd);

        if (isTestBattle)
        {
            addBtnListener(btn_restartTest, () =>
            {
                RoleManager.Instance.TestSetProperty(true, enemyLevel);
                RoleManager.Instance.TestSetProperty(false, roleLevel);
                TestBattle();
            });


            addBtnListener(btn_NextEnemy, () =>
            {
                RoleManager.Instance.TestSetProperty(true, enemyLevel);
                // RoleManager.Instance.TestSetProperty(false, roleLevel);
                TestBattle();
            });
        }
        else
        {
            btn_restartTest.gameObject.SetActive(false);
            btn_NextEnemy.gameObject.SetActive(false);
        }
  
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        //delayStartTimer = 0;
        //startBattle = false;
        //startCalcDelay = true;
        VSAnimSingle.StartAnim();



        // property_player = RoleManager.Instance.playerPeople.protoData.PropertyData;
        //BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.playerPeople.protoData.PropertyData);
        //BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.examPropertyData);

        ////以下为测试数据
        //RoleManager.Instance.InitSingleProperty()


        playerBattleView = PanelManager.Instance.OpenSingle<SingleBattlePeopleView>(trans_playerPos,null,
            this);
        enemyBattleView= PanelManager.Instance.OpenSingle<SingleBattlePeopleView>(trans_enemyPos, curEnemy,
        this);

        //enemyAttackSpeed = (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, property_enemy).PropertyNum / (float)100) * basicAttackSpeed;
        //playerAttackSpeed= (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, property_player).PropertyNum / (float)100) * basicAttackSpeed;

        trans_gameEnd.gameObject.SetActive(false);
        BattleStart();
    }

    void BattleStart()
    {
        delayStartTimer = 0;
        startBattle = false;
        startCalcDelay = true;
    }

    private void Update()
    {
        if (startCalcDelay)
        {
            delayStartTimer += Time.deltaTime;
            if (delayStartTimer >= delayStartTime)
            {
                startCalcDelay = false;
                startBattle = true;
                playerBattleView.StartBattle();
                enemyBattleView.StartBattle();
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TestBattle();
        //}
    }

    /// <summary>
    /// 测试battle
    /// </summary>
    void TestBattle()
    {   
        
     
        //BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.playerPeople.protoData.PropertyData);
        //BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.examPropertyData);


        //playerBattleView.SetTestData(RoleManager.Instance.playerPeople.protoData.PropertyData);
        //enemyBattleView.SetTestData(RoleManager.Instance.examPropertyData);
        playerBattleView.Show();
        enemyBattleView.Show();

        delayStartTimer = 0;
        startBattle = false;
        startCalcDelay = true;
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack(SingleBattlePeopleView singleBattlePeopleView)
    {
        if (singleBattlePeopleView == playerBattleView)
        {
            BattleManager.Instance.Attack(RoleManager.Instance.playerPeople.protoData.PropertyData, enemyBattleView.singleEnemy.Property);
        }
        else
        {
            BattleManager.Instance.Attack(enemyBattleView.singleEnemy.Property, RoleManager.Instance.playerPeople.protoData.PropertyData);
        }

    }

    void OnHit(object[] param)
    {
        HitData hitData = param[0] as HitData;

        //UInt64 onlyId = hitData.beHitPro.OnlyId;
        //PropertyData pro=
        //玩家被打
        if (hitData.beHitPro.IsPlayer)
        {
            playerBattleView.OnHit(hitData);
        }
        else
        {
            enemyBattleView.OnHit(hitData);
        }
    }

    void OnBattleEnd(object[] args)
    {
        PropertyData deadPro = args[0] as PropertyData;
        int score = (int)args[1];
        startBattle = false;
        startCalcDelay = false;
        playerBattleView.OnEnd();
        enemyBattleView.OnEnd();

        trans_gameEnd.gameObject.SetActive(true);
        bool win;
        //玩家死了
        if (deadPro.IsPlayer)
        {
            trans_award.gameObject.SetActive(false);
            txt_gameEnd.SetText("输");
            win = false;
 
        }
        else
        {
            win = true;
            trans_award.gameObject.SetActive(true);
            txt_gameEnd.SetText("赢");

        }
        PanelManager.Instance.OpenSingle<AwardView>(trans_awardGrid, new AwardData(AwardType.Property, (int)PropertyIdType.Score, score));
        addBtnListener(btn_gameEndLeave, () =>
        {
            //直接结算
            PanelManager.Instance.ClosePanel(this);
        });


    }

    

    public override void OnClose()
    {
        base.OnClose();
        EventCenter.Remove(TheEventType.BattleEnd, OnBattleEnd);
        EventCenter.Remove(TheEventType.BattleHit, OnHit);

    }
    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(trans_awardGrid);
    }
    //void StartBattle()
}
