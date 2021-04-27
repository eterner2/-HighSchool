using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// 对象池单元
    /// </summary>
    public enum ObjectPoolSingle
    {
        None=0,
        PeopleView,
    SingleGroup,//一组人
    Text_singleRecord,//单个记录
    SingleRecordPanel,//记录面板
    BlackMaskPanel,//黑幕
    FlyTxtView,
    SinglePropertyView,//单个属性
    WorkDayPanel,//工作日主面板
    PropertyPanel,//属性面板
    DeskPanel,//桌面面板（放动画的）
    StatusPanel,//状态栏面板
    StatusPropertyView,//单个状态
    HomePanel,//在家的面板
    BigMapPanel,//大地图
    ChooseActionPanel,//选择行动面板
    ChooseActionBtnView,//选择行动按钮
    FloatWindowPanel,//飘窗
    SingleGroupView,//单个组合
    CommonHintPanel,//弹窗
    OutsidePanel,//外出场景
    ChoosePeopleInteractionBtnView,//选择和NPC交互的按钮
    ChoosePeopleInteractPanel,//选择和NPC交互的面板
    DialogPanel,//对话
    SingleWetalkFriendView,//微信单个消息
    FriendListLabelView,//好友标签
    SingleWetalkFriendInFriendListView,//通讯录的好友
    CellPhonePanel,//手机
    MainPanel,//主面板TODO和statuspanel做成同一个panel
    SingleChatItemView,//聊天面板
    ActionModulePeopleView,//行动模块的人
    ActionReadyPanelPeopleView,//行动准备模块的人
    ActionReadyPanel,//行动准备界面
    GetAwardPanel,//得奖
    BattlePanel,//战斗面板
    SingleBattlePeopleView,//单个战斗者
    BattleHitEffect,//受击特效
    LoseHPEffect,//掉血特效
}

