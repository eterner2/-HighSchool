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


        static string testEnemyNumerialName = "testEnemyNumerialSetting.json";
        static public Dictionary<int, TestEnemyNumerialSetting> testEnemyNumerialDic = new Dictionary<int, TestEnemyNumerialSetting>();
        static public List<TestEnemyNumerialSetting> _testEnemyNumerialList = new List<TestEnemyNumerialSetting>();


        static string testNumerialName = "testNumerialSetting.json";
        static public Dictionary<int, TestNumerialSetting> testNumerialDic = new Dictionary<int, TestNumerialSetting>();
        static public List<TestNumerialSetting> _testNumerialList = new List<TestNumerialSetting>();


        static string examName = "examSetting.json";
        static public Dictionary<int, ExamSetting> examDic = new Dictionary<int, ExamSetting>();
        static public List<ExamSetting> _examList = new List<ExamSetting>();

        static string peopleUpgradeName = "peopleUpgradeSetting.json";
        static public Dictionary<int, PeopleUpgradeSetting> peopleUpgradeDic = new Dictionary<int, PeopleUpgradeSetting>();
        static public List<PeopleUpgradeSetting> _peopleUpgradeList = new List<PeopleUpgradeSetting>();

        static string physicalUpgradeNumerialName = "physicalUpgradeNumerialSetting.json";
        static public Dictionary<int, PhysicalUpgradeNumerialSetting> physicalUpgradeNumerialDic = new Dictionary<int, PhysicalUpgradeNumerialSetting>();
        static public List<PhysicalUpgradeNumerialSetting> _physicalUpgradeNumerialList = new List<PhysicalUpgradeNumerialSetting>();

        static string artUpgradeNumerialName = "artUpgradeNumerialSetting.json";
        static public Dictionary<int, ArtUpgradeNumerialSetting> artUpgradeNumerialDic = new Dictionary<int, ArtUpgradeNumerialSetting>();
        static public List<ArtUpgradeNumerialSetting> _artUpgradeNumerialList = new List<ArtUpgradeNumerialSetting>();

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

            //行动
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

            //测试数值
            string testNumerialfilePath = filePathPre + testNumerialName;
            string testNumerialStr = File.ReadAllText(testNumerialfilePath);

            if (!string.IsNullOrEmpty(testNumerialStr))
            {
                _testNumerialList = JsonMapper.ToObject<List<TestNumerialSetting>>(testNumerialStr);
                foreach (TestNumerialSetting temp in _testNumerialList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!testNumerialDic.ContainsKey(theID))
                    {
                        testNumerialDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("testNumerialAsset为空");
            }

            //测试数值
            string testEnemyNumerialfilePath = filePathPre + testEnemyNumerialName;
            string testEnemyNumerialStr = File.ReadAllText(testEnemyNumerialfilePath);

            if (!string.IsNullOrEmpty(testEnemyNumerialStr))
            {
                _testEnemyNumerialList = JsonMapper.ToObject<List<TestEnemyNumerialSetting>>(testEnemyNumerialStr);
                foreach (TestEnemyNumerialSetting temp in _testEnemyNumerialList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!testEnemyNumerialDic.ContainsKey(theID))
                    {
                        testEnemyNumerialDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("testEnemyNumerialAsset为空");
            }

            //考试数值
            string examfilePath = filePathPre + examName;
            string examStr = File.ReadAllText(examfilePath);

            if (!string.IsNullOrEmpty(examStr))
            {
                _examList = JsonMapper.ToObject<List<ExamSetting>>(examStr);
                foreach (ExamSetting temp in _examList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!examDic.ContainsKey(theID))
                    {
                        examDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("examAsset为空");
            }

            //升级所需经验
            string peopleUpgradefilePath = filePathPre + peopleUpgradeName;
            string peopleUpgradeStr = File.ReadAllText(peopleUpgradefilePath);

            if (!string.IsNullOrEmpty(peopleUpgradeStr))
            {
                _peopleUpgradeList = JsonMapper.ToObject<List<PeopleUpgradeSetting>>(peopleUpgradeStr);
                foreach (PeopleUpgradeSetting temp in _peopleUpgradeList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!peopleUpgradeDic.ContainsKey(theID))
                    {
                        peopleUpgradeDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("peopleUpgradeAsset为空");
            }

            //体育数值
            string physicalUpgradeNumerialfilePath = filePathPre + physicalUpgradeNumerialName;
            string physicalUpgradeNumerialStr = File.ReadAllText(physicalUpgradeNumerialfilePath);

            if (!string.IsNullOrEmpty(physicalUpgradeNumerialStr))
            {
                _physicalUpgradeNumerialList = JsonMapper.ToObject<List<PhysicalUpgradeNumerialSetting>>(physicalUpgradeNumerialStr);
                foreach (PhysicalUpgradeNumerialSetting temp in _physicalUpgradeNumerialList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!physicalUpgradeNumerialDic.ContainsKey(theID))
                    {
                        physicalUpgradeNumerialDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("physicalUpgradeNumerialAsset为空");
            }


            //艺术数值
            string artUpgradeNumerialfilePath = filePathPre + artUpgradeNumerialName;
            string artUpgradeNumerialStr = File.ReadAllText(artUpgradeNumerialfilePath);

            if (!string.IsNullOrEmpty(artUpgradeNumerialStr))
            {
                _artUpgradeNumerialList = JsonMapper.ToObject<List<ArtUpgradeNumerialSetting>>(artUpgradeNumerialStr);
                foreach (ArtUpgradeNumerialSetting temp in _artUpgradeNumerialList)
                {
                    int theID;
                    if (!int.TryParse(temp.id, out theID))
                    {
                        Debug.LogError("该ID无法转为int，表名为" + temp);
                        return;
                    }
                    if (!artUpgradeNumerialDic.ContainsKey(theID))
                    {
                        artUpgradeNumerialDic.Add(theID, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("artUpgradeNumerialAsset为空");
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

        /// <summary>
        /// 获取单个敌人测试数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static TestEnemyNumerialSetting FindTestEnemyNumerialByLevel(int level)
        {
            for(int i = 0; i < _testEnemyNumerialList.Count; i++)
            {
                TestEnemyNumerialSetting setting = _testEnemyNumerialList[i];
                if (setting.level.ToInt32() == level)
                    return setting;
            }
            Debug.Log("寻找一个不存在的 TestEnemyNumerialSetting，level为" + level);

            return null;
        }


        /// <summary>
        /// 获取单个敌人测试数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static TestEnemyNumerialSetting FindTestEnemyNumerial(int id)
        {
            if (testEnemyNumerialDic.ContainsKey(id))
                return testEnemyNumerialDic[id];
            else
            {
                Debug.Log("寻找一个不存在的 TestEnemyNumerialSetting，id为" + id);
                return null;
            }
        }

        /// <summary>
        /// 获取测试数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static TestNumerialSetting FindTestNumerial(int id)
        {
            if (testNumerialDic.ContainsKey(id))
                return testNumerialDic[id];
            else
            {
                Debug.Log("寻找一个不存在的 testNumerialDic，id为" + id);
                return null;
            }
        }

        /// <summary>
        /// 通过等级获取测试数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static TestNumerialSetting FindTestNumerialByLevel(int level)
        {
            for (int i = 0; i < _testNumerialList.Count; i++)
            {
                TestNumerialSetting setting = _testNumerialList[i];
                if (setting.level.ToInt32() == level)
                    return setting;
            }
            Debug.Log("寻找一个不存在的 TestNumerialSetting，level为" + level);

            return null;
        }
        /// <summary>
        /// 通过等级获取体育数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static PhysicalUpgradeNumerialSetting FindPhysicalUpgradeNumerialByLevel(int level)
        {
            for (int i = 0; i < _physicalUpgradeNumerialList.Count; i++)
            {
                PhysicalUpgradeNumerialSetting setting = _physicalUpgradeNumerialList[i];
                if (setting.level.ToInt32() == level)
                    return setting;
            }
            Debug.Log("寻找一个不存在的 _physicalUpgradeNumerialList，level为" + level);

            return null;
        }

        /// <summary>
        /// 通过等级获取艺术数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static ArtUpgradeNumerialSetting FindArtUpgradeNumerialByLevel(int level)
        {
            for (int i = 0; i < _artUpgradeNumerialList.Count; i++)
            {
                ArtUpgradeNumerialSetting setting = _artUpgradeNumerialList[i];
                if (setting.level.ToInt32() == level)
                    return setting;
            }
            Debug.Log("寻找一个不存在的 ArtUpgradeNumerialSetting，level为" + level);

            return null;
        }

        /// <summary>
        /// 找考试数值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static ExamSetting FindExamSetting(int id)
        {
            if (examDic.ContainsKey(id))
                return examDic[id];
            else
            {
                Debug.Log("寻找一个不存在的 examDic，id为" + id);
                return null;
            }
        }
    }

}
