using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherText : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    public TMP_Dropdown cityDropdown;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        Debug.Log(cityDropdown);
        cityDropdown.onValueChanged.AddListener(delegate { OnCityDropdownValueChanged(cityDropdown.value); });
        //初始化城市选项，我写的有点逆天了
        List<string> options=new List<string>();
        foreach(WeatherCity city in Weather.cities){
            options.Add(city.cityName);
        }
        cityDropdown.AddOptions(options);
    }

    private async void Start(){
        Weather weather=await Weather.GetWeather(Main.userData.cityCode);
        DisplayWeather(weather);
    }

    public async void OnCityDropdownValueChanged(int value)
    {
        Weather weather;
        weather = await Weather.GetWeatherByDropdownValue(value);
        Main.userData.cityCode=Weather.cities[value].cityCode;
        DisplayWeather(weather);
    }

    // 更新 UI 显示天气信息
    private void DisplayWeather(Weather weather)
    {
        switch (weather.status)
        {
            case Weather.WeatherRequestStatus.Success:
                switch (weather.source)
                {
                    case Weather.WeatherSource.LatestRequest:
                    case Weather.WeatherSource.LatestFileData:
                        m_text.text = TextFromWeather(weather);
                        break;
                    case Weather.WeatherSource.OldFileData:
                        m_text.text = "获取最新天气失败，显示旧天气：\n" + TextFromWeather(weather);
                        break;
                }
                break;
            case Weather.WeatherRequestStatus.Failed:
                m_text.text = "获取天气失败，网络异常！";
                break;
            default:
                m_text.text = "获取天气失败，错误代码：" + weather.status;
                break;
        }
    }

    private string TextFromWeather(Weather weather)
    {
        return weather.ToString();
    }
}