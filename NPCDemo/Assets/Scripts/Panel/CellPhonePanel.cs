using RoleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellPhonePanel : PanelBase
{

    public Transform trans_message;//消息页面
    public Transform grid_message;//消息页面格子
    public Transform trans_friendList;//通讯录页面
    public Transform trans_friendListGrid;//通讯录页面格子
    public Transform trans_chat;//聊天页面
    public Transform trans_chatGrid;//聊天页面格子
    public List<SingleChatItemView> allChatItemViewList = new List<SingleChatItemView>();//所有聊天页面
    public RectTransform rectTrans_chatScroll;//聊天界面滑动条
    public Text txt_chatLabel;//聊天标题

    public Text txt_upLabel;//顶部标题


    public List<SingleWetalkFriendView> messageFriendViewList = new List<SingleWetalkFriendView>();//消息页面
    
    public SinglePeopleChatData curSinglePeopleChatData;

    public CellPhoneShowType curShowType;

    public Button btn_messagePage;//聊天页面
    public Image img_messagePageBtn;//聊天页面icon
    public Button btn_friendListPage;//联系人页面
    public GameObject obj_messageBtnRedPoint;//聊天页面红点
    public Image img_friendListBtn;//联系人页面

    public float chatReactTime = 3;//聊天反应延迟时间1秒
    public float chatReactTimer = 0;//聊天反应延迟时间
    public bool startWaitChatInput = false;//开始等待聊天输入
    public WetalkMsgData waitToShowChatMsg;//等待完将显示的

    public CellphoneHandleType curHandleType;//当前操作类型
    public override void Init(params object[] args)
    {
        base.Init(args);
        curHandleType = (CellphoneHandleType)args[0];
        EventCenter.Register(TheEventType.SendWetalkMessage, OnSendMessage);
        switch (curHandleType)
        {
            case CellphoneHandleType.Common:
                Show(CellPhoneShowType.Message);
                break;
            case CellphoneHandleType.Invite:
                Show(CellPhoneShowType.FriendList);
                break;
        }
        RedPointShow();
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        addBtnListener(btn_messagePage, () => Show(CellPhoneShowType.Message));
        addBtnListener(btn_friendListPage, () => Show(CellPhoneShowType.FriendList));

    }


    void Show(CellPhoneShowType cellPhoneShowType)
    {
        curShowType = cellPhoneShowType;
        switch (cellPhoneShowType)
        {
            case CellPhoneShowType.Message:
                ShowAllMessageWetalkFriend();
                break;
            case CellPhoneShowType.FriendList:
                ShowFriendList();
                break;
            case CellPhoneShowType.Chat:
                ShowChat(curSinglePeopleChatData);
                break;
            //case CellPhoneShowType.Invite:
            //    ShowFriendList();
            //    break;
        }

    }

    /// <summary>
    /// 显示聊天页面
    /// </summary>
    public void ShowChat(SinglePeopleChatData singlePeopleChatData)
    {
        curShowType = CellPhoneShowType.Chat;
        ClearChatGrid();
        trans_chat.gameObject.SetActive(true);
        People people = RoleManager.Instance.FindPeopleWithOnlyId(singlePeopleChatData.Belong);
        txt_chatLabel.SetText(people.protoData.Name);
        for(int i=0;i< singlePeopleChatData.ChatDataList.Count; i++)
        {
           allChatItemViewList.Add(PanelManager.Instance.OpenSingle<SingleChatItemView>(trans_chatGrid, singlePeopleChatData.ChatDataList[i]));

        }
        curSinglePeopleChatData = singlePeopleChatData;
        SocializationManager.Instance.CheckedChat(singlePeopleChatData);
        //自动定位
        if(rectTrans_chatScroll.sizeDelta.y<trans_chatGrid.GetComponent<RectTransform>().sizeDelta.y)
        {
            float offset = trans_chatGrid.GetComponent<RectTransform>().sizeDelta.y - rectTrans_chatScroll.sizeDelta.y;
            trans_chatGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(trans_chatGrid.localPosition.x, offset);
        }
        OnCheckedRedPoint(curSinglePeopleChatData);
    }

    /// <summary>
    /// 显示联系人页面
    /// </summary>
    public void ShowFriendList()
    {
        ClearFriendListGrid();

        trans_message.gameObject.SetActive(false);
        trans_chat.gameObject.SetActive(false);
        trans_friendList.gameObject.SetActive(true);
        img_friendListBtn.material = null;
        img_messagePageBtn.material = PanelManager.Instance.mat_grey;
        txt_upLabel.SetText("联系人");
        PanelManager.Instance.OpenSingle<FriendListLabelView>(trans_friendListGrid);
        for (int i = 0; i < RoleManager.Instance.playerPeople.protoData.AllSinglePeopleChatDataList.Count; i++)
        {
            SinglePeopleChatData singlePeopleChatData = RoleManager.Instance.playerPeople.protoData.AllSinglePeopleChatDataList[i];
            PanelManager.Instance.OpenSingle<SingleWetalkFriendInFriendListView>(trans_friendListGrid, singlePeopleChatData,this);
        }
    }

    //private void Update()
    //{
    //    //自动定位
    //    if (rectTrans_chatScroll.sizeDelta.y < trans_chatGrid.GetComponent<RectTransform>().sizeDelta.y)
    //    {
    //        float offset = trans_chatGrid.GetComponent<RectTransform>().sizeDelta.y - rectTrans_chatScroll.sizeDelta.y;
    //      //  trans_chatGrid.localPosition = new Vector2(trans_chatGrid.localPosition.x, offset);
    //        trans_chatGrid.GetComponent<RectTransform>().anchoredPosition= new Vector2(trans_chatGrid.localPosition.x, offset);
    //    }
    //}

    /// <summary>
    /// 显示消息页面所有人
    /// </summary>
    public void ShowAllMessageWetalkFriend()
    {
        ClearMessageGrid();
        img_messagePageBtn.material = null;
        img_friendListBtn.material = PanelManager.Instance.mat_grey;
        trans_friendList.gameObject.SetActive(false);
        trans_chat.gameObject.SetActive(false);
        trans_message.gameObject.SetActive(true);
        txt_upLabel.SetText("消息");
        for (int i = 0; i < RoleManager.Instance.playerPeople.protoData.AllSinglePeopleChatDataList.Count; i++)
        {
            SinglePeopleChatData singlePeopleChatData = RoleManager.Instance.playerPeople.protoData.AllSinglePeopleChatDataList[i];
            SingleWetalkFriendView view= PanelManager.Instance.OpenSingle<SingleWetalkFriendView>(grid_message, singlePeopleChatData,this);
            messageFriendViewList.Add(view);
        }
    }

    /// <summary>
    /// 已读
    /// </summary>
    void OnCheckedRedPoint(object param)
    {
        SinglePeopleChatData singlePeopleChatData = param as SinglePeopleChatData;
        //红点
        for (int i = 0; i < messageFriendViewList.Count; i++)
        {
            if (messageFriendViewList[i].singlePeopleChatData.Belong == singlePeopleChatData.Belong)
            {
                messageFriendViewList[i].RedPointShow();
            }
        }

        RedPointShow();
    }

    /// <summary>
    /// 发消息
    /// </summary>
    void OnSendMessage(object[] param)
    {
        WetalkMsgData wetalkMsgData = param[0] as WetalkMsgData;

        Debug.Log(wetalkMsgData);
        //红点
        for(int i=0;i< messageFriendViewList.Count; i++)
        {
            if (messageFriendViewList[i].singlePeopleChatData.Belong == wetalkMsgData.from.protoData.OnlyId)
            {
                messageFriendViewList[i].RedPointShow();
            }
        }

        RedPointShow();
        //重新赋值chatdata，并且上一条失效
        if (curSinglePeopleChatData.ChatDataList.Count > 1)
        {
            for(int i=0;i< allChatItemViewList.Count; i++)
            {
                allChatItemViewList[i].oneChatData = curSinglePeopleChatData.ChatDataList[i];
                allChatItemViewList[i].ValidShow();
            }
            //allChatItemViewList[curSinglePeopleChatData.ChatDataList.Count - 2].ValidShow();
            //curSinglePeopleChatData.ChatDataList.Count
        }

        //如果是在聊天页面，且正在和他聊天 则已读 并且这里做一个延迟显示的动画（仅显示
        if (curShowType == CellPhoneShowType.Chat)
        {      
            //如果是在聊天页面，且正在和他聊天 则已读 并且这里做一个延迟显示的动画（仅显示
            if (curSinglePeopleChatData.Belong == wetalkMsgData.from.protoData.OnlyId)
            {
                SocializationManager.Instance.CheckedChat(curSinglePeopleChatData);

                chatReactTimer = 0;
                startWaitChatInput = true;
                waitToShowChatMsg = wetalkMsgData;
                txt_chatLabel.SetText("对方正在输入中……");

            }
            //如果是玩家发的 则直接显示聊天内容
            if (curSinglePeopleChatData.Belong==wetalkMsgData.to.protoData.OnlyId
                &&wetalkMsgData.from.protoData.OnlyId == RoleManager.Instance.playerPeople.protoData.OnlyId)
            {
                AddAChat(wetalkMsgData);

            }

        }

    }
    /// <summary>
    /// 添加一项聊天
    /// </summary>
    void AddAChat(WetalkMsgData wetalkMsgData)
    {
        People people = RoleManager.Instance.FindPeopleWithOnlyId(curSinglePeopleChatData.Belong);

        txt_chatLabel.SetText(people.protoData.Name);

        PanelManager.Instance.OpenSingle<SingleChatItemView>(trans_chatGrid, curSinglePeopleChatData.ChatDataList[curSinglePeopleChatData.ChatDataList.Count-1]);
   
        //自动定位
        if (rectTrans_chatScroll.sizeDelta.y < trans_chatGrid.GetComponent<RectTransform>().sizeDelta.y)
        {
            float offset = trans_chatGrid.GetComponent<RectTransform>().sizeDelta.y - rectTrans_chatScroll.sizeDelta.y;
            trans_chatGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(trans_chatGrid.localPosition.x, offset);
        }
        OnCheckedRedPoint(curSinglePeopleChatData);
    }

    private void Update()
    {
        if (startWaitChatInput)
        {
            chatReactTimer += Time.deltaTime;
            if (chatReactTimer >= chatReactTime)
            {
                startWaitChatInput = false;
                AddAChat(waitToShowChatMsg);
            }
        }
    }

    /// <summary>
    /// 红点
    /// </summary>
    void RedPointShow()
    {
        RedPointManager.Instance.SetRedPointUI(obj_messageBtnRedPoint, RedPointType.AllChatMsg, 0);
        EventCenter.Broadcast(TheEventType.ShowMainPanelRedPoint);
    }

    public override void Clear()
    {
        base.Clear();
        ClearMessageGrid();
        ClearFriendListGrid();
        ClearChatGrid();
    }
    void ClearChatGrid()
    {
        startWaitChatInput = false;
        waitToShowChatMsg = null;
        allChatItemViewList.Clear();
        PanelManager.Instance.CloseAllSingle(trans_chatGrid);

    }
    void ClearFriendListGrid()
    {
        PanelManager.Instance.CloseAllSingle(trans_friendListGrid);
    }

    public void ClearMessageGrid()
    {
        PanelManager.Instance.CloseAllSingle(grid_message);
        messageFriendViewList.Clear();
    }

    public override void OnClose()
    {
        base.OnClose();
        EventCenter.Remove(TheEventType.SendWetalkMessage, OnSendMessage);
    }

}

public enum CellPhoneShowType
{
    None=0,
    Message,
    FriendList,
    Chat,
}

/// <summary>
/// 操作类型
/// </summary>
public enum CellphoneHandleType
{
    None=0,
    Common,
    Invite,
}