using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using UnityEditor;
using UnityEngine;

public class VersionEditor : MonoBehaviour
{
    
    [MenuItem("版本控制/打开本地目录")]
    static void OpenPersistentDataPath()
    {
        string path = Application.persistentDataPath;
        path = path.Replace("/", "\\");
        System.Diagnostics.Process.Start("explorer.exe", path);
    }

    [MenuItem("版本控制/复制表格到指定目录并生成version文件")]
    static void CopyTableAndGenerateVersion()
    {        
        //先把表格复制到持久化目录,然后生成版本控制文件，然后把bundle都压缩，最后把压缩文件，表格和version复制到流目录中
        string sourceTablePath = Application.streamingAssetsPath + "/res/DataTable";
        string destTablePath = Application.persistentDataPath + "/res/DataTable";

        // 老的删掉
        DeleteAllFile(destTablePath);
        //老的version删掉
        if (File.Exists(ConstantVal.GetVersionStreamPath()))
            File.Delete(ConstantVal.GetVersionStreamPath());
        //复制表格
        CopyDirectory(new DirectoryInfo(sourceTablePath), new DirectoryInfo(destTablePath));
        List<FileInfo> fileList = GetFile(Application.persistentDataPath,new List<FileInfo>());
        Debug.Log(fileList);
        //打好的bundle包转移到持久化目录,生成版本控制文件
        GenerateTheVersion(Application.persistentDataPath+"/res");

        //把版本控制文件也放到streamingasset
        FileInfo fileInfo = new FileInfo(ConstantVal.GetVersionPersistentPath());
        fileInfo.CopyTo(ConstantVal.GetVersionStreamPath());
        //将打好的bundle包压缩后，和版本控制文件一起放到stramingasset
        Debug.Log("版本文件生成成功");
    }

    /// <summary>
    /// 生成版本控制文件
    /// </summary>
    static void GenerateTheVersion(string thePath)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Generator = new System.Security.Cryptography.MD5CryptoServiceProvider();

        Dictionary<string, string> DicFileMD5 = new Dictionary<string,string>();
        //表格
        foreach (string filePath in Directory.GetFiles(thePath, "*.*", SearchOption.AllDirectories))
        {
            if (filePath.Contains(".meta") || filePath.Contains("TheVersion"))
                continue;
            
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash = md5Generator.ComputeHash(file);
           // List<string> tempList = new List<string>();
            string strMD5 = System.BitConverter.ToString(hash);
            strMD5 = strMD5.Replace("-", ""); //把“-”去掉

            file.Close();
            //key是"C:\Users\asus\AppData\LocalLow\DefaultCompany\xassetTest\res后面的东西"
            string key = filePath.Substring(filePath.IndexOf("res"));
            key = key.Replace('\\', '/');
            if (!DicFileMD5.ContainsKey(key))
                DicFileMD5.Add(key, strMD5);

        }

        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "TheVersion.txt");

        using (FileStream fs = new FileStream(savePath, FileMode.Create))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            foreach (KeyValuePair<string, string> pair in DicFileMD5)
            {
                string temp = pair.Key + ":" + pair.Value;
                sw.WriteLine(temp);
            }
        }
    }

    /// <summary>
    /// 删掉该文件夹中的所有文件和文件夹
    /// </summary>
    public static void DeleteAllFile(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        // 如果目标目录不存在，无视
        if (!dir.Exists)
        {
            return;
        }
        FileInfo[] fil = dir.GetFiles();
        DirectoryInfo[] dii = dir.GetDirectories();
        foreach (FileInfo f in fil)
        {
            f.Delete();
        
        }
        //获取子文件夹内的文件列表，递归遍历  
        foreach (DirectoryInfo d in dii)
        {
            DeleteAllFile(d.FullName);
        }
        dir.Delete();
    }

    /// <summary>  
    /// 获取路径下所有文件以及子文件夹中文件  
    /// </summary>  
    /// <param name="path">全路径根目录</param>  
    /// <param name="FileList">存放所有文件的全路径</param>  
    /// <param name="RelativePath"></param>  
    /// <returns></returns>  
    public static List<FileInfo> GetFile(string path,List<FileInfo> FileList)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] fil = dir.GetFiles();
        DirectoryInfo[] dii = dir.GetDirectories();
        foreach (FileInfo f in fil)
        {
            //int size = Convert.ToInt32(f.Length);  
            long size = f.Length;
            FileList.Add(f);//添加文件路径到列表中  
        }
        //获取子文件夹内的文件列表，递归遍历  
        foreach (DirectoryInfo d in dii)
        {
            GetFile(d.FullName, FileList);
        }
        return FileList;
    }

    /// <summary>
    /// 复制目录到目标目录
    /// </summary>
    public static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
    {
        // 如果两个目录相同，则无须复制
        if (destination.FullName.Equals(source.FullName))
        {
            return;
        }
        // 如果目标目录不存在，创建它
        if (!destination.Exists)
        {
            destination.Create();
        }
        // 复制所有文件
        FileInfo[] files = source.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.FullName.Contains(".meta"))
                continue;
            // 将文件复制到目标目录
            file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
        }
        // 处理子目录
        DirectoryInfo[] dirs = source.GetDirectories();
        foreach (DirectoryInfo dir in dirs)
        {
            string destinationDir = Path.Combine(destination.FullName, dir.Name);
            // 递归处理子目录
            CopyDirectory(dir, new DirectoryInfo(destinationDir));
        }
    }
}
