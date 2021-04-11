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
    public override void Init(params object[] args)
    {
        base.Init(args);
        singlePeopleChatData = args[0] as SinglePeopleChatData;
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        Show();
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
