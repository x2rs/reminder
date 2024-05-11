using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ReminderText : MonoBehaviour
{
    public DateTime date=DateTime.Today;
    private void Start() {
        
        if(File.Exists(UserData.DataPath)){
            string _json=File.ReadAllText(UserData.DataPath);
            Main.userData=JsonUtility.FromJson<UserData>(_json);
            Main.userData.LoadWakeupSchedule();
        }else{
            Main.userData=new UserData();
        }
        UpdateText();
    }

    public async void UpdateText(){
        #if UNITY_ANDROID
        #endif
        #if UNITY_STANDALONE_WIN
        #endif

        GetComponent<Text>().text=File.Exists(Main.userData.wakeupPath)+" "+Main.userData.wakeupPath;
        
        return;

        if(Main.userData.wakeupSchedule==null){
            GetComponent<Text>().text=ReminderLib.ToColorText("找不到Wakeup文件！"+UserData.DataPath,Color.red);
            return;
        }
        string text="";
        try{
            text="今天是："+Main.userData.wakeupSchedule.ToVeryLongDateColor(DateTime.Now)+'\n';
        }catch(Exception e){
            GetComponent<Text>().text=ReminderLib.ToColorText("解析Wakeup文件失败！"+UserData.DataPath,Color.red);
            return;
        }

        Inventory inventory=await Main.userData.GetInventory(date);
        text+="在"+Main.userData.wakeupSchedule.ToVeryLongDateColor(date)+"你需要带"+ReminderLib.ToColorText(inventory.items.Count.ToString(),Color.red)+"件物品：\n";
        int index=1;
        foreach(Item item in inventory.items){
            text+=index.ToString()+"."+item.ToText()+'\n';

            ++index;
        }
        GetComponent<Text>().text=text;
    }

    public void Tomorrow(){
        date=date.AddDays(1);
        UpdateText();
    }

    public void Yesterday(){
        date=date.AddDays(-1);
        UpdateText();
    }
}
