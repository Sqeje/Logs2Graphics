using System.Globalization;
using System.Security.Principal;
using GPU_Z_Logs2Graphics;
using ScottPlot;


if (!Directory.Exists(ProgramSettings.OutputGraphicsPath))
{
    Directory.CreateDirectory(ProgramSettings.OutputGraphicsPath);
}
if (!Directory.Exists(ProgramSettings.InputLogPath))
{
    Directory.CreateDirectory(ProgramSettings.InputLogPath);
}


Console.WriteLine("Hello! This program can Build a Graphics for Log-file from GPU-Z ");
Console.WriteLine($"Please, pull yore logs in {ProgramSettings.InputLogPath} folder");

Console.WriteLine("Files inside folder? (Press any key to continue): ");
Console.ReadKey();
Console.Clear();
// switch (Console.ReadKey().KeyChar)
// {
//     case('y'):
//         Console.WriteLine("В");
//         break;
//     case ('n'):
//         Console.WriteLine("Your graphics wait you in Graphics folder");
//         break;
// }





string inputFilePath = "";

var inputFiles = Directory.GetFiles(ProgramSettings.InputLogPath);
if (inputFiles.Length > 0)
{
    inputFilePath = inputFiles[0];
}

string[] inputFileContent;
using (StreamReader reader = new StreamReader(inputFilePath))
{
    inputFileContent = reader.ReadToEnd().Split("\n");
    
    // Console.WriteLine($"input text:\n{inputFileContent[0].Substring(0, 100)}");
}

GpuzLogInfo fileInfo = new GpuzLogInfo(inputFileContent);

// Пробный вывод заголовков
// Console.WriteLine($"Headers:\n{string.Join(" | ", fileInfo.GetHeaders())} ");

// Пробный вывод временной шкалы
// Console.Clear();
// Console.WriteLine("Time line:\n");
// string[] timeLine = fileInfo.GetSecondsTimeLineData();
// Console.WriteLine(string.Join(" | ", timeLine));
// Console.ReadKey();


string[] headers = fileInfo.GetHeaders();

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


Console.Clear();

string[] selectedHeaders = headersByIds.Where(pair => selectedHeadersId.Contains(pair.Key)).Select(pair => pair.Value).ToArray();
// Console.WriteLine("Selected headers: " + string.Join(", ", headersByIds.Where(pair => selectedHeadersId.Contains(pair.Key)).Select(pair => pair.Value)));


double[] secondsTimeLine = fileInfo.GetSecondsTimeLineData().Select(d => double.Parse(d)).ToArray();
foreach (var item in selectedHeaders)
{
    //ToDo: использовать using для оптимизации памяти
    string[] strDataByHeader = fileInfo.GetTextDataByHeader(item).Where(x => (x != "-")).Select(x => x.Replace(".", ",")).ToArray();
    
    // Console.WriteLine(string.Join(" | ", strDataByHeader));
    
    double[] doubleDataByHeader = strDataByHeader.Select(x => Convert.ToDouble(x)).ToArray();
    
    Plot myPlot = new Plot();
    
    myPlot.Add.Scatter(secondsTimeLine, doubleDataByHeader);
    myPlot.SavePng($"Graphics/graphic_{item}.png", 800, 550);
    
    Console.WriteLine($"image generated: {item}");

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