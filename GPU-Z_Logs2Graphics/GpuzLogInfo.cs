namespace GPU_Z_Logs2Graphics;

public class GpuzLogInfo : AbstractLogInfo
{
    private readonly string[] _logTextLines;

    private string[] _headers = new string[0];
    private string[] _rawTimeLine = new string[0];
    private string[] _secondsTimeLine = new string[0];
    // private string[] _dataByHeaders = new string[0];
    
    
    
    public GpuzLogInfo(string[] textLines)
    {
        _logTextLines = textLines;
    }
    
    
    public override string[] GetHeaders()
    {
        if (_headers.Length == 0)
        {
            // Console.WriteLine(logTextLines[0]);
            string[] newHeaders = _logTextLines[0].Split(",");
        
            _headers = newHeaders.Select(x => x.Trim()).ToArray();
        }

        return _headers;
    }

    public override string[] GetRawTimeLineData()
    {
        if (_rawTimeLine.Length != 0)
        {
            return _rawTimeLine;
        }
        
        List<string> result = new List<string>();
        
        for (int i = 1; i < _logTextLines.Length; i++)
        {
            string timeCode = _logTextLines[i].Split(",")[0].Trim();
            
            if (timeCode.Split(" ").Length == 1)
            {
                Console.WriteLine("Maybe your Log-File contains more when 1 log write. This is happened when you start log to one file more when one time. We Auto fix it. Calm ;)");
                break;
            }
            
            timeCode = timeCode.Split(" ")[1];
            
            result.Add(timeCode);
        }
        
        _rawTimeLine = result.ToArray();
        return _rawTimeLine;
    }

    public override string[] GetSecondsTimeLineData()
    {
        if (_rawTimeLine.Length == 0)
        {
            GetRawTimeLineData();
        }
        if (_secondsTimeLine.Length != 0)
        {
            return _secondsTimeLine;
        }

        List<string> resultInSeconds = new List<string>() {"0"};
        int zeroSeconds = (int)TimeSpan.Parse(_rawTimeLine[0]).TotalSeconds;
        
        for (int i = 1; i < _rawTimeLine.Length; i++)
        {
            int currentInSec = (int)TimeSpan.Parse(_rawTimeLine[i]).TotalSeconds - zeroSeconds;
            resultInSeconds.Add(currentInSec.ToString());
        }
        
        _secondsTimeLine = resultInSeconds.ToArray();
        return _secondsTimeLine;
    }

    public override string[] GetTextDataByHeader(string header)
    {
        if (header.Length == 0)
        {
            GetHeaders();
        }

        List<string> result = new List<string>();
        int columnId = Array.FindIndex(_headers, (x) => x == header);
        
        for (int i = 1; i < _logTextLines.Length; i++)
        {
            string targetLineData = _logTextLines[i].Split(",")[columnId].Trim();
            result.Add(targetLineData);
        }

        return result.ToArray();
    }
}