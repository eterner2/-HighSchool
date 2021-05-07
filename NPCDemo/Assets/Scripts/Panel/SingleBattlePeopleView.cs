using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleBattlePeopleView : SingleViewBase
{
    public SingleExamEnemy singleEnemy;
    public PropertyData pro;
    public Image img_hpBar;
    public Image img_processBar;
    public Text txt_hp;
    public Text txt_name;

    public int curHp;
    public int initHp;

    bool startBattle;

    public float curAttackTime;
    public float curAttackTimer = 0;
    public BattlePanel parentPanel;
    public Transform trans_hitEffectParent;

    public override void Init(params object[] args)
    {
        base.Init(args);
        singleEnemy = args[0] as SingleExamEnemy;
        parentPanel = args[1] as BattlePanel;
  

        startBattle = false;


        if (singleEnemy != null)
            pro = singleEnemy.Property;
        else
            pro = RoleManager.Instance.playerPeople.protoData.PropertyData;
        float proNumSpeed= BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, pro).PropertyNum;
        curAttackTime = (1 / (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, pro).PropertyNum)) * parentPanel.basicAttackSpeed;
        Show();
        //txt_hp.SetText(propertyData.exam)
    }

    public void Show()
    {
        if (singleEnemy==null)
        {
            txt_name.SetText("我");
        }
        else
        {
         
            txt_name.SetText("黄冈选择题1号Lv" + parentPanel.enemyLevel);
        }
        curHp = (int)BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, pro).PropertyNum;
        initHp = (int)BattleManager.Instance.GetInitExamPropertyById(PropertyIdType.Hp, pro).PropertyNum;
        txt_hp.SetText(curHp + "/" + initHp);
        img_hpBar.fillAmount = curHp / (float)initHp;
    }


    private void Update()
    {
        if (startBattle)
        {
            curAttackTimer += Time.deltaTime;
            if (curAttackTimer >= curAttackTime)
            {
                //攻击
                parentPanel.Attack(this);
                curAttackTimer = 0;
                img_processBar.fillAmount = 0;
            }
            img_processBar.fillAmount = curAttackTimer / curAttackTime;

        }

    }

    ///// <summary>
    ///// 设置测试数据
    ///// </summary>
    //public void SetTestData(PropertyData thePro)
    //{
    //    curAttackTime = (1/(BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, thePro).PropertyNum)) *parentPanel.basicAttackSpeed;
    //    curHp = (int)BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, thePro).PropertyNum;
    //    initHp = (int)BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, thePro).PropertyNum;
    //    Show();

    //}

    public void StartBattle()
    {
        startBattle = true;
        curAttackTimer = 0;
        img_processBar.fillAmount = 0;
        //curAttackSpeed= (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, propertyData).PropertyNum) * basicAttackSpeed;
        //curHp = (int)BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, propertyData).PropertyNum;
        //initHp = (int)BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, propertyData).PropertyNum;
        //Show();


    }
    /// <summary>
    /// 被打
    /// </summary>
    public void OnHit(HitData hitData)
    {
        EntityManager.Instance.GenerateEntity<BattleHitEffect>(trans_hitEffectParent,ConstantVal.battleHitEffectPath);
        EntityManager.Instance.GenerateEntity<LoseHPEffect>(trans_hitEffectParent, ConstantVal.loseHPEffectPath,
            trans_hitEffectParent.position,hitData.num);
        Show();
    }

    public void OnEnd()
    {
        startBattle = false;
        img_processBar.fillAmount = 0;
    }
}
