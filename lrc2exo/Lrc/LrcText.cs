
/// <summary>
/// 歌詞データ
/// </summary>
class LrcText : ISubText {

  /// <summary>
  /// 開始時間
  /// </summary>
  public int StartMs { get; }

  /// <summary>
  /// 終了時間
  /// </summary>
  public int EndMs { get; private set; }

  /// <summary>
  /// 長さ
  /// </summary>
  public int Length => EndMs - StartMs;
  /// <summary>
  /// 内容
  /// </summary>
  public string TextContent { get; }

  /// <summary>
  /// コンストラクタ
  /// </summary>
  public LrcText(string min, string sec, string frm, string content) {
    var mm = Convert.ToInt32(min);
    var ss = Convert.ToInt32(sec);
    var ff = Convert.ToInt32(frm);

    StartMs = ((((mm * 60) + ss) * 100) + ff) * 10;
    EndMs = StartMs + 3000;
    TextContent = content;
  }

  /// <summary>
  /// 長さを設定
  /// </summary>
  public void SetLengthFromEndTime(string min, string sec, string frm) {
    var mm = Convert.ToInt32(min);
    var ss = Convert.ToInt32(sec);
    var ff = Convert.ToInt32(frm);

    EndMs = ((((mm * 60) + ss) * 100) + ff) * 10;
  }
}
