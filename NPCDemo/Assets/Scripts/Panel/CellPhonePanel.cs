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
    public override void Init(params object[] args)
    {
        base.Init(args);
        EventCenter.Register(TheEventType.SendWetalkMessage, OnSendMessage);
        Show(CellPhoneShowType.Message);
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
        }

    }

    /// <summary>
    /// 显示聊天页面
    /// </summary>
    public void ShowChat(SinglePeopleChatData singlePeopleChatData)
    {
        ClearChatGrid();
        trans_chat.gameObject.SetActive(true);
        People people = RoleManager.Instance.FindPeopleWithOnlyId(singlePeopleChatData.Belong);
        txt_chatLabel.SetText(people.protoData.Name);
        for(int i=0;i< singlePeopleChatData.ChatDataList.Count; i++)
        {
            PanelManager.Instance.OpenSingle<SingleChatItemView>(trans_chatGrid, singlePeopleChatData.ChatDataList[0]);

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
            PanelManager.Instance.OpenSingle<SingleWetalkFriendInFriendListView>(trans_friendListGrid, singlePeopleChatData);
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
    void OnSendMessage(object param)
    {
        WetalkMsgData wetalkMsgData = param as WetalkMsgData;
        //红点
        for(int i=0;i< messageFriendViewList.Count; i++)
        {
            if (messageFriendViewList[i].singlePeopleChatData.Belong == wetalkMsgData.from.protoData.OnlyId)
            {
                messageFriendViewList[i].RedPointShow();
            }
        }

        RedPointShow();

        //如果是在聊天页面，且正在和他聊天 则已读
        if (curShowType == CellPhoneShowType.Chat)
        {
            if (curSinglePeopleChatData.Belong == wetalkMsgData.from.protoData.OnlyId)
            {
                SocializationManager.Instance.CheckedChat(curSinglePeopleChatData);
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