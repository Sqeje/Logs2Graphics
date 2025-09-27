namespace GPU_Z_Logs2Graphics;

public static class ConsoleCleaner
{
    public static void ClearClean()
    {
        Console.SetCursorPosition(0, 0);
        Console.Clear();
    }

    public static void CleanWithTitle(string title = "")
    {
        ClearClean();
        if (title == "")
        {
            Console.WriteLine(ProgramSettings.AppConsoleTitle);
        }
        else
        {
            Console.WriteLine(title);
        }
    }
}