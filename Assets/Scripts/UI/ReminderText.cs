using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ReminderText : MonoBehaviour
{
    public static DateTime date=DateTime.Today;
    public Transform calendarText;
    private void Start() {
        UpdateText();
    }

    public async void UpdateText(){

        //!注意！从android 10.0开始，关于Wakeup文件，如果不在data目录，必须用unity调用java，通过android的api才能读取，不能直接读取
#if UNITY_EDITOR
        
#else

        try{
            File.Exists(Main.userData.wakeupPath);
        }catch(Exception e){
            GetComponent<Text>().text=ReminderLib.ToColorText(e.Message,Color.red);
            return;
        }

        if(Main.userData.wakeupSchedule==null){
            GetComponent<Text>().text=ReminderLib.ToColorText("找不到Wakeup文件！Main.userData.wakeupSchedule==null"+Main.userData.wakeupPath,Color.red);
            return;
        }
#endif
        string text="";
        text="今天是："+Main.userData.wakeupSchedule.ToVeryLongDateColor(DateTime.Now)+'\n';

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
        calendarText.GetComponent<CalenderText>().UpdateText();
    }

    public void Yesterday(){
        date=date.AddDays(-1);
        UpdateText();
        calendarText.GetComponent<CalenderText>().UpdateText();
    }
}
