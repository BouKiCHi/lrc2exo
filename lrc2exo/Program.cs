
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

internal class Program {
  private static void Main(string[] args) {
    Console.WriteLine("lrc2exo ver 1.1");

    var jsonFilename = "settings.json";

    var setting = Setting.Load(jsonFilename);

    if(args.Length == 0) {
      Console.WriteLine("usage: lrc2exo <file.lrc|file.srt>");
      return;
    }

    var inputFilename = args[0];
    if(!File.Exists(inputFilename)) {
      Console.WriteLine($"File not found:{inputFilename}");
      return;
    }

    // 対応ファイルを読み込む
    var data = LoadSupportedFile(inputFilename);
    if (data == null) {
      Console.WriteLine("File format is not supported.");
      return;
    }


    const string outputFilename = "output.exo";

    // エンコーディングの登録
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    var exo = new ExoWriter(setting);
    exo.OutputExo(outputFilename, data);

    Console.WriteLine("done.\n");
  }

  private static ISubData? LoadSupportedFile(string inputFilename) {
    if(inputFilename.EndsWith(".srt", StringComparison.OrdinalIgnoreCase)) {
      var srt = new SrtData(inputFilename);
      return srt;
    }

    if(inputFilename.EndsWith(".lrc", StringComparison.OrdinalIgnoreCase)) {
      var lrc = new LrcData(inputFilename);
      return lrc;
    }

    return null;
  }
}