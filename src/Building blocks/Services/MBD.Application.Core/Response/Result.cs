namespace MBD.Application.Core.Response
{
    public class Result : IResult
    {
        protected Result() { }

        public string Message { get; protected set; }

        public bool Succeeded { get; protected set; }

        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Message = message };
        }

        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }

        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Message = message };
        }
    }

    public class Result<TData> : Result, IResult<TData>
    {
        public TData Data { get; protected set; }

        public static new IResult<TData> Success()
        {
            return new Result<TData> { Succeeded = true };
        }

        public static new IResult<TData> Success(string message)
        {
            return new Result<TData> { Succeeded = true, Message = message };
        }

        public static IResult<TData> Success(TData data)
        {
            return new Result<TData> { Succeeded = true, Data = data };
        }

        public static IResult<TData> Success(TData data, string message)
        {
            return new Result<TData> { Succeeded = true, Message = message, Data = data };
        }

        public static new IResult<TData> Fail()
        {
            return new Result<TData> { Succeeded = false };
        }

        public static new IResult<TData> Fail(string message)
        {
            return new Result<TData> { Succeeded = false, Message = message };
        }
    }
}