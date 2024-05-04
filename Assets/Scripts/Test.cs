using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour{
    private async void Start() {
        // ReminderLib.ShowToastNotification("title111","1145141919810");
        WakeupSchedule sch=await WakeupSchedule.FromWakeupFile("D:\\Extra\\Desktop\\example.wakeup_schedule");
        Inventory inventory=await Inventory.GetInventory(new DateTime(2024,5,6),sch);
        foreach(Item item in inventory.items){
            Debug.Log(item.name);
        }
    }
}