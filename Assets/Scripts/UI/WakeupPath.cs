using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakeupPath : MonoBehaviour
{
    public void LoadWakeupPath(){
        Main.userData.wakeupPath=GetComponent<InputField>().text;
        Main.userData.LoadWakeupSchedule();
    }
}
