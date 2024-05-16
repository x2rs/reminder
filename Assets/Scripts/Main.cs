using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main附着在场景的第一个对象Main Camera中
/// </summary>
public class Main : MonoBehaviour{
    public static UserData userData;
    /// <summary>
    /// 应用开始时执行
    /// </summary>
    private void Start() {
        try{
            string text=File.ReadAllText("/storage/emulated/0/10.wakeup_schedule");
            DebugText.Log(text);
        }catch(Exception e){
            DebugText.Log(e.Message);
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