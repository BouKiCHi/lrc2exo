
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

/// <summary>
/// 設定情報
/// </summary>
class Setting {
  public int Width { get; set; } = 1280;
  public int Height { get; set; } = 720;
  public int FramePerSec { get; set; } = 30;
  public int Top { get; set; } = 215;
  public int Top2 { get; set; } = 235;
  public int LineLength { get; set; } = 25;

  public int FontSize { get; set; } = 50;

  public string FontName { get; set; } = "メイリオ";

  internal static Setting Load(string jsonFilename) {
    Setting setting;
    if(!File.Exists(jsonFilename)) {
      setting = new Setting();
      WriteJson(jsonFilename, setting);
    } else {
      var settingJson = File.ReadAllText(jsonFilename);
      setting = JsonSerializer.Deserialize<Setting>(settingJson) ?? new Setting();
      WriteJson(jsonFilename, setting);
    }

    return setting;
  }

  private static void WriteJson(string jsonFilename, Setting setting) {
    var settingJson = JsonSerializer.Serialize(setting, new JsonSerializerOptions {
      Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
      WriteIndented = true,
    });
    File.WriteAllText(jsonFilename, settingJson.ToString());
  }
}
