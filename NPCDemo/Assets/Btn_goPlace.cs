using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_goPlace : MonoBehaviour
{
    public string placeName;
    public Button btn;

    private void Awake()
    {
        this.gameObject.name = placeName;
        GetComponentInChildren<Text>().text = placeName;
        btn.onClick.AddListener(() =>
        {
            NewBehaviourScript.Instance.GoOutSide(placeName);
        });
    }
}
