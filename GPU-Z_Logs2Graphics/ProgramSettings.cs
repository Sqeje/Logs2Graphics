namespace GPU_Z_Logs2Graphics;

public static class ProgramSettings
{
    public static readonly string OutputGraphicsPath = "Graphics";
    public static readonly string InputLogPath = "InputFile";

    public static readonly string AppConsoleTitle = "Hello! This program can Build a Graphics for Log-files from GPU-Z";
    public static readonly string HeadersSelectTitle = "Please, select header you want include to Graphics Builder";

    public static readonly int HeadersForPrintColumns = 2;
    public static readonly int HeadersForPrintColWidth = 35;

    public static string[] SupportedLogFormats => Enum.GetNames(typeof(SupportedLogFormats));
}