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

        static string bigMapName = "bigMapSetting.json";
        static public Dictionary<int, BigMapSetting> bigMapDic = new Dictionary<int, BigMapSetting>();
        static public List<BigMapSetting> _bigMapList = new List<BigMapSetting>();

        static string actionName = "actionSetting.json";
        static public Dictionary<int, ActionSetting> actionDic = new Dictionary<int, ActionSetting>();
        static public List<ActionSetting> _actionList = new List<ActionSetting>();

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


            //大地图
            string bigMapfilePath = filePathPre + bigMapName;
            string bigMapStr = File.ReadAllText(bigMapfilePath);
            if (!string.IsNullOrEmpty(bigMapStr))
            {
                _bigMapList = JsonMapper.ToObject<List<BigMapSetting>>(bigMapStr);
                foreach (BigMapSetting temp in _bigMapList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!bigMapDic.ContainsKey(theID))
                    {
                        bigMapDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("bigMapAsset为空");
            }

            //行为
            string actionfilePath = filePathPre + actionName;
            string actionStr = File.ReadAllText(actionfilePath);
            if (!string.IsNullOrEmpty(actionStr))
            {
                _actionList = JsonMapper.ToObject<List<ActionSetting>>(actionStr);
                foreach (ActionSetting temp in _actionList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!actionDic.ContainsKey(theID))
                    {
                        actionDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("actionAsset为空");
            }

        }


        public static PropertySetting FindPropertySetting(int id)
        {
            if (propertyDic.ContainsKey(id))
                return propertyDic[id];
            else
            {
                Debug.Log("寻找一个不存在的PropertySetting，id为" + id);
                return null;
            }
        }

        public static BigMapSetting FindBigMapSetting(int id)
        {
            if (bigMapDic.ContainsKey(id))
                return bigMapDic[id];
            else
            {
                Debug.Log("寻找一个不存在的 bigMapSetting，id为" + id);
                return null;
            }
        }

        public static ActionSetting FindActionSetting(int id)
        {
            if (actionDic.ContainsKey(id))
                return actionDic[id];
            else
            {
                Debug.Log("寻找一个不存在的 actionSetting，id为" + id);
                return null;
            }
        }
    }

}
