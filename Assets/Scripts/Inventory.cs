using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Item{
    public string name;
    public Color color;
    public Lesson lesson;
    public Item(string name,Color? color=null,Lesson? lesson=null){
        this.name=name;
        this.color=color??Color.black;
        this.lesson=lesson??new Lesson();
    }
    public string ToText(){
        return ReminderLib.ToColorText(name,color);
    }
}


[Serializable]
public struct Lesson{
    
}

public class Inventory{
    public List<Item> items=new List<Item>();
    public static readonly Dictionary<string, string> courseName2Item=new Dictionary<string, string>{
        {"大学英语","英语书"},
        {"大学物理实验","大物实验讲义,实验报告,"},
        {"大学物理","物理书"},
        {"程序设计思想与方法","电脑"},
        {"概率统计","概统书"},
        {"思想道德与法治",""},
        {"工程学导论","电脑"},
        {"形势与政策",""},
        {"攀岩","水"}
    };
    public static async Task<Inventory> GetInventory(DateTime date,WakeupSchedule wakeupSchedule){
        Inventory inventory=new Inventory();

        //?默认启动天气系统，异步获取天气
        Task<Weather> weatherTask= Weather.GetWeather();


        List<Schedule> schedules=wakeupSchedule.GetSchedules(date);
        HashSet<string> set=new HashSet<string>();//去重集合
        foreach(Schedule schedule in schedules){//依据子串放东西
            foreach (KeyValuePair<string,string> pair in courseName2Item)
            {
                if(schedule.name.Contains(pair.Key)){
                    set.Add(pair.Value);
                }
            }
        }

        foreach(string itemName in set){
            inventory.AddItem(itemName);
        }


        Weather weather=await weatherTask;

        //尝试加载当天的天气！
        foreach (Weather.WeatherForcast forcast in weather.data.forecast)
        {
            string[] _data = forcast.ymd.Split('-');
            DateTime weather_date = new DateTime(int.Parse(_data[0]), int.Parse(_data[1]), int.Parse(_data[2]));
            if (weather_date == date)//找到了当天的！
            {
                string weather_type=forcast.type;
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
    
    public void AddItem(string name)
    {
        items.Add(new Item(name));
    }
}