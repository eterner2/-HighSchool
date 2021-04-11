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
    OneChatData oneChatData;
    public override void Init(object[] args)
    {
        base.Init(args);

        oneChatData = args[0] as OneChatData;
        //bool isPlayer = false;

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
    /// 点击头像 弹出选择框
    /// </summary>
    public void OnClickIcon()
    {
        //PanelMgr.Instance.OpenPanel<ReplyChoosePanel>(PanelMgr.Instance.GetPanelParent(PanelParentType.None),
        //    this.btn_head.transform.position,
        //    speakData);
    }


}
