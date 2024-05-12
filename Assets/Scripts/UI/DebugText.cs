using UnityEngine;
using UnityEngine.UI;

public static class DebugText {
    /// <summary>
    /// DebugText Prefab对象
    /// </summary>
    private static GameObject DebugObject;
    /// <summary>
    /// Canvas 对象
    /// </summary>
    private static Transform Canvas;
    static DebugText(){
        DebugObject=Resources.Load<GameObject>("Prefabs/DebugText.prefab");
        Canvas=GameObject.Find("Canvas").transform;
    }
    /// <summary>
    /// 字可能会叠一起
    /// </summary>
    /// <param name="text">输出的字符串</param>
    public static void Log(string text){
        GameObject obj = GameObject.Instantiate(DebugObject,Canvas);
        obj.GetComponent<Text>().text=text;
        GameObject.Destroy(obj,5);
    } 
}