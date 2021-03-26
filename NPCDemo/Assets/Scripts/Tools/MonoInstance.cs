using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 继承mono的单例类
/// </summary>
public abstract class MonoInstance<T> : MonoBehaviour where T : MonoInstance<T>
{
    public bool initOk = false;
    static T inst=null;
    public static T Instance
    {
        get {

            if (inst == null)
            {
                inst = new GameObject(typeof(T).Name).AddComponent<T>();
                //inst.Init();
            }
              
            return inst;
        
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        inst = this as T;
    }

    public virtual void Init()
    {
        initOk = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
