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
        cityDropdown = GetComponent<TMP_Dropdown>();
        cityDropdown.onValueChanged.AddListener(delegate { OnCityDropdownValueChanged(cityDropdown.value); });
    }

    public async void OnCityDropdownValueChanged(int value)
    {
        Weather weather;

        switch (value)
        {
            case 0:
                weather = await Main.userData.GetMinhangWeather();
                break;
            case 1:
                weather = await Main.userData.GetYangpuWeather();
                break;
            case 2:
                weather = await Main.userData.GetXuhuiWeather();
                break;
            default:
                weather = await Main.userData.GetMinhangWeather();
                break;
        }

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