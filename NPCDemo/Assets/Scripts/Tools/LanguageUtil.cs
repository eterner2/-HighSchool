using System;
using System.Reflection;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// 语言管理类
/// </summary>
public static class LanguageUtil
{
    private static string curLanguage = "Chinese";

    public static string CurLanguage
    {
        get { return curLanguage; }
        set
        {
            curLanguage = value;
            if (OnLanguageChange != null)
                OnLanguageChange();
        }
    }
    public static event Action OnLanguageChange;


    /// <summary>
    /// 文本值完全是表格里的文本时使用这个
    /// </summary>
    /// <param name="targetText"></param>
    /// <param name="textId">表格id</param>
    public static void SetText(this Text targetText, string textId, bool withFitter = false)
    {

        targetText.text = textId;
        targetText.text = targetText.text.Replace("\\n", "\n");

    }

}



