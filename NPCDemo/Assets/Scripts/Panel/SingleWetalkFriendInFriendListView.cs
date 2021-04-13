using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleWetalkFriendInFriendListView : SingleViewBase
{
    public Image img_icon;
    public Text txt_name;
    public Text txt_content;//后续可以是说说

    SinglePeopleChatData singlePeopleChatData;
    public CellPhonePanel parentPanel;
    public Button btn;
    public override void Init(params object[] args)
    {
        base.Init(args);
        singlePeopleChatData = args[0] as SinglePeopleChatData;
        parentPanel = args[1] as CellPhonePanel;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        Show();
        addBtnListener(btn, () =>
        {
            //如果是邀请 TODO如果已邀请自己的人也做同样的事情 需要过滤掉
            if (parentPanel.curHandleType == CellphoneHandleType.Invite)
            {
                RoleManager.Instance.playerPeople.protoData.ActionId = SocializationManager.Instance.tmpPreferedActionId;
                People people = RoleManager.Instance.FindPeopleWithOnlyId(singlePeopleChatData.Belong);
                ActionSetting actionSetting = Framework.Data.DataTable.FindActionSetting(SocializationManager.Instance.tmpPreferedActionId);
                PanelManager.Instance.OpenCommonHint("确定邀请" + people.protoData.Name + actionSetting.name + "吗？", () =>
                      {
                          //打开聊天面板
                          parentPanel.ShowChat(singlePeopleChatData);
                          SocializationManager.Instance.InviteNPC(people);
                      },
                null);
            }
        });
    }


    void Show()
    {
        People people = RoleManager.Instance.FindPeopleWithOnlyId(singlePeopleChatData.Belong);
        if (people.protoData.Gender == (int)Gender.Male)
        {
            img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.maleIcon);

        }
        else
        {
            img_icon.sprite = ResourceManager.Instance.GetObj<Sprite>(ConstantVal.femaleIcon);

        }
        txt_name.SetText(people.protoData.Name);
   
    }
}
