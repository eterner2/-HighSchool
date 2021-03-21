using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScriptable : ScriptableObject
{
    public List<SingleAction> singleActionList;
    /// <summary>
    /// 行为名找地名
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public SingleAction FindActionByName(string name)
    {
        for(int i=0;i< singleActionList.Count; i++)
        {
            if (name == singleActionList[i].name)
                return singleActionList[i];
        }
        return null;
    }
    //地名找行为名
    public SingleAction FindActionByOutSideName(string name)
    {
        for (int i = 0; i < singleActionList.Count; i++)
        {
            if (name == singleActionList[i].placeName)
                return singleActionList[i];
        }
        return null;
    }
}

/// <summary>
/// 行为
/// </summary>
[System.Serializable]
public class SingleAction
{
    public string name;
    public ActionType actionType;
    public string placeName;
    public Sprite sprt;
}
