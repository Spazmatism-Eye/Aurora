using System.Drawing;

namespace Aurora_Updater.Data;

public class LogEntry
{
    public string Message { get; set; }

    private readonly Color _color;

    public LogEntry()
    {
        Message = string.Empty;
        _color = Color.Black;
    }

    public LogEntry(string message)
    {
        Message = message;
        _color = Color.Black;
    }

    public LogEntry(string message, Color color)
    {
        Message = message;
        _color = color;
    }

    public Color GetColor()
    {
        return _color;
    }

    public override string ToString()
    {
        return Message;
    }
}