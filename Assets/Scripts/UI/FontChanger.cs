using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于变换字体
/// </summary>
public class FontChanger : MonoBehaviour
{
    private static float m_fontSize=MIDDLE;
    public static float fontSize{
        get{
            return m_fontSize;
        }
        set{
            m_fontSize=value;
            ChangeFontSize();
        }
    }
    private const float BIG=100;
    private const float MIDDLE=67;
    private const float SMALL=33;
    private TMP_Dropdown dropdown;
    private void Start() {
        dropdown=GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(dropdown.value); });
        ChangeFontSize();
    }
    /// <summary>
    /// 静态改字体
    /// </summary>
    public static void ChangeFontSize(){
        Debug.Log("Change to "+fontSize);
        foreach(Text text in Main.allGlobalTexts){
            text.fontSize=(int)fontSize;
        }
        foreach(TextMeshProUGUI text in Main.allGlobalTMPs){
            text.fontSize=fontSize;
        }
    }
    /// <summary>
    /// 当值改变时改字体
    /// </summary>
    public void OnDropdownValueChanged(int value){
        switch(value){
            case 0:
            fontSize=BIG;
            break;
            case 1:
            fontSize=MIDDLE;
            break;
            case 2:
            fontSize=SMALL;
            break;
        }
    }
}
