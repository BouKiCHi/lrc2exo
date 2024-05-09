using System;
using System.Text.RegularExpressions;

class LrcData {

  public List<LrcText> Data = new List<LrcText>();
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

  internal int GetLength(int fps) {
    var endms = Data.Max(x => x.EndMs);
    return (int)(endms / 1000f * fps);
  }
}
