namespace MBD.Application.Core.Response
{
    public interface IResult
    {
        string Message { get; }
        bool Succeeded { get; }
    }

    public interface IResult<out TData> : IResult
    {
        TData Data { get; }
    }
}