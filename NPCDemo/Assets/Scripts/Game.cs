using Framework.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CopyFromStreamToPersist(TableOK));

        //List<int> test = new List<int>();
        //test.Insert(0, 111);
        //GameObject o = GameObject.Find(":sd ");
        //Debug.Log(o.transform);
    }
    //public float test;
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    PanelManager.Instance.OpenPanel<ChooseActionPanel>(PanelManager.Instance.trans_layer3);
        //}
    }

    /// <summary>
    /// tableok了 这里是初始化总入口
    /// </summary>
    public void TableOK()
    {
        DataTable.LoadTableData();
        RedPointManager.Instance.Init();
        RoleManager.Instance.Init(-1);
        SocializationManager.Instance.Init();
        GameModuleManager.Instance.Init();
        PanelManager.Instance.Init();
        GameTimeManager.Instance.Init();
        Test();
    }

    void Test()
    {
        PeopleInteractManager.Instance.AddedWetalk(RoleManager.Instance.playerPeople, RoleManager.Instance.allPeopleList[0]);
        PeopleInteractManager.Instance.AddedWetalk(RoleManager.Instance.playerPeople, RoleManager.Instance.allPeopleList[4]);
        PeopleInteractManager.Instance.AddedWetalk(RoleManager.Instance.playerPeople, RoleManager.Instance.allPeopleList[6]);
        PeopleInteractManager.Instance.AddedWetalk(RoleManager.Instance.playerPeople, RoleManager.Instance.allPeopleList[11]);
        PeopleInteractManager.Instance.AddedWetalk(RoleManager.Instance.playerPeople, RoleManager.Instance.allPeopleList[15]);

    }

    /// <summary>
    /// 判断持久化目录有没有version，如果没有说明还没复制过， 把streamingasset的文件放到持久化目录
    /// </summary>
    IEnumerator CopyFromStreamToPersist(Action okCallBack)
    {
        //对比 流目录version/服务器version 若一样 则拷贝version和所有table到持久化目录，然后不需要任何下载 进游戏完事了


        //若流目录version和服务器version不一样 则直接对比持久化目录和服务器version（这里先做判断，如果持久化目录没有version，则将流目录的version和table全部放到持久化目录，再对比持久化目录version和服务器version下载））




        //FileInfo fileInfo = new FileInfo(PathConstant.GetVersionPersistentPath());
        //持久化目录里面没有，需要复制 
        //if (!fileInfo.Exists)
        //{
        //先复制version 再复制version里面提到的
        FileInfo StreamfileInfo = new FileInfo(ConstantVal.GetVersionStreamPath());

        List<string> streamResPathList = new List<string>();//流目录下所有资源的位置
        //Debug.Log("尝试从流目录加载资源" + PathConstant.GetVersionStreamPath());
        WWW www = new WWW(ConstantVal.GetVersionStreamPath());
        yield return www;

        if (www.isDone)
        {
            //Debug.Log("流目录加载version成功！");

            using (var reader = new StringReader(www.text))
            {
                //s = reader.ReadToEnd();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(':');
                    if (fields.Length > 1)
                    {
                        streamResPathList.Add(fields[0]);
                    }
                }
            }
            //拷贝数据到指定路径
            File.WriteAllBytes(ConstantVal.GetVersionPersistentPath(), www.bytes);
            //Debug.Log("拷贝version到指定路径成功");
        }

        for (int i = 0; i < streamResPathList.Count; i++)
        {
            //这些path从version里面取
            string path = ConstantVal.GetFileInStreamPath(streamResPathList[i]);
            //"file://"+ Application.streamingAssetsPath + "/" + streamResPathList[i];
            string targetPath = Application.persistentDataPath + "/" + streamResPathList[i];

            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/res/DataTable");
            if (!info.Exists)
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/res/DataTable");

                // Debug.Log("创建datatable到持久化目录成功"+ Application.persistentDataPath + "/res/DataTable");

            }

            // Debug.Log("尝试从流目录加载" + path);

            WWW www2 = new WWW(path);
            //Debug.Log("流目录加载文件成功" + path);
            yield return www2;

            if (www2.isDone)
            {
                // Debug.Log("拷贝数据到指定路径" + targetPath);

                //拷贝数据到指定路径
                //string path = Application.persistentDataPath + "/" + "map_data.db";
                File.WriteAllBytes(targetPath, www2.bytes);

            }
        }
        Debug.Log("所有文件加载成功！");
        //}
        //持久化目录有，不复制，直接用持久化目录的version
        //else
        //{
        //    FileInfo fileInfo3 = new FileInfo(PathConstant.GetVersionPersistentPath());
        //    if (fileInfo3.Exists)
        //    {
        //        StreamReader r = new StreamReader(PathConstant.GetVersionPersistentPath());
        //        s = r.ReadToEnd();
        //    }
        //}

        //暂时不用加载服务器versino
        // StartCoroutine(LoadVersions(s));

        if (okCallBack != null)
            okCallBack();
    }

}
