using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoInstance<PanelManager>
{
    Dictionary<ObjectPoolSingle, PanelBase> panelDic = new Dictionary<ObjectPoolSingle, PanelBase>();
    List<PanelBase> panelList = new List<PanelBase>();

    public Transform trans_commonPanelParent;//场景等适用于当前游戏模块的
    public Transform trans_layer2;//状态栏 菜单栏等即使切换场景也常显的
    public Transform trans_layer3;//提示框等需要置顶的

    public Material mat_grey;

    public override void Init()
    {
        base.Init();
        trans_commonPanelParent = GameObject.Find("Canvas/Panel/Layer1").transform;
        trans_layer2 = GameObject.Find("Canvas/Panel/Layer2").transform;
        trans_layer3 = GameObject.Find("Canvas/Panel/Layer3").transform;
        mat_grey = ResourceManager.Instance.GetObj<Material>("Material/UI_ImageGreyShader");
        InitPanel((GameModuleType)RoleManager.Instance._CurGameInfo.CurGameModule);

    }

    /// <summary>
    /// 切换游戏模块
    /// </summary>
    /// <param name="gameModuleType"></param>
    public void InitPanel(GameModuleType gameModuleType)
    {
        CloseAllPanel(trans_commonPanelParent);
        CloseAllPanel(trans_layer2);
        //CloseAllPanel(trans_layer3);
        //后续根据当前是什么场景
        switch (gameModuleType)
        {
            case GameModuleType.WeekDay:
                OpenPanel<WorkDayPanel>(trans_commonPanelParent);
                //OpenPanel<PropertyPanel>(trans_commonPanelParent);
                OpenPanel<DeskPanel>(trans_commonPanelParent);
                OpenPanel<StatusPanel>(trans_layer2);
                OpenPanel<MainPanel>(trans_layer2);

                break;
            case GameModuleType.Battle:
                break;
            case GameModuleType.Home:
                OpenPanel<HomePanel>(trans_commonPanelParent);
                OpenPanel<StatusPanel>(trans_layer2);
                OpenPanel<MainPanel>(trans_layer2);

                break;
            case GameModuleType.BigMap:
                OpenPanel<BigMapPanel>(trans_commonPanelParent);
                OpenPanel<StatusPanel>(trans_layer2);
                OpenPanel<MainPanel>(trans_layer2);

                break;
            case GameModuleType.SingleOutsideScene:

                OpenPanel<OutsidePanel>(trans_commonPanelParent, SocializationManager.Instance.action_planDic[RoleManager.Instance.playerPeople.protoData.ChoosedActionId]);
                //OpenPanel<BigMapPanel>(trans_commonPanelParent);
                OpenPanel<StatusPanel>(trans_layer2);
                OpenPanel<MainPanel>(trans_layer2);

                break;
        }
       

    } 


    /// <summary>
    /// 直接获取面板 最好是用事件系统
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public PanelBase GetPanel(ObjectPoolSingle singleType)
    {
        if (panelDic.ContainsKey(singleType))
            return panelDic[singleType];
        return null;
    }
    /// <summary>
    /// 直接获取面板 最好是用事件系统
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public T GetPanel<T>() where T:PanelBase
    {
        for(int i = 0; i < panelList.Count; i++)
        {
            PanelBase panel = panelList[i];
            if(panel is T)
            {
                return panel as T;
            }
        }
        return null;
        //if (panelDic.ContainsValue(T))
        //    return panelDic[singleType];
        //return null;
    }

    /// <summary>
    /// 打开一个小面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent">父对象</param>
    /// <param name="args">参数</param>
    public T OpenSingle<T>(Transform parent, params object[] args) where T : SingleViewBase
    {
        if (parent == null)
        {
            Debug.Log("在无父物体的位置创建single！！！"+ typeof(T).ToString());
        }
            //return null;

       

        string typeName = typeof(T).ToString();

        string path = ConstantVal.GetPanelPath(typeName);//mao 获取panel路径
        // GameObject plobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(path), parent);
        bool available = Enum.TryParse<ObjectPoolSingle>(typeName, out ObjectPoolSingle singleType);
        if (!available)
        {
            Debug.LogError("没有定义该类型的对象池枚举" + typeName);
        }
        GameObject plobj = ObjectPoolManager.Instance.GetObjcectFromPool(singleType, path, true);
        plobj.transform.SetParent(parent, false);
        plobj.name = typeName;
        T t = plobj.GetComponent<T>();
        if (null == t)
            t = plobj.AddComponent<T>();
        t.objType = singleType;
        t.isTmpObj = true;
        t.obj = plobj;
        t.Clear();
        t.Init(args);
        t.OnOpenIng();
        return t;
    }

    /// <summary>
    /// 打开一个面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent">父对象</param>
    /// <param name="args">参数</param>
    public T OpenPanel<T>(Transform parent, params object[] args) where T : PanelBase
    {
        if (parent == null)
            return null;

        string typeName = typeof(T).ToString();

        string path = ConstantVal.GetPanelPath(typeName);//mao 获取panel路径
        // GameObject plobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(path), parent);
        //ObjectPoolSingle res=ObjectPoolSingle.None;
        bool available = Enum.TryParse<ObjectPoolSingle>( typeName,out ObjectPoolSingle singleType);
        //res
        //ObjectPoolSingle singleType = (ObjectPoolSingle)Enum.Parse(typeof(ObjectPoolSingle), typeName);
        if (!available)
        {
            Debug.LogError("没有定义该类型的对象池枚举" + typeName);
        }
        GameObject plobj = ObjectPoolManager.Instance.GetObjcectFromPool(singleType, path, true);
        plobj.transform.SetParent(parent, false);
        plobj.name = typeName;
        T t = plobj.GetComponent<T>();
        if (null == t)
            t = plobj.AddComponent<T>();
        t.objType = singleType;
        t.isTmpObj = true;
        t.obj = plobj;
        t.Clear();
        t.Init(args);
        t.OnOpenIng();

        if (!panelDic.ContainsKey(singleType))
        {
            panelDic.Add(singleType,t);
        }

        if (!panelList.Contains(t))
        {
            panelList.Add(t);
        }
        return t;
    }



    /// <summary>
    /// 关闭面板
    /// </summary>
    /// <param name="panelBase"></param>
    public void ClosePanel(PanelBase panelBase)
    {
 
        panelBase.Clear();
        panelBase.RemoveBtnClick();
        if (panelDic.ContainsKey(panelBase.objType))
        {
            panelDic.Remove(panelBase.objType);
        }

        if (panelList.Contains(panelBase))
        {
            panelList.Remove(panelBase);
        }
        EntityManager.Instance.CloseEntity(panelBase);
    }

    /// <summary>
    /// 关闭某个父物体的所有面板
    /// </summary>
    public void CloseAllPanel(Transform trans)
    {
        int count = trans.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            PanelBase panel = trans.GetChild(i).GetComponent<PanelBase>();
            ClosePanel(panel);
        }
    }

    /// <summary>
    /// 关闭小面板
    /// </summary>
    /// <param name="panelBase"></param>
    public void CloseSingle(SingleViewBase singleViewBase)
    {

        singleViewBase.Clear();
        singleViewBase.RemoveBtnClick();
        EntityManager.Instance.CloseEntity(singleViewBase);
    }

    /// <summary>
    /// 关闭某个父物体所有小面板
    /// </summary>
    /// <param name="trans"></param>
    public void CloseAllSingle(Transform trans)
    {
        int count = trans.childCount;
        for(int i = count-1; i >= 0; i--)
        {
            SingleViewBase view = trans.GetChild(i).GetComponent<SingleViewBase>();
            CloseSingle(view);
        }
    }

    /// <summary>
    /// 黑幕降临
    /// </summary>
    public void BlackMask(BlackMaskType blackMaskType,Action finishCallBack)
    {
        BlackMaskPanel blackMaskPanel = GetPanel<BlackMaskPanel>();
        if (blackMaskPanel == null)
        {
            blackMaskPanel= PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_layer3, blackMaskType,finishCallBack);

        }
    }
    /// <summary>
    /// 黑幕暗-亮
    /// </summary>
    public void PingPongBlackMask( Action finishCallBack,Action closePanelCallBack)
    {
        BlackMaskPanel blackMaskPanel = GetPanel<BlackMaskPanel>();
        if (blackMaskPanel == null)
        {
            blackMaskPanel = PanelManager.Instance.OpenPanel<BlackMaskPanel>(PanelManager.Instance.trans_layer3, BlackMaskType.PingPong, finishCallBack, closePanelCallBack);

        }
    }

    public void OpenFloatWindow(string str)
    {
        OpenPanel<FloatWindowPanel>(trans_layer3, str);
    }

    /// <summary>
    /// 打开普通弹窗
    /// </summary>
    public void OpenCommonHint(string content, Action okCb, Action cancelCb, string okBtnTxt = "", string cancelBtnTxt = "")
    {
        HintData hintData = new HintData();
        hintData.content = content;
        hintData.okCallBack = okCb;
        hintData.cancelCallBack = cancelCb;
        hintData.str_okBtn = okBtnTxt;
        hintData.str_cancelBtn = cancelBtnTxt;
        OpenPanel<CommonHintPanel>(PanelManager.Instance.trans_layer2, hintData);
    }
}
