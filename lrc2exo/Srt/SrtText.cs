
/// <summary>
/// SRTデータ作成
/// </summary>
internal class SrtText : ISubText {
  /// <summary>
  /// 開始時間
  /// </summary>
  public int StartMs { get; private set; }
  /// <summary>
  /// 終了時間
  /// </summary>
  public int EndMs { get; private set; }
  /// <summary>
  /// 長さ
  /// </summary>
  public string TextContent { get; private set; }
  public SrtText(string index, string startTime, string endTime, string content) {
    StartMs = ConvertToMs(startTime);
    EndMs = ConvertToMs(endTime);
    TextContent = content;
  }

  /// <summary>
  /// 時間をミリ秒に変換
  /// </summary>
  private int ConvertToMs(string time) {
    var parts = time.Split(new[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
    return (int.Parse(parts[0]) * 3600000) + (int.Parse(parts[1]) * 60000) + (int.Parse(parts[2]) * 1000) + int.Parse(parts[3]);
  }
}