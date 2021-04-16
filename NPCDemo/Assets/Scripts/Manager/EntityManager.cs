using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoInstance<EntityManager>
{
    /// <summary>
    /// 一个实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent">父对象</param>
    /// <param name="args">参数</param>
    public T GenerateEntity<T>(Transform parent, params object[] args) where T : MonoBehaviour,Entity
    {
        if (parent == null)
            return default(T);

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
        t.Init(args);
        return t;
    }


    /// <summary>
    /// 关闭实体 进对象池
    /// </summary>
    public void CloseEntity(Entity entity)
    {
        entity.OnClose();
        ObjectPoolManager.Instance.DisappearObjectToPool(entity.objType, entity.obj,entity.isTmpObj);
    }
}
