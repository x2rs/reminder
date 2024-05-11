# Assets
unity中的资源文件夹，包含了构建app所需的所有元素：所有的素材，脚本，字体。
## Commands
存放`Powershell`脚本
## Fonts
字体文件`.ttf`，和对应的`TextMesh Pro`文件
## Pictures
图片
## Scenes
unity场景文件
## Scripts
C#脚本

- `Inventory.cs`
- `Main.cs`：
- `ReminderLib.cs`：原本想从python那边照搬功能，但是发现大部分python代码都没用，所以只有一个`ToColorText`是经常用的
- `UserData.cs`：
- `WakeupPath.cs`
- `WakeupSchedule.cs`
- `Weather.cs`：用于访问天气API，默认闵行区

### Scripts/UI

- `ReminderButton.cs`
- `ReminderText.cs`
- `WeatherText.cs`