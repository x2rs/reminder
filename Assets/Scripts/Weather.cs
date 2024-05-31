using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public struct WeatherCity{
    public int cityCode;
    public string cityName;
    public WeatherCity(int cityCode,string cityName){
        this.cityCode=cityCode;
        this.cityName=cityName;
    }
}

[Serializable]
public class Weather{
    //城市代码转成城市名称
    public static WeatherCity[] cities={
        new WeatherCity(101020200,"上海市 闵行区"),
        new WeatherCity(101020500,"上海市 徐汇区"),
        new WeatherCity(101021400,"上海市 杨浦区"),
    };

    /// <summary>
    /// 数据来源
    /// </summary>
    public WeatherSource source=WeatherSource.None;
    public WeatherRequestStatus status;
    public string date;
    public string time;
    [Serializable]
    public class CityInfo{
        public string city;
        public string cityKey;
        public string parent;
        public string updateTime;
        public override string ToString(){
            return parent+" "+city;
        }
    }
    public CityInfo cityInfo;
    [Serializable]
    public class WeatherData{
        public string shidu;
        public float pm25;
        public float pm10;
        public string quality;
        public string wendu;
        public string ganmao;
        public WeatherForcast[] forecast;
    }
    public WeatherData data;
    [Serializable]
    public class WeatherForcast{
        /// <summary>
        /// dd格式
        /// </summary>
        public string date;
        /// <summary>
        /// 高温
        /// </summary>
        public string high;
        /// <summary>
        /// 低温
        /// </summary>
        public string low;
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string ymd;
        /// <summary>
        /// 星期几
        /// </summary>
        public string week;
        /// <summary>
        /// 日出时间
        /// </summary>
        public string sunrise;
        /// <summary>
        /// 日落时间
        /// </summary>
        public string sunset;
        /// <summary>
        /// aqi指数
        /// </summary>
        public int aqi;
        /// <summary>
        /// 风向
        /// </summary>
        public string fx;
        /// <summary>
        /// 风力
        /// </summary>
        public string fl;
        /// <summary>
        /// 天气类型，多云；晴天
        /// </summary>
        public string type;
        /// <summary>
        /// 小贴士
        /// </summary>
        public string notice;
        public override string ToString()
        {
            return high+"\n"+low+"\n"+type+"\n"+notice;
        }
    }

    public override string ToString(){
        return cityInfo+"\n更新时间："+date+" "+cityInfo.updateTime+"\n"+data.forecast[0];
    }

    /// <summary>
    /// 直接从服务器获取天气
    /// <br />
    /// 天气信息的获取上限是每分钟300次，超过会禁用一小时，数据每8小时更新一次
    /// </summary>
    /// <returns>天气json字符串</returns>
    private async static Task<string> RequestWeatherString(int cityCode){
        string m_url="http://t.weather.sojson.com/api/weather/city/" + cityCode;
        HttpClient httpClient=new();
        try{
            string jsonContent=await httpClient.GetStringAsync(m_url);
            Debug.Log("Request Success!");
            return jsonContent;
        }catch(Exception e){
            Debug.Log("Request Failed! "+e.Message);
        }
        return "{\"status\":"+(int)WeatherRequestStatus.Failed+"}";//Failed
    }
    public enum WeatherSource{
        None,LatestRequest,LatestFileData,OldFileData
    }
    public enum WeatherRequestStatus{
        Success=200,Failed=-1
    }
    /// <summary>
    /// 获取天气对象，自带缓存
    /// </summary>
    /// <returns>天气对象</returns>
    public async static Task<Weather> GetWeather(int cityCode){
        string weather_path = ReminderLib.dirPath + "/weather_"+cityCode+".json";
        Weather weather;
        if(!File.Exists(weather_path)){
            //无缓存文件
            string jsonContent=await RequestWeatherString(cityCode);
            weather=JsonUtility.FromJson<Weather>(jsonContent);
            switch(weather.status){
                case WeatherRequestStatus.Success:
                File.WriteAllText(weather_path,jsonContent);//Success就保存
                weather.source=WeatherSource.LatestRequest;//设置为最新
                break;
                //返回带有错误代码的weather
            }
        }else{
            //有缓存文件
            string jsonContent=File.ReadAllText(weather_path);
            weather=JsonUtility.FromJson<Weather>(jsonContent);//解析的weather默认为旧
            // 转换日期
            DateTime date=DateTime.ParseExact(weather.date,"yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            TimeSpan updateTime=TimeSpan.ParseExact(weather.cityInfo.updateTime,"h\\:mm",System.Globalization.CultureInfo.CurrentCulture);
            //检测是否大于8小时
            if (DateTime.Now-(date+updateTime)>new TimeSpan(1,0,0)){
                jsonContent=await RequestWeatherString(cityCode);
                Weather newWeather=JsonUtility.FromJson<Weather>(jsonContent);
                if(newWeather.status == WeatherRequestStatus.Success){
                    //保存
                    File.WriteAllText(weather_path,jsonContent);
                    newWeather.source=WeatherSource.LatestRequest;//设置为最新
                    return newWeather;
                }else{
                    newWeather.source=WeatherSource.OldFileData;
                    //失败，return 旧的weather，并传入
                }
            }else{//小于8小时，那么weather是最新的
                weather.source=WeatherSource.LatestFileData;
            }
        }
        return weather;
    }

    public static Task<Weather> GetWeatherByDropdownValue(int value){
        return GetWeather(cities[value].cityCode);
    }
    
}
