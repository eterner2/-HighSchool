// 由于单例基类不能实例化，故设计为抽象类
using System;

public abstract class CommonInstance<T> where T : class
{
    class Nested
    {
        // 创建模板类实例，参数2设为true表示支持私有构造函数
        internal static readonly T instance = Activator.CreateInstance(typeof(T), true) as T;
    }
    private static T instance = null;
    public static T Instance { get { return Nested.instance; } }

    bool initOk = false;
    public virtual void Init()
    {
        initOk = true;
    }
}