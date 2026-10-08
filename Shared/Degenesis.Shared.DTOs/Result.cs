namespace Degenesis.Shared.DTOs;
public class Result<T>
{
    public bool IsError { get; set; }
    public string Error { get; set; } = string.Empty;
    public T? Value { get; set; }
}