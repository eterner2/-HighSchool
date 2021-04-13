using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeopleView : SingleViewBase
{
    public Sprite sprt_female;
    public Sprite sprt_male;

    public Image img;
    public Text txt_name;
    public Button btn;
    public People people;
    public override void Init(params object[] args)
    {
         people = args[0] as People;
        if (people.gender==Gender.Female)
            img.sprite = sprt_female;
        else
            img.sprite = sprt_male;
        txt_name.text = people.name;

        //btn.onClick.RemoveAllListeners();
        //btn.onClick.AddListener(() =>
        //{
        //    SingleRecordPanel panel = NewBehaviourScript.Instance.GenerateEntity(ObjectPoolSingle.SingleRecordPanel) as SingleRecordPanel;
        //    panel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        //    panel.Init(people);
        //});
    }

    public override void OnOpenIng()
    {
        base.OnOpenIng();
    

    }

}
