using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleWetalkFriendView : SingleViewBase
{
    public Image img_icon;
    public Text txt_name;
    public Text txt_content;

    public SinglePeopleChatData singlePeopleChatData;
    public Button btn;
    public CellPhonePanel cellPhonePanel;
    public GameObject obj_redPoint;
    public Text txt_redPointNum;//红点数

    public override void Init(params object[] args)
    {
        base.Init(args);
        singlePeopleChatData = args[0] as SinglePeopleChatData;
        cellPhonePanel = args[1] as CellPhonePanel;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        Show();
        RedPointShow();
        addBtnListener(btn, () =>
        {
            cellPhonePanel.ShowChat(singlePeopleChatData);
        });
    }


    void Show()
    {
        People people = RoleManager.Instance.FindPeopleWithOnlyId(singlePeopleChatData.Belong);
        if (people.protoData.Gender==(int)Gender.Male)
        {
            img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.maleIcon);

        }
        else
        {
            img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.femaleIcon);

        }
        txt_name.SetText(people.protoData.Name);
        //聊天记录显示最近一条
        if (singlePeopleChatData.ChatDataList.Count > 0)
        {
            txt_content.SetText(singlePeopleChatData.ChatDataList[singlePeopleChatData.ChatDataList.Count - 1].Content);
        }
    }

    /// <summary>
    /// 红点
    /// </summary>
    public void RedPointShow()
    {
        RedPointManager.Instance.SetRedPointUI(obj_redPoint, RedPointType.SinglePeopleChatMsg, singlePeopleChatData.Belong);
        txt_redPointNum.SetText(SocializationManager.Instance.GetUnCheckChatNum(singlePeopleChatData.Belong).ToString());
        RectTransform rect = obj_redPoint.GetComponent<RectTransform>();
        rect.sizeDelta =new Vector2(rect.sizeDelta.x, txt_redPointNum.preferredWidth);

    }
}
