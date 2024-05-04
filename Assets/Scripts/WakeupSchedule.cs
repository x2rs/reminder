using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class WakeupSchedule{
    public GlobalSettings global;
    public Interval[] intervals;
    public ApparenceSettings apparence;
    public Course[] courses;
    public Arrangement[] arrangements;

    public static readonly TimeSpan[,] timeIntervals=new TimeSpan[15,2];

    static WakeupSchedule(){
        for(int i=1;i<14;i+=2){
            timeIntervals[i,0]=new TimeSpan(i+7,0,0);
            timeIntervals[i,1]=new TimeSpan(i+7,45,0);
            timeIntervals[i+1,0]=new TimeSpan(i+7,55,0);
            timeIntervals[i+1,1]=new TimeSpan(i+8,40,0);
        }
    }

    [Serializable]
    public class GlobalSettings{
        public int courseLen;
        public int id;
        public string name;
        public bool sameBreakLen;
        public int theBreakLen;
    }
    [Serializable]
    public class Interval{
        public string endTime;
        public int node;
        public string startTime;
        public int timeTable;
    }
    [Serializable]
    public class ApparenceSettings{
        public string background;
        public int courseTextColor;
        public int id;
        public int itemAlpha;
        public int itemHeight;
        public int itemTextSize;
        public int maxWeek;
        public int nodes;
        public bool showOtherWeekCourse;
        public bool showSat;
        public bool showSun;
        public bool showTime;
        public string startDate;
        public int strokeColor;
        public bool sundayFirst;
        public string tableName;
        public int textColor;
        public int timeTable;
        public int type;
    }
    [Serializable]
    public class Course{
        public string color;
        public string courseName;
        public float credit;
        public int id;
        public string note;
        public int tableId;
    }
    [Serializable]
    public class Arrangement{
        public int day;
        public string endTime;
        public int endWeek;
        public int id;
        public int level;
        public bool ownTime;
        public string room;
        public int startNode;
        public string startTime;
        public int startWeek;
        public int step;
        public int tableId;
        public string teacher;
        /// <summary>
        /// 0为全部，1为单周，2为双周
        /// </summary>
        public int type;
    }

    public async static Task<WakeupSchedule> FromWakeupFile(string path){
        string _str = await File.ReadAllTextAsync(path);

        //括号栈算法
        int _cnt=0;
        int _start=0;
        string[] _data =new string[5];
        int _ind=0;
        for(int i=0;i<_str.Length;++i){
            if(_str[i]=='[' || _str[i]=='{'){
                ++_cnt;
            }
            else if(_str[i]==']' || _str[i]=='}'){
                --_cnt;
                if(_cnt==0){
                    _data[_ind]=_str.Substring(_start,i-_start+1);
                    ++_ind;
                    _start=i+1;
                }
            }
        }
        string json="{"+
        "\"global\":"+_data[0]+","+
        "\"intervals\":"+_data[1]+","+
        "\"apparence\":"+_data[2]+","+
        "\"courses\":"+_data[3]+","+
        "\"arrangements\":"+_data[4]+
        "}";
        return JsonUtility.FromJson<WakeupSchedule>(json);
    }

    //长日期格式，2024年5月4日 第11周 星期六
    public string ToVeryLongDateColor(DateTime date){
        return ReminderLib.ToColorText(date.ToLongDateString(),Color.blue)
        +" "+$"第{ReminderLib.ToColorText(ToWeekN(date).ToString(),Color.cyan)}周"+" "
        +ReminderLib.ToColorText(date.DayOfWeek.ToString(),Color.magenta);
    }

    //计算是第几周
    public int ToWeekN(DateTime date){
        string[] _date=apparence.startDate.Split('-');
        DateTime startDate=new DateTime(int.Parse(_date[0]),int.Parse(_date[1]),int.Parse(_date[2]));
        return (date-startDate).Days/7+1;
    }

    public List<Schedule> GetSchedules(DateTime date){
        List<Schedule> list=new List<Schedule>();
        string[] _date=apparence.startDate.Split('-');

        //第一周周一的日期
        DateTime startDate=new DateTime(int.Parse(_date[0]),int.Parse(_date[1]),int.Parse(_date[2]));

        TimeSpan span=date-startDate;
        if(span<TimeSpan.Zero){//还没开学
            return list;
        }
        //开学了

        //计算第几周星期几
        int week=span.Days/7+1;
        int day_of_week=(int)date.DayOfWeek;

        Debug.Log(week);
        Debug.Log(day_of_week);

        foreach(Arrangement arrangement in arrangements){
            if(arrangement.day==day_of_week){//星期几是否匹配
                //是否本周
                if(week<arrangement.startWeek||week>arrangement.endWeek) continue;
                switch(arrangement.type){
                    case 1:
                        if(week%2!=1){
                            continue;
                        }
                    break;
                    case 2:
                        if(week%2!=0){
                            continue;
                        }
                    break;
                }
                //加入到list
                Course course=courses[arrangement.id];

                Schedule schedule = new Schedule
                {
                    startTime = timeIntervals[arrangement.startNode, 0],
                    endTime = timeIntervals[arrangement.startNode + arrangement.step, 1],
                    name = course.courseName,
                    place = arrangement.room
                };
                list.Add(schedule);
            }
        }
        return list;
    }
}



public class Schedule{
    public TimeSpan startTime;
    public TimeSpan endTime;
    public string name;
    public string place;
    
}