using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReminderText : MonoBehaviour
{
    public DateTime date=DateTime.Today;
    private async void Start() {
        Main.userData=await UserData.Initialize("D:\\Extra\\Desktop\\example.wakeup_schedule");
        UpdateText();
    }

    private async void UpdateText(){
        string text="今天是："+Main.userData.wakeupSchedule.ToVeryLongDateColor(DateTime.Now)+'\n';
        

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
