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
    public override void Init(object[] args)
    {
        base.Init(args);

        //peopleData = args[0] as SinglePeopleData;
        //speakData = args[1] as SpeakData;

        bool isPlayer = false;

        //img_icon.sprite = Resources.Load<Sprite>(peopleData.iconName);
        //txt_content.text = speakData.content;
        //txt_name.text = speakData.belong.name;
        //是普通评论还是图片评论
     
            //trans_pictureRoot.gameObject.SetActive(false);
            //trans_wordRoot.gameObject.SetActive(true);
        
        SetHeight();
        if (!isPlayer)
        {
            txt_name.gameObject.SetActive(true);
            img_icon.transform.SetParent(trans_enemyHeadPos, false);
            img_icon.transform.localPosition = Vector2.zero;

        }
        else
        {
            txt_name.gameObject.SetActive(false);
            img_icon.transform.SetParent(trans_myHeadPos,false);

            img_icon.transform.localPosition = Vector2.zero;
        }
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
        addBtnListener(btn_head, OnClickIcon);
    }

    public void SetHeight() 
    {
        rectTrans_txtBg.sizeDelta = new Vector2(rectTrans_txtBg.sizeDelta.x, txt_content.preferredHeight + 21);
        rectTrans_this.sizeDelta = new Vector2(rectTrans_this.sizeDelta.x, txt_content.preferredHeight + 81);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
