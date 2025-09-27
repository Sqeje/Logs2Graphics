using GPU_Z_Logs2Graphics;
using GPU_Z_Logs2Graphics.AskWindow;
using ScottPlot;


if (!Directory.Exists(ProgramSettings.OutputGraphicsPath))
{
    Directory.CreateDirectory(ProgramSettings.OutputGraphicsPath);
}
if (!Directory.Exists(ProgramSettings.InputLogPath))
{
    Directory.CreateDirectory(ProgramSettings.InputLogPath);
}


// Start Program
ConsoleCleaner.CleanWithTitle();

if (Directory.GetFiles(ProgramSettings.OutputGraphicsPath).Length > 0)
{
    Console.WriteLine("The graphs had already been built previously. Do you want to delete them? (y/n)");
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.Y:
            Directory.Delete(ProgramSettings.OutputGraphicsPath,  true);
            Directory.CreateDirectory(ProgramSettings.OutputGraphicsPath);
            break;
        default:
            break;
    }
}

ConsoleCleaner.CleanWithTitle();

FileInfo[] allLogFilesInfo = Directory.GetFiles(ProgramSettings.InputLogPath).Select(x => new FileInfo(x)).ToArray();

while (allLogFilesInfo.Length == 0)
{
    ConsoleCleaner.CleanWithTitle();
    
    Console.WriteLine($"Folder is Empty. Please, pull your log-files in '{ProgramSettings.InputLogPath}' app folder. (Press any key to repeat check)");
    Console.ReadKey();
    
    allLogFilesInfo = Directory.GetFiles(ProgramSettings.InputLogPath).Select(x => new FileInfo(x)).ToArray();
}

//ToDo: Добавить защиту от пользователя. Нет файлов - попросить положить их и указать полный путь до папки
Console.WriteLine("All files inside folder? (Press any key to continue): ");
Console.ReadKey();


// Выбор файлов из папки
AskWindowInfo logFilesSelectWindow = new AskWindowInfo(allLogFilesInfo.Select(x => x.Name).ToArray());
string[] selectedLogFilesName = logFilesSelectWindow
    .AskAnswer(true, 1, "All your files from folder bellow. Select the files you want build a Graphics:")
    .Select(x => x.Content)
    .ToArray();

FileInfo[] selectedLogFilesInfo = allLogFilesInfo
    .Where(x => selectedLogFilesName.Contains(x.Name))
    .ToArray();


// Выбор типа файла
ConsoleCleaner.ClearClean();
Console.WriteLine("Select the log-files type you want parse:");

AskWindowInfo logFilesTypeSelectWindow = new AskWindowInfo(ProgramSettings.SupportedLogFormats);
string selectedLogFileType = logFilesTypeSelectWindow.AskAnswer(false, 1).Select(x => x.Content).FirstOrDefault() ?? "";

// Console.WriteLine($"Selected Log File Type: {selectedLogFileType}");
// Console.WriteLine($"Enummed Log File Type: {Enum.GetName(SupportedLogFormats.gpuz)}");
//
// Thread.Sleep(20000);


// List<string[]> inputFilesContent = new List<string[]>();
//
// foreach (var fileInfo in selectedLogFilesInfo)
// {
//     using (StreamReader reader = new StreamReader(fileInfo.FullName))
//     {
//         inputFilesContent.Add(reader.ReadToEnd().Split("\n"));
//     }
// }


// Читаем первый файл
// string[] firstSelectedFileContent = [];
// using (StreamReader reader = new StreamReader(selectedLogFilesInfo[0].FullName))
// {
//     firstSelectedFileContent = reader.ReadToEnd().Split("\n");
// }
string[] headers = [];
string[] selectedHeaders = [];
// if (selectedLogFileType == Enum.GetName(SupportedLogFormats.gpuz))
// {
//     GpuzLogInfo fileLogInfo = new GpuzLogInfo(firstSelectedFileContent);
//     headers = fileLogInfo.GetHeaders();
// }

// Выбираем заголовки, по которым хотим построить графики
// AskWindowInfo askHeadersWindows = new AskWindowInfo(headers);
// string[] selectedHeaders = askHeadersWindows
//     .AskAnswer(true, ProgramSettings.HeadersForPrintColumns, ProgramSettings.HeadersSelectTitle)
//     .Select(x => x.Content).ToArray();