public class TempItem
    {
       public GameObject obj;
       public long storeTime;
    }

    public class ObjectPoolManager : MonoBehaviour
    {

    static ObjectPoolManager inst;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (inst == null)
            {
                GameObject obj = new GameObject();
                obj.name = "ObjectPoolManager";
                inst = obj.AddComponent<ObjectPoolManager>();
                return inst;
            }
            return inst;
        }
    }
    private Dictionary<Type, Stack<MonoBehaviour>> ObjectDic = new Dictionary<Type, Stack<MonoBehaviour>>();
        private Dictionary<ObjectPoolSingle, List<GameObject>> EnumIndexObjectDic = new Dictionary<ObjectPoolSingle, List<GameObject>>();//如无必要，永久存储的对象
        private Dictionary<ObjectPoolSingle, List<TempItem>> tempEnumIndexObjectDic = new Dictionary<ObjectPoolSingle, List<TempItem>>();//临时存储的对象

        Dictionary<ObjectPoolSingle, List<GameObject>> des_Queue = new Dictionary<ObjectPoolSingle, List<GameObject>>();
        Dictionary<ObjectPoolSingle, List<TempItem>> tempDes_Queue = new Dictionary<ObjectPoolSingle, List<TempItem>>();

        public const int objCount = 100;

        const int tempObjStoreTime=60;//临时存储对象，60秒后销毁

        private void Awake()
        {
            //Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// 从对象池里面拿东西 枚举 物体名（路径） 是否临时对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectName">物体的全路径</param>
        /// <returns></returns>
        public GameObject GetObjcectFromPool(ObjectPoolSingle objType, string objectName,bool isTemp)
        {
            object obj = null;
        GameObject gameObj = null;
            if (!isTemp)
            {
                if (EnumIndexObjectDic.ContainsKey(objType))
                {
                    if (EnumIndexObjectDic[objType].Count == 0)
                    {
                        if (Resources.Load(objectName) == null)
                        {
                            Debug.LogError(string.Format("找不到名字为{0}的物体", objectName));
                            return null;
                        }
                        obj = GameObject.Instantiate(Resources.Load(objectName));
                    }
                    if (EnumIndexObjectDic[objType].Count > 0)
                    {
                        //拿走该对象
                        obj = EnumIndexObjectDic[objType][0];
                        EnumIndexObjectDic[objType].RemoveAt(0);
                    }
                }
                else
                {
                    //先从回收站取
                    if (des_Queue.ContainsKey(objType) && des_Queue[objType].Count > 0)
                    {
                        int index = des_Queue[objType].Count;
                        obj = des_Queue[objType][index - 1];
                    }
                    else
                    {
                        if (Resources.Load(objectName) == null)
                        {
                            Debug.LogError(string.Format("找不到名字为{0}的物体", objectName));
                            return null;
                        }
                        obj = GameObject.Instantiate(Resources.Load(objectName));

                    }
                }
        }
            else
            {
                if (tempEnumIndexObjectDic.ContainsKey(objType))
                {
                    if (tempEnumIndexObjectDic[objType].Count == 0)
                    {
                        obj = GameObject.Instantiate(Resources.Load(objectName));
                    }
                    if (tempEnumIndexObjectDic[objType].Count > 0)
                    {
                        //拿走该对象
                        obj = tempEnumIndexObjectDic[objType][0].obj;
                        tempEnumIndexObjectDic[objType].RemoveAt(0);
                    }
                }
                else
                {
                    //先从回收站取
                    if (tempDes_Queue.ContainsKey(objType) && tempDes_Queue[objType].Count > 0)
                    {
                        int index = tempDes_Queue[objType].Count;
                        obj = tempDes_Queue[objType][index - 1].obj;
                    }
                    else
                    {
                        obj = GameObject.Instantiate(Resources.Load(objectName));
                    }
                }

           
            }
        gameObj = obj as GameObject;

        //GameObject gameObj = obj as GameObject;
        if (gameObj==null)
            gameObj = GameObject.Instantiate(Resources.Load(objectName)) as GameObject;
            gameObj.SetActive(true);
            return gameObj;
        }


        float gcTime = 1.2f;

        float ticker = 0;

        
        //float clearTempTicker = 0;

        private void LateUpdate()
        {
            ticker += Time.deltaTime;

            //if(haveTempItem)
            //clearTempTicker+=Time.deltaTime;

            if (ticker > gcTime)
            {
                foreach (var t in des_Queue.Keys)
                {
                    CheckPool(t);
                    CheckTempPool(t);
                }

                foreach (var t in des_Queue.Values)
                {
                    if (t.Count > 0)
                    {
                        var temp = t[0];
                        if (temp != null)
                        {
                            DestroyObject(temp);
                        }
                        t.RemoveAt(0);
                        break;
                    }
                }

                foreach (var t in tempDes_Queue.Values)
                {
                    if (t.Count > 0)
                    {
                        var temp = t[0];
                        if(CGameTime.Instance.GetTimeStamp()- temp.storeTime > tempObjStoreTime)
                        {
                            if (temp != null)
                            {
                                DestroyObject(temp.obj);
                            }
                            t.RemoveAt(0);
                            break;
                        }
                    }
                }
                foreach (var t in tempEnumIndexObjectDic.Values)
                {
                    if (t.Count > 0)
                    {
                        var temp = t[0];
                        if (CGameTime.Instance.GetTimeStamp() - temp.storeTime > tempObjStoreTime)
                        {
                            if (temp != null)
                            {
                                DestroyObject(temp.obj);
                            }
                            t.RemoveAt(0);
                            break;
                        }
                    }
                }
                ticker = 0;
            }


     


        }

        /// <summary>
        /// 这里加参数
        /// </summary>
        void CheckTempPool(ObjectPoolSingle type)
        {
            if (tempEnumIndexObjectDic.ContainsKey(type))
            {
                if (tempEnumIndexObjectDic[type].Count > objCount)
                {
                    var obj = tempEnumIndexObjectDic[type][0];
                    tempEnumIndexObjectDic[type].RemoveAt(0);
                    tempDes_Queue[type].Add(obj);
                }
            }
        }
        /// <summary>
        /// 这里加参数
        /// </summary>
        void CheckPool(ObjectPoolSingle type)
        {
            if (EnumIndexObjectDic.ContainsKey(type))
            {
                if (EnumIndexObjectDic[type].Count > objCount)
                {
                    var obj = EnumIndexObjectDic[type][0];
                    EnumIndexObjectDic[type].RemoveAt(0);
                    des_Queue[type].Add(obj);
                }
            }
        }

        /// <summary>
        /// 关闭对象，返还给对象池默认可以存5个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="objCount"></param>
        public void DisappearObjectToPool(ObjectPoolSingle type,GameObject obj,bool isTempObj)
        {
            long storeTime = CGameTime.Instance.GetTimeStamp();
            if (!isTempObj)
            {
                if (!EnumIndexObjectDic.ContainsKey(type))
                {
                    EnumIndexObjectDic.Add(type, new List<GameObject>());
                    des_Queue.Add(type, new List<GameObject>());
                }

                if (obj == null)
                {
                    Debug.LogError("exist null pool obj!");
                }
                //对象返回池子

                obj.SetActive(false);
                obj.transform.SetParent(this.transform, false);
                //防止点快了
                bool canAddToPool = true;
                foreach (GameObject existObj in EnumIndexObjectDic[type])
                {
                    if (existObj.GetInstanceID() == obj.GetInstanceID())
                    {
                        canAddToPool = false;
                        break;
                    }
                }
                if (canAddToPool)
                {
                    EnumIndexObjectDic[type].Add(obj);
                }

            }
            else
            {
                if (!tempEnumIndexObjectDic.ContainsKey(type))
                {
                    tempEnumIndexObjectDic.Add(type, new List<TempItem>());
                    tempDes_Queue.Add(type, new List<TempItem>());
                }

                if (obj == null)
                {
                    Debug.LogError("exist null pool obj!");
                }
                //对象返回池子

                obj.SetActive(false);
                obj.transform.SetParent(this.transform, false);
                //防止点快了
                bool canAddToPool = true;
                foreach (TempItem existTemp in tempEnumIndexObjectDic[type])
                {
                    if (existTemp.obj.GetInstanceID() == obj.GetInstanceID())
                    {
                        canAddToPool = false;
                        break;
                    }
                }
                if (canAddToPool)
                {
                    TempItem tempItem = new TempItem();
                    tempItem.obj = obj;
                    tempItem.storeTime = storeTime;
                    tempEnumIndexObjectDic[type].Add(tempItem);
                }
            }
           

        }



     
    }






    //public interface IPoolObject
    //{
    //    /// <summary>
    //    /// init
    //    /// </summary>
    //    void Spawn();
    //    /// <summary>
    //    /// Finallize
    //    /// </summary>
    //    void UnSpawn();
    //}

