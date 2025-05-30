﻿using System;
using System.Text.RegularExpressions;

/// <summary>
/// LRCデータ作成
/// </summary>
class LrcData : ISubData {

  public List<ISubText> Data { get; } = new List<ISubText>();
  public LrcData(string file) {
    var lines = File.ReadAllLines(file);
    LrcText? lastObject = null;

    foreach(var line in lines) {
      var m = Regex.Match(line, "^\\[(\\d+):(\\d+).(\\d+)\\](.*)$");
      if(m.Success) {
        var min = m.Groups[1].ToString();
        var sec = m.Groups[2].ToString();
        var frm = m.Groups[3].ToString();
        var content = m.Groups[4].ToString();
        Console.WriteLine($"match:{min}:{sec}.{frm} text:{content}");

        LrcText? o = null;
        if (!string.IsNullOrEmpty(content)) {
          o = new LrcText(min, sec, frm, content);
          Data.Add(o);
        }
        if (lastObject != null) {
          lastObject.SetLengthFromEndTime(min,sec,frm);
        }
        lastObject = o;
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
