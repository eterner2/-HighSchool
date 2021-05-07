using DG.Tweening;
using RoleData;
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
    public float basicAttackSpeed = 5f;//基本速度

    public override void Init(params object[] args)
    {
        base.Init(args);
        this.curEnemy = args[0] as SingleExamEnemy;
        EventCenter.Register(TheEventType.BattleHit, OnHit);
        EventCenter.Register(TheEventType.BattleEnd, OnBattleEnd);

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

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        //delayStartTimer = 0;
        //startBattle = false;
        //startCalcDelay = true;
        VSAnimSingle.StartAnim();



        // property_player = RoleManager.Instance.playerPeople.protoData.PropertyData;
        BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.playerPeople.protoData.PropertyData);
        BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.examPropertyData);

        ////以下为测试数据
        //RoleManager.Instance.InitSingleProperty()


        playerBattleView = PanelManager.Instance.OpenSingle<SingleBattlePeopleView>(trans_playerPos, RoleManager.Instance.playerPeople.protoData.PropertyData,
            this);
        enemyBattleView= PanelManager.Instance.OpenSingle<SingleBattlePeopleView>(trans_enemyPos, RoleManager.Instance.examPropertyData,
        this);

        //enemyAttackSpeed = (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, property_enemy).PropertyNum / (float)100) * basicAttackSpeed;
        //playerAttackSpeed= (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, property_player).PropertyNum / (float)100) * basicAttackSpeed;
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


        playerBattleView.SetTestData(RoleManager.Instance.playerPeople.protoData.PropertyData);
        enemyBattleView.SetTestData(RoleManager.Instance.examPropertyData);
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
            BattleManager.Instance.Attack(RoleManager.Instance.playerPeople.protoData.PropertyData, RoleManager.Instance.examPropertyData);
        }
        else
        {
            BattleManager.Instance.Attack(RoleManager.Instance.examPropertyData,RoleManager.Instance.playerPeople.protoData.PropertyData);

        }

    }

    void OnHit(object[] param)
    {
        HitData hitData = param[0] as HitData;
        //玩家被打
        if (hitData.isPlayer)
        {
            playerBattleView.OnHit(hitData);
        }
        else
        {
            enemyBattleView.OnHit(hitData);
        }
    }

    void OnBattleEnd()
    {
        startBattle = false;
        startCalcDelay = false;
        playerBattleView.OnEnd();
        enemyBattleView.OnEnd();
    }

    public override void OnClose()
    {
        base.OnClose();
        EventCenter.Remove(TheEventType.BattleEnd, OnBattleEnd);
        EventCenter.Remove(TheEventType.BattleHit, OnHit);

    }
    //void StartBattle()
}
