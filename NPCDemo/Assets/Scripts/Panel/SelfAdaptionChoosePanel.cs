using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfAdaptionChoosePanel : PanelBase
{
    public Vector3 contentPos;//位置
    public float rectXOffset;//x补偿
    public float rectYOffset=10;//y补偿
    //世界边界
    public float leftBorder;
    public float rightBorder;
    public float topBorder;
    public float downBorder;
    private float myWidth;
    private float myHeight;

    public Transform grid;

    protected List<SelfAdaptionChooseBtnView> selfAdaptionChooseBtnViewList = new List<SelfAdaptionChooseBtnView>();

    public override void Init(params object[] args)
    {
        base.Init(args);
        Vector3 theVec = (Vector3)args[0];
        this.contentPos = new Vector3(theVec.x, theVec.y,transform.position.z);
        //selfAdaptionChooseBtnViewList = args[1] as List<SelfAdaptionChooseBtnView>;


        //string[] actionIdArr = bigMapSetting.actions.Split('|');

        //for(int i = 0; i < actionIdArr.Length; i++)
        //{
        //    int theId = actionIdArr[i].ToInt32();
        //    PanelManager.Instance.OpenSingle<ChooseActionBtnView>(grid, theId);

        //}


      
    }




    public override void OnOpenIng()
    {
        base.OnOpenIng();
        Show();
        ShowPos();

    }

    public void Show()
    {
        int count = selfAdaptionChooseBtnViewList.Count;
        for(int i = 0; i < count; i++)
        {
            selfAdaptionChooseBtnViewList[i].Show();
        }

        float maxRtX = 0;
        float rtY = 0;
        //int count = grid.childCount;
        for (int i = 0; i < count; i++)
        {
            Vector2 sizeDelta = grid.GetChild(i).GetComponent<RectTransform>().sizeDelta;
            float theX = sizeDelta.x;
            if (theX >= maxRtX)
                maxRtX = theX;
            rtY += sizeDelta.y;
        }
        for (int i = 0; i < count; i++)
        {
            SelfAdaptionChooseBtnView view = grid.GetChild(i).GetComponent<SelfAdaptionChooseBtnView>();

            view.SetSize(new Vector2(maxRtX, view.rect.sizeDelta.y));
        }
        RectTransform rt = trans_content.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(maxRtX, rtY + rectYOffset);
    }
    /// <summary>
    /// 显示位置 如果这里出现出框bug 那就是锚点设置有问题content的锚点为x0y1
    /// </summary>
    void ShowPos()
    {
        //首先出现在原始位置
        trans_content.position =new Vector3(contentPos.x,contentPos.y,contentPos.z);
        //世界坐标右上角
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
         Mathf.Abs(-Camera.main.transform.position.z)));
        //世界坐标左边界
        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        //世界坐标右边界
        rightBorder = cornerPos.x;
        //世界坐标上边界
        topBorder = cornerPos.y;
        //世界坐标下边界
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);



        //index从0到3分别为 左下 左上 右上 右下的世界坐标
        Vector3[] corners = new Vector3[4];
        trans_content.GetComponent<RectTransform>().GetWorldCorners(corners);
        //width = rightBorder - leftBorder;
        //height = topBorder - downBorder;

        Vector3 leftDownVec = corners[0];//左下
        Vector3 leftUpVec = corners[1];//左上
        Vector3 rightUpVec = corners[2];//右上
        Vector3 rightDownVec = corners[3];//右下

        myWidth = rightUpVec.x - leftUpVec.x;
        myHeight = leftUpVec.y - leftDownVec.y;

        if (leftUpVec.y >= topBorder)
        {
           // Debug.Log("到达上边界");
            trans_content.position = new Vector3(trans_content.position.x, topBorder, 0);
        }
        //下
        if (leftDownVec.y <= downBorder)
        {
           // Debug.Log("到达下边界");
           // Debug.Log("原来的pos是" + trans_content.position.y);
            trans_content.position = new Vector3(trans_content.position.x, downBorder + myHeight, 0);
            //Debug.Log("新的pos是" + trans_content.position.y);

        }
        //左
        if (leftDownVec.x <= leftBorder)
        {
           // Debug.Log("到达左边界");

            trans_content.position = new Vector3(leftBorder + myWidth / 2, trans_content.position.y, 0);
        }
        //右
        if (rightDownVec.x >= rightBorder)
        {
           // Debug.Log("到达右边界");
            trans_content.position = new Vector3(rightBorder - myWidth / 2, trans_content.position.y, 0);

        }
    }


    public override void Clear()
    {
        base.Clear();
        PanelManager.Instance.CloseAllSingle(grid);
        selfAdaptionChooseBtnViewList.Clear();
    }
}
