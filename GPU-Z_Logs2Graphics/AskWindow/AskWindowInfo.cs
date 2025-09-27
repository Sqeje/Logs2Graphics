namespace GPU_Z_Logs2Graphics.AskWindow;

public class AskWindowInfo(string[] content)
{
    private readonly string[] _content = content;



    public SelectableString[] AskAnswer(bool multiple, int printColumns, string windowTitle = "")
    {
        SelectableString[] selectableStrings = AskWindow.GenerateSelectableStrings(_content);

        if (multiple)
        {
            return AskWindow.AskAnswerMultiple(selectableStrings, printColumns, windowTitle);
        }

        return [AskWindow.AskAnswer(selectableStrings, printColumns, windowTitle)];
    }
}