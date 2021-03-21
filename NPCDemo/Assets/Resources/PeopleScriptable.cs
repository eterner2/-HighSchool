using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleScriptable : ScriptableObject
{
    public List<PeopleData> peopleDataList;
}

[System.Serializable]
public class PeopleData
{
    public string name;
    public Gender gender;
}