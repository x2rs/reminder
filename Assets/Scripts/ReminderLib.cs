using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ReminderLib
{
    public static readonly string appDataDir=Application.persistentDataPath;
    public static string dirPath = appDataDir;

    
    public static string ToColorText(string text, Color color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color>";
    }
    static ReminderLib(){
        if(!Directory.Exists(dirPath)){
            Directory.CreateDirectory(dirPath);
        }
    }

#if UNITY_STANDALONE_WIN
    // !only windows!
    public static void ShowToastNotification(string title,string text){
        string icon_path="D:\\Program Files (x86)\\Dev-Cpp\\Icons\\Danger.ico";
        string script_destination="D:\\Extra\\Documents\\UnityPrograms\\reminder\\Assets\\Commands\\push_notification.ps1";

        Process process = new Process();
        //! 危险，powershell绝对路径
        process.StartInfo.FileName="C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe";
        process.StartInfo.Arguments=$". {script_destination} -title '{title}' -text '{text}' -icon_path '{icon_path}'";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.ErrorDialog = false;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        process.WaitForExit();
        process.Close();
    }
#endif
}