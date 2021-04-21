using DG.Tweening;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePanel : PanelBase
{
    public float delayStartTime = 1f;//延迟多久以后开始
    public float delayStartTimer = 0;
    bool startCalcDelay;

    bool startBattle = false;
    public VSAnimSingle VSAnimSingle;

    //public PropertyData property_enemy;//敌人属性
    //public PropertyData property_player;//我的属性

    //public float basicAttackSpeed = 2f;//基本速度
    //public float enemyAttackSpeed;
    //public float enemyAttackTimer = 0;
    //public float playerAttackSpeed;
    //public float playerAttackTimer = 0;//
    public Transform trans_playerPos;
    public Transform trans_enemyPos;

    public SingleBattlePeopleView playerBattleView;
    public SingleBattlePeopleView enemyBattleView;

    public override void Init(params object[] args)
    {
        base.Init(args);
        EventCenter.Register(TheEventType.BattleHit, OnHit);
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        delayStartTimer = 0;
        startBattle = false;
        startCalcDelay = true;
        VSAnimSingle.StartAnim();



        // property_player = RoleManager.Instance.playerPeople.protoData.PropertyData;
        BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.playerPeople.protoData.PropertyData);
        BattleManager.Instance.InitCurExamPropertyData(RoleManager.Instance.examPropertyData);

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

        //if (startBattle)
        //{
        
        //}
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
            playerBattleView.OnHit();
        }
        else
        {
            enemyBattleView.OnHit();
        }
    }

    //void StartBattle()
}
