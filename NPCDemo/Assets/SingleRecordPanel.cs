using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRecordPanel : Entity
{
    public UnityEngine.UI.Text txt_name;
    public Transform trans_grid;
    public UnityEngine.UI.Button btn_close;

    public void Init(People people)
    {
        txt_name.text = people.name+"的记录";
        NewBehaviourScript.Instance.ClearAllChildEntity(trans_grid);

        for(int i = 0; i < people.recordList.Count; i++)
        {
            Text_singleRecord txt = NewBehaviourScript.Instance.GenerateEntity(ObjectPoolSingle.Text_singleRecord) as Text_singleRecord;
            txt.transform.SetParent(trans_grid, false);
            txt.Init(people.recordList[i]);
        }


        btn_close.onClick.RemoveAllListeners();
        btn_close.onClick.AddListener(() =>
        {
            ObjectPoolManager.Instance.DisappearObjectToPool(objType, gameObject, isTmpObj);
        });
    }

    
}
