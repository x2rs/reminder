using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UserData{
    public WakeupSchedule wakeupSchedule;

    public string[] globalItem={"笔记本","笔袋","水"};

    public static string DataPath{
        get{
            return $"{ReminderLib.dirPath}\\user_data.json";
        }
    }
    public static async Task<UserData> LoadData(){
        if(!File.Exists(DataPath)){
            return null;
        }
        string json=await File.ReadAllTextAsync(DataPath);
        UserData data=JsonUtility.FromJson<UserData>(json);
        return data;
    }
    public async void SaveData(){
        string json=JsonUtility.ToJson(this);
        await File.WriteAllTextAsync(DataPath,json);
    }
    public static async Task<UserData> Initialize(string wakeup_schedule_path){
        UserData data = new UserData
        {
            wakeupSchedule = await WakeupSchedule.FromWakeupFile(wakeup_schedule_path)
        };
        return data;
    }

    public static Dictionary<string, string> courseName2Item = new Dictionary<string, string>{
        //最前面的先判定
        {"数学分析","数分书"},
        {"大学英语","英语书"},
        {"大学物理实验","大物实验讲义,预习报告"},//多个物品用逗号隔开
        {"大学物理","物理书"},
        {"程序设计思想与方法","电脑"},
        {"概率统计","概统书"},
        {"思想道德与法治",""},
        {"工程学导论","电脑"},
        {"形势与政策",""},
        {"攀岩","水"}
    };
    public async Task<Inventory> GetInventory(DateTime date)
    {
        Inventory inventory = new Inventory();

        //?默认启动天气系统，异步获取天气
        Task<Weather> weatherTask = Weather.GetWeather();


        List<Schedule> schedules = wakeupSchedule.GetSchedules(date);
        HashSet<string> set = new HashSet<string>(globalItem);//去重集合，先包含globalItem
        foreach (Schedule schedule in schedules)
        {//依据子串放东西
            foreach (KeyValuePair<string, string> pair in courseName2Item)
            {
                if(pair.Value=="") continue;
                if (schedule.name.Contains(pair.Key))
                {
                    string[] _data=pair.Value.Split(',');
                    foreach(string _datum in _data){
                        set.Add(_datum);
                    }
                    break;
                }
            }
        }

        foreach (string itemName in set)
        {
            inventory.AddItem(itemName);
        }


        Weather weather = await weatherTask;

        //尝试加载当天的天气！
        //!todo uv 遮阳帽
        foreach (Weather.WeatherForcast forcast in weather.data.forecast)
        {
            string[] _data = forcast.ymd.Split('-');
            DateTime weather_date = new DateTime(int.Parse(_data[0]), int.Parse(_data[1]), int.Parse(_data[2]));
            if (weather_date == date)//找到了当天的！
            {
                string weather_type = forcast.type;
                //下雨带伞，只要天气中有“雨”字，蚌埠住了
                if (weather_type.Contains("雨"))
                {
                    inventory.AddItem("雨伞");
                }
                Debug.Log(weather_type);
            }
        }


        return inventory;
    }

    public async Task LoadWakeupScheduleFromPathAsync(string path){
        wakeupSchedule=await WakeupSchedule.FromWakeupFile(path);
    }

}