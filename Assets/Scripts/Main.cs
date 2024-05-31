using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main附着在场景的第一个对象Main Camera中
/// </summary>
/// 

public class Main : MonoBehaviour{
    public static UserData userData;

    public static GameObject canvas;
    
    // 用于存储找到的Text组件和TextMeshProUGUI组件
    public static List<Text> allGlobalTexts = new List<Text>();
    public static List<TextMeshProUGUI> allGlobalTMPs = new List<TextMeshProUGUI>();

    /// <summary>
    /// Awake时机初始化
    /// </summary>
    private void Awake() {
        canvas=GameObject.Find("Canvas");
        GetTextAndTMPInChildrenIncludingInactive(canvas);
        FontChanger.ChangeFontSize();

        
        if(File.Exists(UserData.DataPath)){
            string _json=File.ReadAllText(UserData.DataPath);
            userData=JsonUtility.FromJson<UserData>(_json);
            userData.LoadWakeupSchedule();
        }
#if UNITY_WINDOWS
        // windows下备选debug.wakeup_schedule
        const string DEBUG_WAKEUP_PATH = "./debug.wakeup_schedule";
        else if(File.Exists(DEBUG_WAKEUP_PATH)){
            Main.userData=UserData.InitializeByWakeup(DEBUG_WAKEUP_PATH);
        }
#endif       
        else{
            userData=new UserData();
        }
    }
    /// <summary>
    /// Start时机初始化
    /// </summary>
    private void Start(){
    }

    /// <summary>
    /// 来自GPT，递归获取所有Text和TMP组件
    /// </summary>
    /// <param name="obj"></param>
    private void GetTextAndTMPInChildrenIncludingInactive(GameObject obj)
    {
        // 检查当前物体上的Text组件
        Text text = obj.GetComponent<Text>();
        if (text != null)
        {
            allGlobalTexts.Add(text);
        }

        // 检查当前物体上的TextMeshProUGUI组件
        TextMeshProUGUI textMeshPro = obj.GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            allGlobalTMPs.Add(textMeshPro);
        }

        // 遍历当前物体的所有子物体
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject child = obj.transform.GetChild(i).gameObject;
            GetTextAndTMPInChildrenIncludingInactive(child);
        }
    }

    /// <summary>
    /// 应用退出时执行
    /// </summary>
    private void OnApplicationQuit() {
        if(userData!=null)
            userData.SaveData();
    }

    
    private void Update() {
        #if UNITY_ANDROID
        if(Input.GetKeyDown(KeyCode.Escape)){
            QuitApplication();
        }
        #endif
    }

    /// <summary>
    /// 调用以退出程序
    /// </summary>
    private void QuitApplication(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
}