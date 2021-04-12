using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActionPanel : SelfAdaptionChoosePanel
{
    //public Vector3 contentPos;//位置
    //public float rectXOffset;//x补偿
    //public float rectYOffset=10;//y补偿
    ////世界边界
    //public float leftBorder;
    //public float rightBorder;
    //public float topBorder;
    //public float downBorder;
    //private float myWidth;
    //private float myHeight;

    //public Transform grid;

    public override void Init(params object[] args)
    {
        base.Init(args);
        //Vector3 theVec = (Vector3)args[0];
        //this.contentPos = new Vector3(theVec.x, theVec.y,transform.position.z);
        BigMapSetting bigMapSetting = args[1] as BigMapSetting;
        //string str1 = (string)args[1];
        //string str2 = (string)args[2];
        //string str3 = (string)args[3];

        string[] actionIdArr = bigMapSetting.actions.Split('|');

        //如果答应了和谁一起做事
        int appliedPeopleActionId = 0;
        if (RoleManager.Instance.playerPeople.protoData.AppliedInvitePeople != 0)
        {
            appliedPeopleActionId = RoleManager.Instance.FindPeopleWithOnlyId(RoleManager.Instance.playerPeople.protoData.AppliedInvitePeople).protoData.ActionId;
        }
        for(int i = 0; i < actionIdArr.Length; i++)
        {
            int theId = actionIdArr[i].ToInt32();
            if(appliedPeopleActionId==theId)  
                selfAdaptionChooseBtnViewList.Add(PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid,this, theId, RoleManager.Instance.playerPeople.protoData.AppliedInvitePeople));
            else
                selfAdaptionChooseBtnViewList.Add(PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, this, theId, 0));

        }

        //PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, "测试");
        //PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, "测试测");
        //PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, "测试测试");

        //float maxRtX = 0;
        //float rtY = 0;
        //int count = grid.childCount;
        //for(int i = 0; i < count; i++)
        //{
        //    Vector2 sizeDelta = grid.GetChild(i).GetComponent<RectTransform>().sizeDelta;
        //   float theX = sizeDelta.x;
        //    if (theX >= maxRtX)
        //        maxRtX = theX;
        //    rtY += sizeDelta.y;
        //}
        //for (int i = 0; i < count; i++)
        //{
        //    ChooseActionBtnView view = grid.GetChild(i).GetComponent<ChooseActionBtnView>();

        //    view.SetSize(new Vector2(maxRtX, view.rect.sizeDelta.y));
        //}
        //RectTransform rt = trans_content.GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(maxRtX, rtY+ rectYOffset);
    }



    public override void OnOpenIng()
    {
        base.OnOpenIng();

       // ShowPos();

    }

    /// <summary>
    /// 显示位置 如果这里出现出框bug 那就是锚点设置有问题content的锚点为x0y1
    /// </summary>
    void ShowPos()
    {

    }

   

}
