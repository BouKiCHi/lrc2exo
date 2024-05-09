class LrcText {
  public int StartMs { get; }
  public int EndMs { get; private set; }

  public int Length => EndMs - StartMs;
  public string Content { get; }

  public LrcText(string min, string sec, string frm, string content) {
    var mm = Convert.ToInt32(min);
    var ss = Convert.ToInt32(sec);
    var ff = Convert.ToInt32(frm);

    StartMs = ((((mm * 60) + ss) * 100)+ff)*10;
    EndMs = StartMs + 3000;
    Content = content;
  }

  public void SetLengthFromEndTime(string min, string sec, string frm) {
    var mm = Convert.ToInt32(min);
    var ss = Convert.ToInt32(sec);
    var ff = Convert.ToInt32(frm);

    EndMs = ((((mm * 60) + ss) * 100) + ff) * 10;
  }

  public int GetStartFrame(int fps) {
    return (int)(StartMs / 1000f * fps);
  }

  public int GetEndFrame(int fps) {
    var ed = (int)(EndMs / 1000f * fps);
    return 1 < ed ? ed - 1 : ed;
  }
}
