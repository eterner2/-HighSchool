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
        //暂时50%同意 TODO引入数值
        int val = RandomManager.Next(0, 100);
        if (val < 50)
        {
            List<DialogData> dialogList = new List<DialogData>();
            dialogList.Add(new DialogData(beAsked, "喏，扫我的吧。"));
            DialogManager.Instance.CreateDialog(dialogList,()=> 
            {
                //加微信逻辑
                AddedWetalk(main,beAsked);
                PanelManager.Instance.OpenFloatWindow("你已经成功添加"+beAsked.protoData.Name+"的微信");

            });
        }
        else
        {
            List<DialogData> dialogList = new List<DialogData>();
            dialogList.Add(new DialogData(beAsked, "我们好像不是很熟吧"));
            DialogManager.Instance.CreateDialog(dialogList,()=> 
            {
                PanelManager.Instance.OpenFloatWindow(beAsked.protoData.Name+"拒绝了你的请求");
            });
        }
    }

    /// <summary>
    /// 加微信 2+1的微信
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public void AddedWetalk(People p1,People p2)
    {
        p1.weTalkFriends.Add(p2);
        p2.weTalkFriends.Add(p1);
        //有人和玩家加了微信 发信息到手机ui上
        if (p1.isPlayer)
        {
            WetalkMsgData wetalkMsgData = new WetalkMsgData(WetalkMsgType.Nonsense, "我们已经是好友了，以后请多多指教",p2,p1);
            SocializationManager.Instance.SendMsgToPlayer(p2,p1, wetalkMsgData);
        }
    }
}
