using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using LitJson;
using System.IO;
/// <summary>
/// mao 配置表加载
/// </summary>
namespace Framework.Data
{
    public class DataTable
    {
        static string propertyName = "propertySetting.json";
        static public Dictionary<int, PropertySetting> propertyDic = new Dictionary<int, PropertySetting>();
        static public List<PropertySetting> _propertyList = new List<PropertySetting>();

        public static void LoadTableData()
        {
            JsonMapper.RegisterImporter<int, long>((int value) =>
            {
                return (long)value;
            });
            string filePathPre = "";

#if UNITY_EDITOR
            filePathPre = Application.streamingAssetsPath + "/res/DataTable/";
#else
            filePathPre = Application.persistentDataPath + "/res/DataTable/";

#endif

            //属性
            string propertyfilePath = filePathPre + propertyName;
            string propertyStr = File.ReadAllText(propertyfilePath);
            if (!string.IsNullOrEmpty(propertyStr))
            {
                _propertyList = JsonMapper.ToObject<List<PropertySetting>>(propertyStr);
                foreach (PropertySetting temp in _propertyList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!propertyDic.ContainsKey(theID))
                    {
                        propertyDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("propertyAsset为空");
            }
        }

    }

}
