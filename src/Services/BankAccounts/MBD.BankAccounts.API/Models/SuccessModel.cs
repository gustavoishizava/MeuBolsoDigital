using MBD.Application.Core.Response;

namespace MBD.BankAccounts.API.Models
{
    public class SuccessModel<TData>
    {
        public string Message { get; private set; }
        public TData Data { get; private set; }

        public SuccessModel(IResult<TData> result)
        {
            Data = result.Data;
            Message = result.Message;
        }
    }
}