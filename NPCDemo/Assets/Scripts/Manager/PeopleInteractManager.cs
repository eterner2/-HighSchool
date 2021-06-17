using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInteractManager : CommonInstance<PeopleInteractManager>
{
    
    /// <summary>
    /// 要微信号
    /// </summary>
    public void AskForWetalkNum(People main,People beAsked)
    {
        //两人先交手一次
        SocializationManager.Instance.SocialAttack(main.protoData, beAsked.protoData);

        //如果是npc向玩家要微信
        if(beAsked.protoData.IsPlayer)
        {
            List<DialogData> dialogList = new List<DialogData>();
            dialogList.Add(new DialogData(main, "那个……能不能加个微信呀。"));
            DialogManager.Instance.CreateDialog(dialogList, "可以", () =>
            {
                //加微信逻辑
                AddedWetalk(main, beAsked);

                PanelManager.Instance.OpenFloatWindow("你已经成功添加" + beAsked.protoData.Name + "的微信");

            },
            "不行……",
            () =>
            {
                List<DialogData> dialogList2 = new List<DialogData>();
                dialogList2.Add(new DialogData(main, "那……不打扰你了。"));
                DialogManager.Instance.CreateDialog(dialogList2, null);
            }
            );
        }
        else
        {
            //由于是被要，好感度需求只要大于20则同意
            int index = beAsked.protoData.SensedOtherPeopleIdList.IndexOf(main.protoData.OnlyId);
            float val = beAsked.protoData.FriendlinessToSensedOtherPeopleList[index];


            if (val >= 20)
            {
                //如果是玩家向npc要微信 要有对话
                if (main.protoData.IsPlayer)
                {
                    List<DialogData> dialogList = new List<DialogData>();
                    dialogList.Add(new DialogData(beAsked, "喏，扫我的吧。"));
                    DialogManager.Instance.CreateDialog(dialogList, () =>
                    {
                        //加微信逻辑
                        AddedWetalk(main, beAsked);
                        PanelManager.Instance.OpenFloatWindow("你已经成功添加" + beAsked.protoData.Name + "的微信");

                    });
                }

            }
            //拒绝
            else
            {
                //如果是玩家向npc要微信，要有对话
                if (main.protoData.IsPlayer)
                {
                    List<DialogData> dialogList = new List<DialogData>();
                    dialogList.Add(new DialogData(beAsked, "我们好像不是很熟吧"));
                    DialogManager.Instance.CreateDialog(dialogList, () =>
                    {
                        PanelManager.Instance.OpenFloatWindow(beAsked.protoData.Name + "拒绝了你的请求");
                    });
                }

            }
        }


     
    }
    /// <summary>
    /// 普通闲聊
    /// </summary>
    /// <param name="main"></param>
    /// <param name="beAsked"></param>
    public void Chat(People main,People beAsked)
    {

        //如果是npc找玩家聊天
        if (beAsked.protoData.IsPlayer)
        {
            List<DialogData> dialogList = new List<DialogData>();
            dialogList.Add(new DialogData(main, "hello，你也在呀。"));
            DialogManager.Instance.CreateDialog(dialogList, "聊聊天", () =>
            {
               SocializationManager.Instance.SocialAttack(main.protoData, beAsked.protoData);


            },
            "算了……",
            () =>
            {
             
            }
            );
        }
        else
        {

            bool res = SocializationManager.Instance.SocialAttack(main.protoData, beAsked.protoData);

            if (main.protoData.IsPlayer)
            { //玩家找npc聊天
                PanelManager.Instance.PingPongBlackMask(null, () =>
                {
                    //对方好感度下降
                    if (!res)
                    {
                        List<DialogData> dialogList = new List<DialogData>();
                        dialogList.Add(new DialogData(beAsked, "和你聊天有点浪费时间。"));

                        DialogManager.Instance.CreateDialog(dialogList, () =>
                        {
                            PanelManager.Instance.OpenFloatWindow(beAsked.protoData.Name + "对你的好感度下降");

                        });
                    }
                    else
                    {
                        List<DialogData> dialogList = new List<DialogData>();
                        dialogList.Add(new DialogData(beAsked, "聊得很开心呢，谢谢你陪我聊天。"));

                        DialogManager.Instance.CreateDialog(dialogList, () =>
                        {
                            PanelManager.Instance.OpenFloatWindow(beAsked.protoData.Name + "对你的好感度提升了！");

                        });
                    }
                });
            }

        }
    }

    /// <summary>
    /// 加微信 2+1的微信
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public void AddedWetalk(People p1,People p2)
    {
        p1.protoData.WetalkFriends.Add(p2.protoData.OnlyId);
        p2.protoData.WetalkFriends.Add(p1.protoData.OnlyId);
        //有人和玩家加了微信 发信息到手机ui上
        if (p1.isPlayer)
        {
            WetalkMsgData wetalkMsgData = new WetalkMsgData(WetalkMsgType.Nonsense, "我们已经是好友了，以后请多多指教",p2,p1,0);
            SocializationManager.Instance.SendMsgToPlayer(p2,p1, wetalkMsgData);
        }
    }
}
