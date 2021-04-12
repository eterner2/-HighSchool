using Framework.Data;
using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleChatItemView : SingleViewBase
{
    //public SinglePeopleData peopleData;
    //public SpeakData speakData;

    public RectTransform rectTrans_txtBg;

    public RectTransform rectTrans_this;//它本身的大小

    public Text txt_name;
    public Image img_icon;
    public Text txt_content;

    //public Transform trans_pictureRoot;//图片信息
    //public Transform trans_wordRoot;//文字信息


    public Transform trans_enemyHeadPos;
    public Transform trans_myHeadPos;

    public Button btn_head;
    public OneChatData oneChatData;
    public Button btn_reply;//回复按钮
    public override void Init(object[] args)
    {
        base.Init(args);

        oneChatData = args[0] as OneChatData;
        //bool isPlayer = false;
        People people = RoleManager.Instance.FindPeopleWithOnlyId(oneChatData.Belong);
        txt_content.SetText(oneChatData.Content);
        
        SetHeight();
        if (!oneChatData.IsPlayer)
        {
            //txt_name.gameObject.SetActive(true);
            img_icon.transform.SetParent(trans_enemyHeadPos, false);
            img_icon.transform.localPosition = Vector2.zero;

        }
        else
        {
           // txt_name.gameObject.SetActive(false);
            img_icon.transform.SetParent(trans_myHeadPos,false);

            img_icon.transform.localPosition = Vector2.zero;

         
        }
        
        switch (oneChatData.ChatType)
        {
            case (int)WetalkMsgType.InviteAction:
                if(oneChatData.Valid)
                btn_reply.gameObject.SetActive(true);

                break;
            default:

                btn_reply.gameObject.SetActive(false);
                break;

        }

        addBtnListener(btn_reply, () =>
        {
            switch (oneChatData.ChatType)
            {
                case (int)WetalkMsgType.InviteAction:
                    ActionSetting actionSetting = DataTable.FindActionSetting(oneChatData.InviteActionId);
                    PanelManager.Instance.OpenCommonHint("是否答应和" + people.protoData.Name + actionSetting.name + "?"
                        ,()=> 
                        {
                            SocializationManager.Instance.ApplyInvite(people);
                        },
                        null,
                        "答应",
                        "拒绝");
                    break;
                default:
                    break;
            }

        });
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        People people = RoleManager.Instance.FindPeopleWithOnlyId(oneChatData.Belong);
        if (people.protoData.Gender == (int)Gender.Male)
        {
            img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.maleIcon);

        }
        else
        {
            img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.femaleIcon);

        }
    }

    public void SetHeight() 
    {
        rectTrans_txtBg.sizeDelta = new Vector2(rectTrans_txtBg.sizeDelta.x, txt_content.preferredHeight + 21);
        rectTrans_this.sizeDelta = new Vector2(rectTrans_this.sizeDelta.x, txt_content.preferredHeight + 81);
    }

    /// <summary>
    /// 失效
    /// </summary>
    public void ValidShow()
    {
        if(!oneChatData.Valid)
            btn_reply.gameObject.SetActive(false);

    }


    /// <summary>
    /// 点击头像 弹出选择框
    /// </summary>
    public void OnClickIcon()
    {
        //PanelMgr.Instance.OpenPanel<ReplyChoosePanel>(PanelMgr.Instance.GetPanelParent(PanelParentType.None),
        //    this.btn_head.transform.position,
        //    speakData);
    }


}
