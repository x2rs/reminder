using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarText : MonoBehaviour
{

    private void Start() {
        UpdateText();
    }
    public void UpdateText(){
        GetComponent<Text>().text=Main.userData.GetScheduleString(ReminderText.date);
    }
}
