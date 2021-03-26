using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoInstance<PanelManager>
{
    Dictionary<ObjectPoolSingle, PanelBase> panelDic = new Dictionary<ObjectPoolSingle, PanelBase>();
    List<PanelBase> panelList = new List<PanelBase>();

    public Transform trans_commonPanelParent;

    public override void Init()
    {
        base.Init();
        trans_commonPanelParent = GameObject.Find("Canvas/Panel").transform;
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
            return null;

       

        string typeName = typeof(T).ToString();

        string path = ConstantVal.GetPanelPath(typeName);//mao 获取panel路径
        // GameObject plobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(path), parent);
        ObjectPoolSingle singleType = (ObjectPoolSingle)Enum.Parse(typeof(ObjectPoolSingle), typeName);

        GameObject plobj = ObjectPoolManager.Instance.GetObjcectFromPool(singleType, path, false);
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
        ObjectPoolSingle singleType = (ObjectPoolSingle)Enum.Parse(typeof(ObjectPoolSingle), typeName);

        GameObject plobj = ObjectPoolManager.Instance.GetObjcectFromPool(singleType, path, false);
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
    /// 关闭小面板
    /// </summary>
    /// <param name="panelBase"></param>
    public void CloseSingle(SingleViewBase singleViewBase)
    {

        singleViewBase.Clear();
        singleViewBase.RemoveBtnClick();
        EntityManager.Instance.CloseEntity(singleViewBase);
    }
}
