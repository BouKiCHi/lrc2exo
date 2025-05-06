
/// <summary>
/// 字幕コンテナインターフェース
/// </summary>
internal interface ISubData {
  /// <summary>
  /// 字幕データを取得
  /// </summary>
  public List<ISubText> Data { get; }

  /// <summary>
  /// 全体の終了時間を取得
  /// </summary>
  int GetTotalEndMs();
}