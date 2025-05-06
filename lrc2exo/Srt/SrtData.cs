using System.Text.RegularExpressions;

/// <summary>
/// SRTデータ作成
/// </summary>
internal class SrtData : ISubData {
  public List<ISubText> Data { get; } = new List<ISubText>();

  public SrtData(string file) {
    var fileText = File.ReadAllText(file);

    var matches = Regex.Matches(fileText,
      "^(\\d+)\\r?\\n(\\d{2}:\\d{2}:\\d{2},\\d{3}) --> (\\d{2}:\\d{2}:\\d{2},\\d{3})\\r?\\n(?s)(.*?)^$", RegexOptions.Multiline);
    foreach(Match m in matches) {
      if(m.Success) {
        var index = m.Groups[1].ToString();
        var startTime = m.Groups[2].ToString();
        var endTime = m.Groups[3].ToString();
        var content = m.Groups[4].ToString();
        content = content.Trim();
        Console.WriteLine($"match:{index} start:{startTime} end:{endTime} text:{content}");
        SrtText? o = null;
        if(!string.IsNullOrEmpty(content)) {
          o = new SrtText(index, startTime, endTime, content);
          Data.Add(o);
        }
      }
    }
  }

  /// <summary>
  /// 全体の終了時間を取得
  /// </summary>
  public int GetTotalEndMs() {
    if(Data.Count == 0)
      return 0;
    return Data.Max(x => x.EndMs);
  }
}