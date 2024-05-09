
using System.Text;

class ExoWriter {
  private readonly Setting setting;

  public ExoWriter(Setting setting) {
    this.setting = setting;
  }

  public void OutputExo(string filename, LrcData lrc) {
    var sjisEnc = Encoding.GetEncoding("shift_jis");

    var sw = new StreamWriter(filename, false, sjisEnc);
    var y = setting.Top;
    var fps = setting.FramePerSec;
    var length = lrc.GetLength(fps);

    WriteHeader(sw, length);

    var itemNo = 0;
    foreach(var e in lrc.Data) {
      var start = e.GetStartFrame(fps);
      var end = e.GetEndFrame(fps);
      var content = e.Content;
      Console.WriteLine($"start:{start} end:{end} text:{content}");
      WriteText(sw, itemNo++, y, start, end, ToExoText(content));
    }

    sw.Close();
  }

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


  private void WriteText(StreamWriter sw, int itemNo, int y, int start, int end,string content) {
    var font = setting.FontName;
    var size = setting.FontSize;
    var output = $@"[{itemNo}]
start={start + 1}
end={end + 1}
layer=1
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
