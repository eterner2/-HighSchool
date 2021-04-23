using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleBattlePeopleView : SingleViewBase
{
    public PropertyData propertyData;

    public Image img_hpBar;
    public Text txt_hp;
    public Text txt_name;

    public int curHp;
    public int initHp;

    bool startBattle;

    public float basicAttackSpeed = 2f;//基本速度
    public float curAttackSpeed;
    public float curAttackTimer = 0;
    public BattlePanel parentPanel;
    public Transform trans_hitEffectParent;

    public override void Init(params object[] args)
    {
        base.Init(args);
        propertyData = args[0] as PropertyData;
        parentPanel = args[1] as BattlePanel;
        if (propertyData.IsPlayer)
        {
            txt_name.SetText("我");
        }
        else
        {
            txt_name.SetText("黄冈选择题1号");
        }
        curHp = BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Hp, propertyData).PropertyNum;
        initHp = BattleManager.Instance.GetInitExamPropertyById(PropertyIdType.Hp, propertyData).PropertyNum;
        startBattle = false;

        curAttackSpeed= (BattleManager.Instance.GetCurExamPropertyById(PropertyIdType.Speed, propertyData).PropertyNum / (float)100) * basicAttackSpeed;
        //curAttackTimer = 0;
        Show();
        //txt_hp.SetText(propertyData.exam)
    }

    void Show()
    {
        txt_hp.SetText(curHp + "/" + initHp);
        img_hpBar.fillAmount = curHp / (float)initHp;
    }


    private void Update()
    {
        if (startBattle)
        {
            curAttackTimer += Time.deltaTime;
            if (curAttackTimer >= curAttackSpeed)
            {
                //攻击
                parentPanel.Attack(this);
                curAttackTimer = 0;
            }

        }

    }

    public void StartBattle()
    {
        startBattle = true;
        curAttackTimer = 0;
    }
    /// <summary>
    /// 被打
    /// </summary>
    public void OnHit()
    {
        EntityManager.Instance.GenerateEntity<BattleHitEffect>(trans_hitEffectParent,ConstantVal.battleHitEffectPath);

    }
}
