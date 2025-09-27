namespace GPU_Z_Logs2Graphics.AskWindow;

public static class AskWindow
{
    public static readonly string StopKey = "enter";




    public static SelectableString[] AskAnswerMultiple(SelectableString[] samples, int printColumns, string selectTitle = "")
    {
        // List<SelectableString> answer;
        string lastAnswer = "";
        
        while (lastAnswer != StopKey)
        {
            ConsoleCleaner.CleanWithTitle(selectTitle);
            
            // Печать вариантов
            string[] printableSamples = GenerateColumnsStrings(samples, printColumns, ProgramSettings.HeadersForPrintColWidth);
            foreach (var str in printableSamples)
            {
                Console.WriteLine(str);
            }
            
            Console.WriteLine($"Enter the number of point you select ({StopKey} for confirm): ");
            lastAnswer = Console.ReadLine() ?? "";

            if (lastAnswer != StopKey)
            {
                samples.FirstOrDefault(x => x.Id == lastAnswer)?.ToggleSelectableState();
                if (samples.Select(x => x.Selected).All(x => x))
                {
                    Console.WriteLine("Выбраны все пункты");
                    return samples.Where(x => x.Selected).ToArray();
                } 
            }
        }

        return samples.Where(x => x.Selected).ToArray();
    }

    public static SelectableString AskAnswer(SelectableString[] samples, int printColumns, string selectTitle = "")
    {
        ConsoleCleaner.CleanWithTitle(selectTitle);
        
        // Печать вариантов
        string[] printableSamples = GenerateColumnsStrings(samples, printColumns, ProgramSettings.HeadersForPrintColWidth);
        foreach (var str in printableSamples)
        {
            Console.WriteLine(str);
        }
            
        Console.WriteLine("Enter the number of point you select: ");
        string answer = Console.ReadLine() ?? "";

        
        SelectableString? selectedStr = samples.FirstOrDefault(x => x.Id == answer);
        
        if (selectedStr != null)
        {
            selectedStr.SetSelectableState(true);
            return selectedStr;
        }
        
        return AskAnswer(samples, printColumns, selectTitle);
    }


    public static SelectableString[] GenerateSelectableStrings(string[] sample)
    {
        List<SelectableString> result = new List<SelectableString>();

        for (int i = 0; i < sample.Length; i++)
        {
            result.Add( new SelectableString(i.ToString(), sample[i]));
        }
        
        return result.ToArray();
    }


    
    /// ToDo: Сделать более нативно. Во первых, надо чтобы он работал с классом данных. Его ещё в планах сделать
    /// ToDo: Класс, который будет описывать каждый вариант выбора, иметь свой номер и т.д.
    public static string[] GenerateColumnsStrings(SelectableString[] sample, int columns = 2, int columnWidth = 35)
    {
        List<string> result = new List<string>();

        int counter = 0;
        string lastString = "";
        
        foreach (var item in sample)
        {
            lastString += item.ToString().PadRight(columnWidth);
            counter += 1;
            if (counter == columns)
            {
                counter = 0;
                result.Add(lastString);
                lastString = "";
            }
        }

        return result.ToArray();
    }
}