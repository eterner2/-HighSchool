

using UnityEngine;

public class ConstantVal
{

    /// <summary>
    /// 所有panel暂时放该文件夹
    /// </summary>
    public const string PanelPath = "UIPanel";

    /// <summary>
    /// 通过Panel名字获取路径
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public static string GetPanelPath(string panelName)
    {
        return PanelPath + "/" + panelName;
    }

    /// <summary>
    /// 获取文件在流目录的路径
    /// </summary>
    /// <returns></returns>
    public static string GetFileInStreamPath(string file)
    {
#if UNITY_IOS
        return "file://"+ Application.streamingAssetsPath + "/" + file;
#else
        return Application.streamingAssetsPath + "/" + file;
#endif
    }

    /// <summary>
    /// 获取文件在持久化目录的路径
    /// </summary>
    /// <returns></returns>
    public static string GetFileInPersistentPath(string file)
    {
        //#if UNITY_IOS
        // return "file://" + Application.persistentDataPath + "/" + file;
        //#else
        return Application.persistentDataPath + "/" + file;
        //#endif
    }

    /// <summary>
    /// 获取version的持久化目录
    /// </summary>
    /// <returns></returns>
    public static string GetVersionPersistentPath()
    {
        //#if UNITY_IOS
        return Application.persistentDataPath + "/theVersion.txt";

        //#else
        //return Application.persistentDataPath + "/theVersion.txt";
        //#endif
    }
    /// <summary>
    /// 获取version的流目录
    /// </summary>
    /// <returns></returns>
    public static string GetVersionStreamPath()
    {
#if UNITY_IOS
        return "file://" +Application.streamingAssetsPath + "/theVersion.txt";
#else
        return Application.streamingAssetsPath + "/theVersion.txt";
#endif
    }
}
