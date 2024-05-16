using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReminderButton : MonoBehaviour
{
    
    private void Start() {
        Button button=GetComponent<Button>();
        button.onClick.AddListener(delegate{
            string panelName=gameObject.name+"Panel";
            Transform activePanelTrans=GameObject.Find("Canvas/Panels").GetComponentsInChildren<Transform>()[1];// this gets the only active panel obj.
            GameObject panelObj=GameObject.Find("Canvas/Panels/"+panelName);
            if(activePanelTrans==panelObj.transform){
                return;
            }
            activePanelTrans.gameObject.SetActive(false);
            panelObj.SetActive(true);
        });
    }
}
