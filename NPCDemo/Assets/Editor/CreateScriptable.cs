using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateScriptable
{
    [MenuItem("Assets/ExportPeopleScriptable")]

    public static void Execute()
    {
        PeopleScriptable p = ScriptableObject.CreateInstance<PeopleScriptable>();//创建Test的一个实例
                                                      //设置一些参数
       // test.testString = "Test String";
        //创建资源文件,这时会在监视面板中看到并且可以直接编辑数据啦！！！！
        AssetDatabase.CreateAsset(p, "Assets/Resources/PeopleScriptable.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ExportActionScriptable")]
    public static void ExecuteAction()
    {
        ActionScriptable p = ScriptableObject.CreateInstance<ActionScriptable>();//创建Test的一个实例
                                                                                 //设置一些参数
                                                                                 // test.testString = "Test String";
                                                                                 //创建资源文件,这时会在监视面板中看到并且可以直接编辑数据啦！！！！
        AssetDatabase.CreateAsset(p, "Assets/Resources/ActionScriptable.asset");
        AssetDatabase.Refresh();
    }


    //[MenuItem("配置表生成/生成Actionscriptable")]
    //public static void CreateTestScriptable()
    //{
    //    DirectoryInfo info = new DirectoryInfo("Assets/Resources");
    //    if (!info.Exists)
    //    {
    //        info.Create();
    //    }

    //    string sFullName = Path.Combine("Assets/Resources", "ActionScriptable.asset");

    //    ActionScriptable actionScriptable = ScriptableObject.CreateInstance<ActionScriptable>();


    //    AssetDatabase.CreateAsset(actionScriptable, sFullName);
    //}
}
