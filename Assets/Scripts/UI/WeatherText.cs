using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherText:MonoBehaviour
{
    private TextMeshProUGUI m_text;
    private void Awake() {
        m_text=GetComponent<TextMeshProUGUI>();
    }
    private async void OnEnable() {
        Task<Weather> weatherTask=Weather.GetWeather();
        Weather weather=await weatherTask;
        switch(weather.status){
            case Weather.WeatherRequestStatus.Success://成功
            switch(weather.source){
                case Weather.WeatherSource.LatestRequest:
                case Weather.WeatherSource.LatestFileData:
                m_text.text=TextFromWeather(weather);
                break;
                case Weather.WeatherSource.OldFileData:
                m_text.text="获取最新天气失败，显示旧天气：\n"+TextFromWeather(weather);
                break;
            }
            break;
            case Weather.WeatherRequestStatus.Failed:
            m_text.text="获取天气失败，网络异常！";
            break;
            default:
            m_text.text="获取天气失败，错误代码："+weather.status;
            break;
        }
    }

    private string TextFromWeather(Weather weather)
    {
        return weather.ToString();
    }
}