/* =====================================================================================================================
Dictionary<int, string> headersByIds = new Dictionary<int, string>();
for (int i = 0; i < headers.Length; i++)
{
    headersByIds.Add(i,  headers[i]);
}

string inputedString = "";
// List<string> selectedHeaders = new List<string>();
List<int> selectedHeadersId = new List<int>();
List<int> nonSelectedHeadersId = headers.Select((h, index) => index).ToList();

while (inputedString != "enter")
{
    Console.Clear();
    
    
    
    Console.WriteLine("Please, choose number, who you want to build a Graphic");
    
    for (int i = 0; i < headers.Length / ProgramSettings.HeadersSelectColumnsPrint; i += ProgramSettings.HeadersSelectColumnsPrint)
    {
        Console.WriteLine("");
        for (int j = 0; j < ProgramSettings.HeadersSelectColumnsPrint; j++)
        {
            int currentListId = i + j;
            string headerName = headersByIds[currentListId];

            if (nonSelectedHeadersId.Contains(currentListId))
            {
                Console.Write($"[{currentListId}] {headerName} ".PadRight(35));
            }
            else
            {
                Console.Write($"[{currentListId}] {headerName.Substring(0, (int)Math.Clamp(8, 0, (int)headerName.Length * 0.25f))}... (SELECTED)".PadRight(35));
                // Console.Write($"[{currentListId}] ... (SELECTED)".PadRight(35));
            }
            
            // KeyValuePair<int, string> selectedPair = headersByIds.First((pair) => nonSelectedHeadersId.Contains(pair.Key));
            // Console.Write($"[{selectedPair.Key}] {selectedPair.Value} ".PadRight(35));
            
            // int currentPairId = nonSelectedHeadersId[i + j];
            
        }
    }
    
    Console.WriteLine("\nEnter number (Or type 'enter' to continue):");
    inputedString = Console.ReadLine() ?? "";
    
    if (int.TryParse(inputedString, out int number))
    {
        if(nonSelectedHeadersId.Contains(number))
        {
            selectedHeadersId.Add(number);
            nonSelectedHeadersId.Remove(number);
        }
        else
        {
            selectedHeadersId.Remove(number);
            nonSelectedHeadersId.Add(number);
        }
        
    }
}
*///====================================================================================================================



//ToDo: использовать using для оптимизации памяти
foreach (var fileInfo in selectedLogFilesInfo)
{
    // Читаем файл
    string[] fileContent = [];
    string fileRelativePath = Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, fileInfo.FullName);
    
    using (StreamReader reader = new StreamReader(fileRelativePath))
    {
        fileContent = reader.ReadToEnd().Split("\n");
    }
    
    
    // Обрабатываем LogInfo от выбранного типа логов
    AbstractLogInfo selectedLogInfo;
    if (selectedLogFileType == Enum.GetName(SupportedLogFormats.gpuz))
    {
        selectedLogInfo = new GpuzLogInfo(fileContent);
        if (headers.Length == 0)
        {
            headers = selectedLogInfo.GetHeaders();
        }
    }
    else
    {
        selectedLogInfo = new GpuzLogInfo(fileContent);
        if (headers.Length == 0)
        {
            headers = selectedLogInfo.GetHeaders();
        }
    }

    // Запрашиваем выбрать заголовки если те не выбраны
    if (selectedHeaders.Length == 0)
    {
        AskWindowInfo askHeadersWindows = new AskWindowInfo(headers);
        selectedHeaders = askHeadersWindows
            .AskAnswer(true, ProgramSettings.HeadersForPrintColumns, ProgramSettings.HeadersSelectTitle)
            .Select(x => x.Content).ToArray();
    }
    
    
    double[] secondsTimeLine = selectedLogInfo.GetSecondsTimeLineData().Select(double.Parse).ToArray();
    
    
    // Строим график по выбранным заголовкам
    foreach (var item in selectedHeaders)
    {
        string[] strDataByHeader = selectedLogInfo.GetTextDataByHeader(item).Select(x => x.Replace(".", ",")).ToArray();
        
        double[] doubleDataByHeader = strDataByHeader.Select(Convert.ToDouble).ToArray();

        // using (Plot tempPlot = new Plot())
        // {
        //     tempPlot.
        // }
        Plot myPlot = new Plot();

        
    
        myPlot.Add.Scatter(secondsTimeLine, doubleDataByHeader);
        myPlot.SavePng($"Graphics/{Path.GetFileNameWithoutExtension(fileInfo.Name)}_graph_{item}.png", 800, 550);
    
        Console.WriteLine($"image generated: Graphics/{Path.GetFileNameWithoutExtension(fileInfo.Name)}_graph_{item}.png");
    }
}


/*
double[] dataX = { 0, 1, 2, 3, 4, 5 };
double[] dataY = { 0, 1, 4, 9, 16, 25 };

Plot myPlot = new Plot();

myPlot.Add.Scatter(dataX, dataY);
myPlot.SavePng("Graphics/Quickstart.png", 400, 300);
*/





// Thread.Sleep(2000);
// Console.Clear();
//
//
// Console.Write("DeleteGraphics? (Y/N): ");
// switch (Console.ReadKey().KeyChar)
// {
//     case('y'):
//         Directory.Delete("Graphics", true);
//         Console.WriteLine("В");
//         break;
//     case ('n'):
//         Console.WriteLine("Your graphics wait you in Graphics folder");
//         break;
//     default:
//         Console.WriteLine("Your graphics wait you in Graphics folder");
//         break;
// }


Console.WriteLine("Press any key to continue...");
Console.ReadKey();