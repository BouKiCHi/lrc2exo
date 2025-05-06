/// <summary>
/// 字幕データインターフェース
/// </summary>
internal interface ISubText {
  /// <summary>
  /// 開始時間
  /// </summary>
  int StartMs { get; }
  /// <summary>
  /// 終了時間
  /// </summary>
  int EndMs { get; }

  /// <summary>
  /// テキスト内容
  /// </summary>
  string TextContent { get; }
}