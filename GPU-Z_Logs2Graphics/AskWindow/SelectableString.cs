namespace GPU_Z_Logs2Graphics.AskWindow;



/// <summary>
/// Обычно используется как временный тип данных для составления списка выбора.
/// </summary>
/// <param name="id"></param>
/// <param name="content"></param>
public class SelectableString(string id, string content)
{
    private readonly string _id = id;
    private readonly string _content = content;
    private bool _selected;

    
    public string Id => _id;
    public string Content => _content;
    public bool Selected => _selected;
    
    
    public void SetSelectableState(bool state)
    {
        _selected = state;
    }

    public void ToggleSelectableState()
    {
        _selected = !_selected;
    }
    
    
    /// <summary>
    /// return string type of: [id] content
    /// </summary>
    public override string ToString()
    {
        if (!_selected)
        {
            return $"[{_id}] {_content}";
        }
        
        return $"[{_id}] {_content.Substring(0, (int)Math.Clamp(8, 0, (int)_content.Length * 0.25f))}... (SELECTED)";
    }
}