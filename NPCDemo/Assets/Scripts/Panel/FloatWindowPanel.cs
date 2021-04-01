using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 飘窗
/// </summary>
public class FloatWindowPanel : PanelBase
{
    public string str_content;
    public Text txt_content;//内容
    public Image img_bg;//背景
    float showTime = 0.5f;

    public override void Init(object[] args)
    {
        base.Init(args);
        str_content = args[0] as string;
    }

    /// <summary>
    /// 给组件赋值
    /// </summary>
    public override void OnOpenIng()
    {
        base.OnOpenIng();
        txt_content.text = str_content;
        StartCoroutine(ShowTips(this.gameObject,showTime));
    }


    IEnumerator ShowTips(GameObject obj, float showTime)
    {
        yield return new WaitForSeconds(showTime);
        if (obj == null)
            yield break;
        obj.transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, 0.5f, true);
        txt_content.CrossFadeAlpha(0, 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        if (obj == null)
            yield break;
        PanelManager.Instance.ClosePanel(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
