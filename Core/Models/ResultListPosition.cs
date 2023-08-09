namespace Core.Models;

public enum ResultStatus
{
    NotInList = 0,
    InList = 1
}
public class ResultListPosition
{
    public ResultListPosition(int position, ResultStatus status)
    {
        Position = position;
        ResultStatus = status;
    }
    public int Position { get; set; }
    public ResultStatus ResultStatus { get; set; }
}