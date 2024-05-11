using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour{
    public static UserData userData;
    private void OnApplicationQuit() {
        if(userData!=null)
            userData.SaveData();
    }
    private void Update() {
        #if UNITY_ANDROID
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home)){
            QuitApplication();
        }
        #endif
    }
    private void QuitApplication(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
}