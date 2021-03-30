using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// mao资源管理器
/// </summary>
public class ResourceManager
{
    static ResourceManager inst;

    public static ResourceManager Instance
    {
        get
        {
            if (inst == null)
                inst = new ResourceManager();
            return inst;
        }
    }

    public static Dictionary<string,object> ResDic = new Dictionary<string, object>();

    /// <summary>
    /// 获取组件（物体）但不实例化，Todo接入对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T GetObj<T>(string path) where T : Object
    {
        //在这里判断t类型 todo 走字典
        return Resources.Load<T>(path) as T;
    }

    /// <summary>
    /// 移除组件
    /// </summary>
    public void RemoveObj()
    {

    }
   

}
