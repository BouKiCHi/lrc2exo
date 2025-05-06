using System.Text;

/// <summary>
/// EXOファイル出力
/// </summary>
class ExoWriter {
  private readonly Setting setting;

  public ExoWriter(Setting setting) {
    this.setting = setting;
  }


  /// <summary>
  /// EXOファイル出力
  /// </summary>
  public void OutputExo(string filename, ISubData lrc) {
    var sjisEnc = Encoding.GetEncoding("shift_jis");

    var sw = new StreamWriter(filename, false, sjisEnc);
    var y = setting.Top;
    var fps = setting.FramePerSec;
    var totalEndMs = lrc.GetTotalEndMs();
    var length = CalcLength(totalEndMs, fps);

    WriteHeader(sw, length);

    var itemNo = 0;
    foreach(var e in lrc.Data) {
      var startFrame= CalcStartFrame(e.StartMs, fps);
      var endFrame = CalcEndFrame(e.EndMs, fps);
      var content = e.TextContent;
      Console.WriteLine($"startFrame:{startFrame} endFrame:{endFrame} text:{content}");

      // 1行に収まらない、かつ改行がない場合は、改行を入れる
      if(setting.LineLength < content.Length && !content.Contains("\n")) {

        var chunks = content.Chunk(setting.LineLength)
              .Select(chunk => new string(chunk));

        content = string.Join("\n", chunks);
      }
      WriteTextObject(sw, 1, itemNo++, y, startFrame, endFrame, ToExoText(content));
    }

    sw.Close();
  }

  /// <summary>
  /// 長さ計算
  /// </summary>
  private int CalcLength(int endMs, int fps) {
    return (int)(endMs / 1000f * fps);
  }

  /// <summary>
  /// 開始フレームを計算
  /// </summary>
  private int CalcStartFrame(int startMs, int fps) {
    return (int)(startMs / 1000f * fps);
  }

  /// <summary>
  /// 終了フレームを計算
  /// </summary>
  private int CalcEndFrame(int endMs, int fps) {
    var ed = (int)(endMs / 1000f * fps);
    // 後の位置を考慮して、1フレーム前にする
    return 1 < ed ? ed - 1 : ed;
  }

  /// <summary>
  /// ヘッダ部分の作成
  /// </summary>
  private void WriteHeader(StreamWriter sw, int length) {
    var width = setting.Width;
    var height = setting.Height;
    var rate = setting.FramePerSec;

    var output = $@"[exedit]
width={width}
height={height}
rate={rate}
scale=1
length={length}
audio_rate=44100
audio_ch=2
";
    sw.Write(output);
  }


  /// <summary>
  /// テキストオブジェクトの作成
  /// </summary>
  private void WriteTextObject(StreamWriter sw, int layer, int itemNo, int y, int start, int end,string content) {
    var font = setting.FontName;
    var size = setting.FontSize;
    var output = $@"[{itemNo}]
start={start + 1}
end={end + 1}
layer={layer}
overlay=1
camera=0
[{itemNo}.0]
_name=テキスト
サイズ={size}
表示速度=0.0
文字毎に個別オブジェクト=0
移動座標上に表示する=0
自動スクロール=0
B=0
I=0
type=3
autoadjust=0
soft=1
monospace=0
align=1
spacing_x=0
spacing_y=0
precision=1
color=ffffff
color2=000000
font={font}
text={content}
[{itemNo}.1]
_name=標準描画
X=0.0
Y={y}
Z=0.0
拡大率=100.00
透明度=0.0
回転=0.00
blend=0
";
    sw.Write(output);
  }

  /// <summary>
  /// exoテキストへの変換
  /// ref: https://qiita.com/R_TES_/items/57ad42b7b66882c39797
  /// </summary>
  /// <returns>exoテキスト(hex文字列)</returns>
  private static string ToExoText(string text) {
    var result = BitConverter.ToString(Encoding.Unicode.GetBytes(text)).Replace("-", "");
    result += new string('0', 4096 - result.Length);
    return result;
  }

  /// <summary>
  /// exoテキストからの変換
  /// </summary>
  /// <returns>テキスト</returns>
  private static string ExoTextToString(string es) {
    List<byte> output = new List<byte>();
    for(var i = 0; i < es.Length; i += 2) {
      var s = es.Substring(i, 2);
      var b = Convert.ToByte(s, 16);
      output.Add(b);
    }

    // null terminaltion
    var result = Encoding.Unicode.GetString(output.ToArray());
    var elements = result.Split('\0');
    return elements[0];
  }
}
