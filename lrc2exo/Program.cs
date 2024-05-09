
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

internal class Program {
  private static void Main(string[] args) {
    Console.WriteLine("lrc2exo ver 1.0");

    var jsonFilename = "settings.json";

    Setting setting;
    if (!File.Exists(jsonFilename)) {
      setting = new Setting();
      var settingJson = JsonSerializer.Serialize(setting, new JsonSerializerOptions { 
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
      });
      File.WriteAllText(jsonFilename, settingJson.ToString());
    } else {
      var settingJson = File.ReadAllText(jsonFilename);
      setting = JsonSerializer.Deserialize<Setting>(settingJson) ?? new Setting();
    }


    if(args.Length == 0) {
      Console.WriteLine("usage: lrc2exo <file.lrc>");
      return;
    }

    var lrcFilename = args[0];
    if (!File.Exists(lrcFilename)) {
      Console.WriteLine($"File not found:{lrcFilename}");
      return;
    }

    var lrc = new LrcData(args[0]);

    const string filename = "output.exo";

    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    var exo = new ExoWriter(setting);
    exo.OutputExo(filename, lrc);

    Console.WriteLine("done.\n");
  }
}