using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMPanel : MonoBehaviour
{
    public InputField input_addExpNum;
    public Button btn_addExpTest;
    // Start is called before the first frame update
    void Start()
    {
        btn_addExpTest.onClick.AddListener(AddExpTest);
    }

    void AddExpTest()
    {
        int awardId = (int)PropertyIdType.Study;
        int awardNum= input_addExpNum.text.ToInt32();

        //升级前
        LevelInfo levelInfo = null;
        if (awardId == (int)PropertyIdType.Study)
        {
            levelInfo = RoleManager.Instance.GetPeopleLevelInfo(awardNum);
        }


        RoleManager.Instance.AddProperty((PropertyIdType)awardId, awardNum);
        List<AwardData> awardList = new List<AwardData>();
        awardList.Add(new AwardData(AwardType.Property, awardId, awardNum));
        //把需要显示的发给ui
        PanelManager.Instance.OpenPanel<GetAwardPanel>(PanelManager.Instance.trans_layer2, awardList, null, levelInfo);
    }
}
