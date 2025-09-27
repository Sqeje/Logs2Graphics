namespace GPU_Z_Logs2Graphics;

public abstract class AbstractLogInfo : IDisposable
{
    //public string TimeLineHeaderName { get; private set; }


    public abstract string[] GetHeaders();
    
    public abstract string[] GetRawTimeLineData();
    
    public abstract string[] GetSecondsTimeLineData();
    
    
    
    public abstract string[] GetTextDataByHeader(string header);

    
    public virtual void Dispose()
    {
        // TODO release managed resources here
    }
}